// YAMP/Scratch/ScratchPlaybackSource.cs  
using CSCore;
using CSCore.DSP;
using System;
using System.Threading;

namespace YAMP.Scratch
{
    /// <summary>  
    /// ISampleAggregator that implements true vinyl-style scratching.  
    ///  
    /// Pipeline position:  
    ///   ISampleSource (decoded audio)  
    ///     → ScratchPlaybackSource   ← sits here  
    ///       → Effects chain  
    ///         → ISoundOut  
    ///  
    /// Thread safety:  
    ///   Read() is called exclusively by the audio thread.  
    ///   ScratchState is transferred via Interlocked.Exchange — no locks in Read().  
    /// </summary>  
    public sealed class ScratchPlaybackSource : SampleAggregatorBase
    {
        // ── Cache configuration ──────────────────────────────────────────────  
        private const int CacheSeconds = 2;       // seconds of PCM to keep in RAM  
        private const int InterpolationTaps = 4;       // Hermite needs 4 samples  
        private const float VelocitySmoothing = 0.5f;   // WAS 0.04f — much faster response  
        private const int CrossfadeSamples = 8;       // WAS 128 — just enough to prevent clicks
        private const float VelocityDeadZone = 0.005f;  // below this → consider stopped  
        private const float DefaultFriction = 8.0f;    // good starting point
        private readonly LowpassFilter[] _lpFilters;
        // ── Pre-allocated buffers (no alloc during playback) ─────────────────  
        private readonly float[] _cache;          // rolling PCM window  
        private readonly int _cacheFrames;    // frames (not samples) in cache  
        private readonly int _channels;
        private readonly int _sampleRate;

        // ── Cache bookkeeping ────────────────────────────────────────────────  
        private long _cacheStartFrame;   // source frame index of cache[0]  
        private int _cacheValidFrames;  // how many frames are valid in cache  
        private readonly long _sourceFrames;  // total frames in source  
        // ── DSP state (audio thread only) ────────────────────────────────────  
        private double _fractionalFrame;  // current read position in source (frames)  
        private float _smoothedVelocity; // IIR-smoothed velocity  
        private float _prevVelocity;     // for direction-change detection  
        private float[] _crossfadeBuffer; // pre-allocated crossfade scratch buffer  
        private int _crossfadeRemaining;
        private float _crossfadeGain;

        // ── Atomic state (written by UI thread, read by audio thread) ────────  
        private ScratchState _pendingState;

        // Different smoothing rates:  
        private const float TorqueSpinUp = 0.002f;  // slow spin-up (motor)  
        private const float TorqueSpinDown = 0.01f;  // faster brake  
        private const float ScratchAlpha = 0.5f;   // near-instant during active scratch

        // ── Position tracking fields (audio thread only) ─────────────────────  
        private double _trackingTarget = -1.0;       // normalised target position, -1 = inactive  
        private volatile int _hasNewTarget = 0;       // atomic flag  
        private double _pendingTarget;                // written by UI thread 

        // ── Friction / Backspin state (audio thread only) ──────────────────── 
        private FrictionMode _frictionMode = FrictionMode.None;
        private float _frictionCoeff = 0f;
        private bool _backspinActive = false;
        private bool _returnToPlayback = true;
        private float _frictionVelocity;   // the velocity being decelerated by friction  
        private bool _frictionEngaged = false; // true while friction is decelerating  

        // ── Public observable state ──────────────────────────────────────────  
        public bool IsScratching { get; private set; }
        public float ScratchVelocity => _smoothedVelocity;
        public double CurrentScratchPosition =>
                    _sourceFrames > 0 ? _fractionalFrame / _sourceFrames : 0.0;
        public bool AllowReversePlayback { get; private set; } = true;

        private readonly long _sourceLength; // total frames in source  


        public ScratchPlaybackSource(ISampleSource source) : base(source)
        {
            if (!source.CanSeek)
                throw new ArgumentException(
                    "ScratchPlaybackSource requires a seekable ISampleSource.", nameof(source));

            _channels = source.WaveFormat.Channels;
            _sampleRate = source.WaveFormat.SampleRate;
            _sourceLength = source.Length / _channels; // Length is in samples; convert to frames  
            _sourceFrames = source.Length / _channels;// ISampleSource.Length is in samples; divide by channels for frames  
            _cacheFrames = _sampleRate * CacheSeconds;
            _cache = new float[_cacheFrames * _channels];
            _crossfadeBuffer = new float[CrossfadeSamples * _channels];

            _smoothedVelocity = 1.0f;
            _prevVelocity = 1.0f;
            _fractionalFrame = source.Position / _channels;

            // Seed the cache  
            _pendingState = ScratchState.Default;
            FillCacheForward();
        }

