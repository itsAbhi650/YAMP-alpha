// YAMP/Scratch/ScratchController.cs  
using System;

namespace YAMP.Scratch
{
    /// <summary>  
    /// Public API facade for the scratch system.  
    /// All methods are safe to call from any thread (UI, timer, MIDI, etc.).  
    /// Internally posts an immutable ScratchState to ScratchPlaybackSource via  
    /// Interlocked.Exchange — no locks, no blocking.  
    /// </summary>  
    public sealed class ScratchController
    {
        private readonly ScratchPlaybackSource _source;

        // Mutable scratch parameters — written by UI thread only  
        private volatile float _targetVelocity = 1.0f;
        private volatile bool _isScratching = false;
        private volatile bool _allowReverse = true;

        internal ScratchController(ScratchPlaybackSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        // ── Observable state ─────────────────────────────────────────────────  

        public bool IsScratching => _source.IsScratching;
        public float ScratchVelocity => _source.ScratchVelocity;
        public double CurrentScratchPosition => _source.CurrentScratchPosition;
        public bool AllowReversePlayback
        {
            get => _allowReverse;
            set { _allowReverse = value; Commit(); }
        }

        // ── Core scratch API ─────────────────────────────────────────────────  

        // ── Friction properties ──────────────────────────────────────────────  

        private volatile float _frictionCoeff = DefaultFriction;
        private volatile FrictionMode _frictionMode = FrictionMode.None;
        private volatile bool _returnToPlayback = true;
        private const float DefaultFriction = 8.0f;

        /// <summary>  
        /// Friction coefficient. Controls how quickly velocity decays.  
        ///   0   = no friction (infinite spin)  
        ///   4   = light friction (~0.75s backspin from -3x)  
        ///   8   = medium friction (~0.4s backspin) — recommended default  
        ///   16  = heavy friction (~0.2s backspin)  
        ///   50  = near-instant stop  
        /// </summary>  
        public float FrictionCoefficient
        {
            get => _frictionCoeff;
            set { _frictionCoeff = Math.Max(0f, value); }
        }

        // <summary>  
        /// Which friction model to use during backspin/release.  
        /// </summary>  
        public FrictionMode FrictionMode
        {
            get => _frictionMode;
            set { _frictionMode = value; }
        }

        /// <summary>  
        /// If true, after friction brings velocity to zero, the motor resumes  
        /// normal playback (velocity ramps back to +1.0 via torque).  
        /// If false, the record stays stopped until manually restarted.  
        /// </summary>  
        public bool ReturnToPlaybackAfterStop
        {
            get => _returnToPlayback;
            set { _returnToPlayback = value; }
        }

        // ── Backspin API ─────────────────────────────────────────────────────  

        /// <summary>  
        /// Trigger a backspin. The record immediately starts moving backward  
        /// at <paramref name="initialVelocity"/> and decelerates via friction  
        /// until it stops (then optionally resumes normal playback).  
        ///  
        /// Typical values:  
        ///   -1.5f = gentle backspin (short "rewind" sound)  
        ///   -3.0f = standard DJ backspin  
        ///   -5.0f = aggressive backspin (long, dramatic pitch-down sweep)  
        ///  
        /// Friction must be set to a non-None mode for deceleration to occur.  
        /// If FrictionMode is None, the backspin will spin forever at the  
        /// initial velocity until StopScratch() or SetScratchVelocity() is called.  
        /// </summary>  
        /// <param name="initialVelocity">  
        /// Starting velocity (negative = backward). Must be negative for a true backspin.  
        /// Positive values create a "forward flick" effect.  
        /// </param>  
        public void Backspin(float initialVelocity = -3.0f)
        {
            _isScratching = false; // backspin is a release gesture  
            _source.PostState(new ScratchState(
                isScratching: false,
                velocity: 0f,           // irrelevant during friction  
                targetPosition: -1.0,
                allowReverse: _allowReverse,
                frictionMode: _frictionMode == FrictionMode.None
                                      ? FrictionMode.Exponential  // default if not set  
                                      : _frictionMode,
                frictionCoeff: _frictionCoeff > 0 ? _frictionCoeff : DefaultFriction,
                backspinActive: true,
                backspinVelocity: initialVelocity,
                returnToPlayback: _returnToPlayback));
        }

        /// <summary>  
        /// Trigger a forward flick (opposite of backspin).  
        /// Record moves forward faster than normal and decelerates to normal speed.  
        /// </summary>  
        /// <param name="initialVelocity">Starting velocity (e.g. +3.0f).</param>  
        public void ForwardFlick(float initialVelocity = 3.0f)
        {
            Backspin(initialVelocity); // Same physics, just positive direction  
        }

        /// <summary>  
        /// Release the record with its current velocity and let friction decelerate it.  
        /// Useful after a scratch gesture — the record "coasts" to a stop.  
        /// </summary>  
        public void ReleaseWithFriction()
        {
            float currentVel = _source.ScratchVelocity;
            _isScratching = false;
            _source.PostState(new ScratchState(
                isScratching: false,
                velocity: 0f,
                targetPosition: -1.0,
                allowReverse: _allowReverse,
                frictionMode: _frictionMode == FrictionMode.None
                                      ? FrictionMode.Exponential
                                      : _frictionMode,
                frictionCoeff: _frictionCoeff > 0 ? _frictionCoeff : DefaultFriction,
                backspinActive: true,
                backspinVelocity: currentVel,  // coast from current velocity  
                returnToPlayback: _returnToPlayback));
        }

        // ── Updated Commit (includes friction fields) ────────────────────────  

        private void Commit() =>
            _source.PostState(new ScratchState(
                isScratching: _isScratching,
                velocity: _targetVelocity,
                targetPosition: -1.0,
                allowReverse: _allowReverse,
                frictionMode: _frictionMode,
                frictionCoeff: _frictionCoeff,
                backspinActive: false,         // not a backspin trigger  
                backspinVelocity: 0f,
                returnToPlayback: _returnToPlayback));

        /// <summary>  
        /// Engage scratch mode. Normal playback velocity is suspended;  
        /// the engine holds position until MoveScratch or SetScratchVelocity is called.  
        /// </summary>  
        public void StartScratch()
        {
            _isScratching = true;
            _grabPosition = _source.CurrentScratchPosition;  // remember where we grabbed  
            _targetVelocity = 0.0f;
            _source.PostState(new ScratchState(
                isScratching: true,
                velocity: 0f,
                targetPosition: -1.0,
                allowReverse: _allowReverse,
                frictionMode: _frictionMode,
                frictionCoeff: _frictionCoeff,
                backspinActive: false,
                backspinVelocity: 0f,
                returnToPlayback: _returnToPlayback));
        }

        /// <summary>  
        /// Release scratch mode. Playback resumes at normal speed (+1.0).  
        /// </summary>  
        public void StopScratch()
        {
            _isScratching = false;
            _targetVelocity = 1.0f;
            Commit();
        }

        /// <summary>  
        /// Apply a relative position delta from mouse/jog-wheel.  
        /// The delta is in normalised track units [0,1] PER CALL.  
        /// Internally converts to a velocity (source frames per output frame).  
        /// </summary>  
        /// <param name="delta">Normalised position change since last call.</param>  
        /// <param name="updateRateHz">How often this method is called (e.g. 60).</param>  
        public void MoveScratch(float delta, float updateRateHz = 60f)
        {
            // Convert: delta is fraction of track per call.  
            // velocity = (delta * totalFrames * updateRateHz) / sampleRate  
            // Simplified: velocity = delta * totalDurationSeconds * updateRateHz  
            // For a 3-minute track at 60 Hz:  
            //   delta=0.001 → velocity = 0.001 * 180 * 60 = 10.8 (way too fast)  
            // Better: treat delta as direct velocity multiplier already scaled by caller  

            // Simple approach: delta IS the velocity. Caller scales appropriately.  
            _targetVelocity = _allowReverse ? delta : Math.Max(0f, delta);
            Commit();
        }

        /// <summary>  
        /// Apply a relative position delta (in normalised track units).  
        /// Positive = forward, negative = backward.  
        /// Designed to be called at 60+ Hz from mouse/jog-wheel events.  
        /// </summary>  
        /// <param name="delta">Normalised position delta per update interval.</param>  
        public void MoveScratch(float delta)
        {
            // Convert delta to a velocity: delta / (1/updateRate) = delta * updateRate  
            // Caller is responsible for scaling delta to match their update rate.  
            // Here we treat delta directly as a velocity multiplier for simplicity.  
            _targetVelocity = delta;
            Commit();
        }

        /// <summary>  
        /// Set scratch velocity directly.  
        /// +1.0 = normal forward, -1.0 = normal reverse, 0.0 = hold.  
        /// </summary>  
        public void SetScratchVelocity(float velocity)
        {
            _targetVelocity = _allowReverse ? velocity : Math.Max(0f, velocity);
            Commit();
        }

        /// <summary>  
        /// Jump to a normalised position [0, 1] in the track.  
        /// 0.0 = start, 1.0 = end, 0.5 = middle.  
        /// Arms a micro-crossfade to prevent clicks.  
        /// Safe to call from any thread.  
        /// </summary>  
        public void SetScratchPosition(double normalizedPosition)
        {
            normalizedPosition = Math.Max(0.0, Math.Min(1.0, normalizedPosition));
            _source.PostState(new ScratchState(
                isScratching: _isScratching,
                velocity: _targetVelocity,
                targetPosition: normalizedPosition,   // ← signals a position jump  
                allowReverse: _allowReverse,
                frictionMode: _frictionMode,
                frictionCoeff: _frictionCoeff,
                backspinActive: false,
                backspinVelocity: 0f,
                returnToPlayback: _returnToPlayback));
        }

        // ── Internal ─────────────────────────────────────────────────────────  

        /// <summary>  
        /// Set the absolute scratch position offset from the grab point.  
        /// Called at 60Hz+ during mouse drag. The audio thread will  
        /// track toward this position each sample, computing velocity implicitly.  
        ///   
        /// Unlike SetScratchVelocity (which sets a constant speed), this makes  
        /// the audio "stick" to your finger — fast drag = fast audio, slow drag = slow audio,  
        /// stop = audio stops.  
        /// </summary>  
        /// <param name="positionOffset">  
        /// Offset in normalised track units from where the scratch started.  
        /// Positive = forward from grab point, negative = backward.  
        /// </param>  
        public void SetScratchPositionOffset(double positionOffset)
        {
            // Convert to absolute position  
            double absolutePos = _grabPosition + positionOffset;
            absolutePos = Math.Max(0.0, Math.Min(1.0, absolutePos));

            _source.PostTrackingTarget(absolutePos);
        }

        private double _grabPosition;
    }
}