        // ════════════════════════════════════════════════════════════════════  
        // Public API (called from UI / ScratchController — any thread)  
        // ════════════════════════════════════════════════════════════════════  

        /// <summary>Atomically posts a new scratch state to the audio thread.</summary>  
        internal void PostState(ScratchState state)
        {
            Interlocked.Exchange(ref _pendingState, state);
        }

        // ════════════════════════════════════════════════════════════════════  
        // ISampleAggregator — called exclusively by the audio thread  
        // ════════════════════════════════════════════════════════════════════  

        // The target velocity must be stored separately from _smoothedVelocity  
        private float _targetVelocity;

        // ScratchPlaybackSource.cs  

        public override int Read(float[] buffer, int offset, int count)
        {
            // Align to channel boundary  
            if (count % _channels != 0)
                count -= count % _channels;

            // ════════════════════════════════════════════════════════════════════  
            // 1. CONSUME PENDING STATE (lock-free)  
            // ════════════════════════════════════════════════════════════════════  
            var state = Interlocked.Exchange(ref _pendingState, _pendingState);
            ApplyState(state);

            // Check for new tracking target (position-based drag mode)  
            if (Interlocked.Exchange(ref _hasNewTarget, 0) == 1)
                _trackingTarget = _pendingTarget;

            int framesRequested = count / _channels;
            int framesWritten = 0;

            for (int f = 0; f < framesRequested; f++)
            {
                // ════════════════════════════════════════════════════════════════  
                // 2. VELOCITY DETERMINATION  
                // ════════════════════════════════════════════════════════════════  

                if (_frictionEngaged)
                {
                    // ── Friction / backspin deceleration (per-sample) ─────────  
                    _frictionVelocity = ApplyFriction(_frictionVelocity);

                    if (Math.Abs(_frictionVelocity) < VelocityDeadZone)
                    {
                        _frictionVelocity = 0f;
                        _frictionEngaged = false;
                        _trackingTarget = -1.0; // clear tracking if it was active  

                        if (_returnToPlayback && !IsScratching)
                            _targetVelocity = 1.0f; // motor resumes  
                        else
                            _targetVelocity = 0f;   // hold  
                    }

                    // Minimal smoothing (2-sample anti-click)  
                    _smoothedVelocity += 0.5f * (_frictionVelocity - _smoothedVelocity);
                }
                else if (IsScratching && _trackingTarget >= 0.0)
                {
                    // ── Position-tracking mode (drag scratching) ─────────────  
                    // Audio position "sticks" to the user's finger.  
                    // Velocity is computed implicitly from the gap between  
                    // current position and the target (where the finger is).  

                    double targetFrame = _trackingTarget * (_sourceFrames - 1);
                    double gap = targetFrame - _fractionalFrame;

                    // How many samples until next UI update? (~735 at 44100/60Hz)  
                    float samplesToTarget = _sampleRate / 60.0f;

                    // Implicit velocity = distance to cover / time available  
                    float implicitVelocity = (float)(gap / samplesToTarget);

                    // Light smoothing (α=0.3) — just enough to remove mouse jitter  
                    // NOT enough to kill the scratch feel  
                    _smoothedVelocity += 0.3f * (implicitVelocity - _smoothedVelocity);
                }
                else if (IsScratching)
                {
                    // ── Explicit velocity mode (SetScratchVelocity was used) ─  
                    _smoothedVelocity += 0.5f * (_targetVelocity - _smoothedVelocity);
                }
                else
                {
                    // ── Normal playback with motor torque ─────────────────────  
                    float alpha = (_smoothedVelocity < _targetVelocity)
                        ? TorqueSpinUp    // 0.002 — slow ramp up  
                        : TorqueSpinDown; // 0.01  — medium brake  
                    _smoothedVelocity += alpha * (_targetVelocity - _smoothedVelocity);
                }

                // Clamp reverse if not allowed  
                if (!AllowReversePlayback && _smoothedVelocity < 0f)
                    _smoothedVelocity = 0f;

                // ════════════════════════════════════════════════════════════════  
                // 3. DIRECTION-CHANGE CROSSFADE (anti-click)  
                // ════════════════════════════════════════════════════════════════  

                if ((_prevVelocity > 0.001f && _smoothedVelocity < -0.001f) ||
                    (_prevVelocity < -0.001f && _smoothedVelocity > 0.001f))
                {
                    if (_crossfadeRemaining == 0)
                        ArmCrossfadeAtCurrentPosition();
                }
                _prevVelocity = _smoothedVelocity;

                // ════════════════════════════════════════════════════════════════  
                // 4. CACHE MANAGEMENT  
                // ════════════════════════════════════════════════════════════════  

                EnsureCacheCoverage();

                // ════════════════════════════════════════════════════════════════  
                // 5. HERMITE INTERPOLATION → output buffer  
                // ════════════════════════════════════════════════════════════════  

                int outIdx = offset + f * _channels;
                ReadHermiteFrame(_fractionalFrame, buffer, outIdx);

                // ════════════════════════════════════════════════════════════════  
                // 6. VELOCITY-DEPENDENT AMPLITUDE SCALING  
                //    Quiet at turnaround (vel≈0), full volume at normal speed.  
                //    This is what makes it sound like vinyl — not constant volume.  
                // ════════════════════════════════════════════════════════════════  

                float absVel = Math.Abs(_smoothedVelocity);
                float gain = Math.Min(1.0f, (float)Math.Sqrt(absVel));

                for (int ch = 0; ch < _channels; ch++)
                    buffer[outIdx + ch] *= gain;

                // ════════════════════════════════════════════════════════════════  
                // 7. VELOCITY-DEPENDENT LOW-PASS FILTER  
                //    Slow movement = muffled (dark). Fast = full spectrum.  
                //    Update cutoff every 64 frames to avoid excessive coefficient calc.  
                // ════════════════════════════════════════════════════════════════  

                if (f % 64 == 0)
                {
                    double cutoff = 200.0 + absVel * (_sampleRate * 0.5 - 201.0);
                    cutoff = Math.Max(200.0, Math.Min(_sampleRate * 0.5 - 1.0, cutoff));
                    for (int ch = 0; ch < _channels; ch++)
                        _lpFilters[ch].Frequency = cutoff;
                }

                for (int ch = 0; ch < _channels; ch++)
                    buffer[outIdx + ch] = _lpFilters[ch].Process(buffer[outIdx + ch]);

                // ════════════════════════════════════════════════════════════════  
                // 8. CROSSFADE BLEND (if armed from direction change or position jump)  
                // ════════════════════════════════════════════════════════════════  

                if (_crossfadeRemaining > 0)
                {
                    float t = (float)_crossfadeRemaining / CrossfadeSamples;
                    float newWeight = 0.5f * (1.0f - (float)Math.Cos(Math.PI * (1.0 - t)));
                    float oldWeight = 1.0f - newWeight;

                    for (int ch = 0; ch < _channels; ch++)
                    {
                        int cbIdx = _crossfadeReadPos + ch;
                        if (cbIdx < _crossfadeBuffer.Length)
                        {
                            buffer[outIdx + ch] = buffer[outIdx + ch] * newWeight
                                                + _crossfadeBuffer[cbIdx] * oldWeight;
                        }
                    }
                    _crossfadeReadPos += _channels;
                    _crossfadeRemaining -= 1;
                }

                // ════════════════════════════════════════════════════════════════  
                // 9. ADVANCE FRACTIONAL READ HEAD  
                // ════════════════════════════════════════════════════════════════  

                _fractionalFrame += _smoothedVelocity;
                _fractionalFrame = Math.Max(0.0, Math.Min(_sourceFrames - 1.0, _fractionalFrame));

                framesWritten++;
            }

            return framesWritten * _channels;
        }

        /// <summary>  
        /// Applies friction deceleration to the given velocity.  
        /// Called once per sample frame (44100 or 48000 times per second).  
        ///  
        /// FrictionCoeff meaning:  
        ///   Exponential: higher = faster decay. 8.0 ≈ 0.5s to stop from normal speed.  
        ///   Linear:     deceleration in velocity-units per second. 4.0 ≈ 0.25s from vel=1.  
        ///   Combined:   both applied together.  
        /// </summary>  
        private float ApplyFriction(float velocity)
        {
            if (_frictionMode == FrictionMode.None)
                return velocity;

            float dt = 1.0f / _sampleRate;  // time per sample  

            switch (_frictionMode)
            {
                case FrictionMode.Exponential:
                    {
                        // Exponential decay: vel *= e^(-k*dt) ≈ vel * (1 - k*dt) for small dt  
                        // For k=8, half-life ≈ ln(2)/8 ≈ 0.087s  
                        float decay = 1.0f - (_frictionCoeff * dt);
                        decay = Math.Max(0f, decay);  // clamp to prevent sign flip  
                        velocity *= decay;
                        break;
                    }

                case FrictionMode.Linear:
                    {
                        // Constant deceleration: vel -= sign(vel) * k * dt  
                        // For k=4.0 at 44100Hz: loses 4.0 velocity-units per second  
                        // From vel=-3.0: stops in 0.75 seconds  
                        float decel = _frictionCoeff * dt;
                        if (velocity > 0f)
                            velocity = Math.Max(0f, velocity - decel);
                        else if (velocity < 0f)
                            velocity = Math.Min(0f, velocity + decel);
                        break;
                    }

                case FrictionMode.Combined:
                    {
                        // Apply exponential first (dominant at high speed)  
                        float decay = 1.0f - (_frictionCoeff * 0.5f * dt);
                        decay = Math.Max(0f, decay);
                        velocity *= decay;

                        // Then linear (dominant at low speed — ensures clean stop)  
                        float linearK = _frictionCoeff * 0.3f; // weaker linear component  
                        float decel = linearK * dt;
                        if (velocity > 0f)
                            velocity = Math.Max(0f, velocity - decel);
                        else if (velocity < 0f)
                            velocity = Math.Min(0f, velocity + decel);
                        break;
                    }
            }

            return velocity;
        }

        // ════════════════════════════════════════════════════════════════════  
        // State application  
        // ════════════════════════════════════════════════════════════════════  

        private void ApplyState(ScratchState state)
        {
            IsScratching = state.IsScratching;
            AllowReversePlayback = state.AllowReverse;
            _frictionMode = state.FrictionMode;
            _frictionCoeff = state.FrictionCoeff;
            _returnToPlayback = state.ReturnToPlayback;

            // Backspin trigger...  
            if (state.BackspinActive && !_backspinActive)
            {
                _frictionVelocity = state.BackspinVelocity;
                _frictionEngaged = true;
                _backspinActive = true;
            }
            else if (!state.BackspinActive)
            {
                _backspinActive = false;
            }

            if (!_frictionEngaged)
                _targetVelocity = IsScratching ? state.Velocity : 1.0f;

            // ── Position jump ────────────────────────────────────────────────  
            if (state.TargetPosition >= 0.0)
            {
                // Convert normalised [0,1] → frame index  
                double newFrame = state.TargetPosition * (_sourceFrames - 1);
                double distance = Math.Abs(newFrame - _fractionalFrame);

                if (distance > 1.0)
                {
                    // Arm crossfade: pre-render a few frames at old position  
                    // so the jump doesn't click  
                    if (distance > CrossfadeSamples)
                        ArmCrossfadeAtCurrentPosition();

                    // Jump the read head  
                    _fractionalFrame = newFrame;

                    // Refill cache centred on the new position  
                    RefillCache((long)_fractionalFrame - _cacheFrames / 2);
                }
            }
        }

        private void ArmCrossfadeAtCurrentPosition()
        {
            // Pre-render CrossfadeSamples frames at current position/velocity  
            // into _crossfadeBuffer, so we can blend old→new after the jump  
            _crossfadeRemaining = CrossfadeSamples;
            _crossfadeReadPos = 0;

            double tempPos = _fractionalFrame;
            for (int i = 0; i < CrossfadeSamples; i++)
            {
                ReadHermiteFrame(tempPos, _crossfadeBuffer, i * _channels);
                tempPos += _smoothedVelocity;
                tempPos = Math.Max(0.0, Math.Min(_sourceFrames - 1.0, tempPos));
            }
        }

        // ════════════════════════════════════════════════════════════════════  
        // Cache management  
        // ════════════════════════════════════════════════════════════════════  

        private void FillCacheForward()
        {
            long seekFrame = (long)_fractionalFrame;
            seekFrame = Math.Max(0, Math.Min(_sourceLength - 1, seekFrame));

            BaseSource.Position = seekFrame * _channels;
            _cacheStartFrame = seekFrame;

            int toRead = _cacheFrames * _channels;
            int read = BaseSource.Read(_cache, 0, toRead);
            _cacheValidFrames = read / _channels;
        }

        private void InvalidateCache()
        {
            _cacheValidFrames = 0;
        }

        private void EnsureCacheCoverage()
        {
            long frame = (long)_fractionalFrame;

            // Need margin of 2 frames for Hermite (indices -1 to +2)  
            bool covered = (frame - 1) >= _cacheStartFrame
                        && (frame + 2) < _cacheStartFrame + _cacheValidFrames;

            if (!covered)
            {
                // Centre new cache around current position  
                RefillCache(frame - _cacheFrames / 2);
            }
        }

        private void RefillCache(long centreFrame)
        {
            // Centre cache so reverse movement doesn't immediately miss  
            long startFrame = Math.Max(0L, centreFrame);
            startFrame = Math.Min(startFrame, Math.Max(0L, _sourceFrames - _cacheFrames));

            BaseSource.Position = startFrame * _channels;
            _cacheStartFrame = startFrame;

            int read = BaseSource.Read(_cache, 0, _cacheFrames * _channels);
            _cacheValidFrames = read / _channels;
        }

        // ════════════════════════════════════════════════════════════════════  
        // Hermite interpolation  
        // ════════════════════════════════════════════════════════════════════  

        /// <summary>  
        /// 4-point, 3rd-order Hermite interpolation.  
        /// Reads one interleaved frame at fractional source position <paramref name="pos"/>.  
        /// </summary>  
        private void ReadHermiteFrame(double pos, float[] dst, int dstOffset)
        {
            long baseFrame = (long)pos;
            double t = pos - baseFrame;

            for (int ch = 0; ch < _channels; ch++)
            {
                float y0 = GetCacheSample(baseFrame - 1, ch);
                float y1 = GetCacheSample(baseFrame, ch);
                float y2 = GetCacheSample(baseFrame + 1, ch);
                float y3 = GetCacheSample(baseFrame + 2, ch);

                // Catmull-Rom / Hermite coefficients  
                float c0 = y1;
                float c1 = 0.5f * (y2 - y0);
                float c2 = y0 - 2.5f * y1 + 2.0f * y2 - 0.5f * y3;
                float c3 = 0.5f * (y3 - y0) + 1.5f * (y1 - y2);

                float ft = (float)t;
                dst[dstOffset + ch] = ((c3 * ft + c2) * ft + c1) * ft + c0;
            }
        }

        private float GetCacheSample(long frame, int channel)
        {
            if (frame < 0 || frame >= _sourceLength) return 0f;

            long cacheIdx = frame - _cacheStartFrame;
            if (cacheIdx < 0 || cacheIdx >= _cacheValidFrames) return 0f;

            return _cache[cacheIdx * _channels + channel];
        }

        // ════════════════════════════════════════════════════════════════════  
        // Anti-click crossfade  
        // ════════════════════════════════════════════════════════════════════  

        private int _crossfadeReadPos;

        private void ArmCrossfade(float[] currentBuffer, int currentOffset)
        {
            // Snapshot the NEXT few samples at the OLD velocity direction  
            // so we can blend them with the new direction  
            _crossfadeRemaining = CrossfadeSamples;
            _crossfadeReadPos = 0;

            // Pre-render CrossfadeSamples frames at old velocity into _crossfadeBuffer  
            double tempPos = _fractionalFrame;
            for (int i = 0; i < CrossfadeSamples; i++)
            {
                ReadHermiteFrame(tempPos, _crossfadeBuffer, i * _channels);
                tempPos += _prevVelocity;
                tempPos = Math.Max(0.0, Math.Min(_sourceFrames - 1.0, tempPos));
            }
        }

        private void ApplyCrossfadeSample(float[] buffer, int idx)
        {
            float blend = _crossfadeGain;
            for (int ch = 0; ch < _channels; ch++)
                buffer[idx + ch] *= (1.0f - blend);

            _crossfadeGain -= 1.0f / CrossfadeSamples;
            _crossfadeRemaining -= 1;
        }

        // ════════════════════════════════════════════════════════════════════  
        // Position / Length passthrough (audio thread reads these)  
        // ════════════════════════════════════════════════════════════════════  

        public override long Position
        {
            get => (long)(_fractionalFrame) * _channels;  // frames → samples  
            set
            {
                // External seek (e.g. from playback engine seeking)  
                double newFrame = (double)value / _channels;
                if (Math.Abs(newFrame - _fractionalFrame) > 1.0)
                    ArmCrossfadeAtCurrentPosition();

                _fractionalFrame = newFrame;
                RefillCache((long)_fractionalFrame - _cacheFrames / 2);
            }
        }

        /// <summary>  
        /// Called by ScratchController at 60Hz+ during drag.  
        /// Posts a target position for the audio thread to track toward.  
        /// </summary>  
        internal void PostTrackingTarget(double normalizedTarget)
        {
            _pendingTarget = normalizedTarget;
            Interlocked.Exchange(ref _hasNewTarget, 1);
        }

        public override long Length => BaseSource.Length;
        public new bool CanSeek => true;
    }
}