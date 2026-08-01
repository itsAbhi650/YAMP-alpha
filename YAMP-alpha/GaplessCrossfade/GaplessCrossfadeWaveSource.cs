using CSCore;
using CSCore.DSP;
using CSCore.Streams;
using System;

namespace YAMP_alpha
{
    public sealed class GaplessCrossfadeWaveSource : IWaveSource
    {
        private readonly GaplessCrossfadeSampleSource _engine;
        private readonly IWaveSource _waveSource;

        public event EventHandler NextTrackRequested;
        public event EventHandler TrackSwitched;
        public event EventHandler StreamEnded;

        public GaplessCrossfadeWaveSource(IWaveSource current, int crossfadeMilliseconds)
            : this(current, true, crossfadeMilliseconds, crossfadeMilliseconds)
        {
        }

        public GaplessCrossfadeWaveSource(IWaveSource current, bool enableCrossfadeOverlap, int requestNextTrackMilliseconds, int crossfadeMilliseconds)
        {
            if (current == null)
                throw new ArgumentNullException("current");

            _engine = new GaplessCrossfadeSampleSource(current, enableCrossfadeOverlap, requestNextTrackMilliseconds, crossfadeMilliseconds);
            _engine.NextTrackRequested += Engine_NextTrackRequested;
            _engine.TrackSwitched += Engine_TrackSwitched;
            _engine.StreamEnded += Engine_StreamEnded;
            _waveSource = _engine.ToWaveSource();
        }

        public bool QueueNext(IWaveSource next)
        {
            return _engine.QueueNext(next);
        }

        public void ReplaceCurrent(IWaveSource current)
        {
            _engine.ReplaceCurrent(current);
        }

        public WaveFormat WaveFormat
        {
            get { return _waveSource.WaveFormat; }
        }

        public long Position
        {
            get { return _waveSource.Position; }
            set { _waveSource.Position = value; }
        }

        public long Length
        {
            get { return _waveSource.Length; }
        }

        public bool CanSeek
        {
            get
            {
                try
                {
                    return _waveSource != null && _waveSource.CanSeek;
                }
                catch
                {
                    return false;
                }
            }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return _waveSource.Read(buffer, offset, count);
        }

        public void Dispose()
        {
            _waveSource.Dispose();
        }

        private void Engine_NextTrackRequested(object sender, EventArgs e)
        {
            EventHandler handler = NextTrackRequested;
            if (handler != null)
                handler(this, e);
        }

        private void Engine_TrackSwitched(object sender, EventArgs e)
        {
            EventHandler handler = TrackSwitched;
            if (handler != null)
                handler(this, e);
        }

        private void Engine_StreamEnded(object sender, EventArgs e)
        {
            EventHandler handler = StreamEnded;
            if (handler != null)
                handler(this, e);
        }

        private sealed class GaplessCrossfadeSampleSource : SampleAggregatorBase
        {
            private readonly object _sync = new object();
            private readonly int _crossfadeFrames;
            private readonly int _requestLeadFrames;
            private readonly bool _enableCrossfadeOverlap;
            private ISampleSource _nextSource;
            private float[] _currentBuffer;
            private float[] _nextBuffer;
            private long _overlapFramePosition;
            private bool _overlapStarted;
            private bool _nextTrackRequestedRaised;

            public event EventHandler NextTrackRequested;
            public event EventHandler TrackSwitched;
            public event EventHandler StreamEnded;

            public GaplessCrossfadeSampleSource(IWaveSource current, bool enableCrossfadeOverlap, int requestNextTrackMilliseconds, int crossfadeMilliseconds)
                : base(CreateCompatibleCurrentSource(current))
            {
                if (current == null)
                    throw new ArgumentNullException("current");

                _enableCrossfadeOverlap = enableCrossfadeOverlap;

                if (crossfadeMilliseconds < 0)
                    crossfadeMilliseconds = 0;

                if (requestNextTrackMilliseconds < 0)
                    requestNextTrackMilliseconds = 0;

                _crossfadeFrames = Math.Max(1, (BaseSource.WaveFormat.SampleRate * crossfadeMilliseconds) / 1000);
                _requestLeadFrames = Math.Max(1, (BaseSource.WaveFormat.SampleRate * requestNextTrackMilliseconds) / 1000);
            }

            public bool QueueNext(IWaveSource next)
            {
                if (next == null)
                    return false;

                lock (_sync)
                {
                    ISampleSource converted = CreateCompatibleNextSource(next, BaseSource.WaveFormat);
                    if (converted == null)
                        return false;

                    if (_nextSource != null)
                    {
                        try { _nextSource.Dispose(); } catch { }
                    }

                    _nextSource = converted;
                    _nextTrackRequestedRaised = true;
                    return true;
                }
            }

            public void ReplaceCurrent(IWaveSource current)
            {
                if (current == null)
                    throw new ArgumentNullException("current");

                lock (_sync)
                {
                    ISampleSource converted = CreateCompatibleCurrentSource(current, BaseSource.WaveFormat);
                    if (converted == null)
                        throw new InvalidOperationException("Incompatible source format.");

                    if (BaseSource != null)
                    {
                        try { BaseSource.Dispose(); } catch { }
                    }

                    if (_nextSource != null)
                    {
                        try { _nextSource.Dispose(); } catch { }
                        _nextSource = null;
                    }

                    BaseSource = converted;
                    _overlapFramePosition = 0;
                    _overlapStarted = false;
                    _nextTrackRequestedRaised = false;
                    _currentBuffer = null;
                    _nextBuffer = null;
                }
            }

            public override int Read(float[] buffer, int offset, int count)
            {
                if (buffer == null)
                    throw new ArgumentNullException("buffer");
                if (offset < 0)
                    throw new ArgumentOutOfRangeException("offset");
                if (count < 0)
                    throw new ArgumentOutOfRangeException("count");
                if (buffer.Length < offset + count)
                    throw new ArgumentException("buffer");

                int channels = Math.Max(1, WaveFormat.Channels);
                count -= count % channels;
                if (count <= 0)
                    return 0;

                EventHandler requestNext = null;
                EventHandler switched = null;
                EventHandler ended = null;

                lock (_sync)
                {
                    if (BaseSource == null)
                        return 0;

                    TryRaiseNextTrackRequest(ref requestNext);

                    if (_nextSource == null)
                    {
                        int read = BaseSource.Read(buffer, offset, count);
                        if (read == 0)
                        {
                            ended = StreamEnded;
                        }

                        if (!_nextTrackRequestedRaised)
                            TryRaiseNextTrackRequest(ref requestNext);

                        InvokeEvents(requestNext, switched, ended);
                        return read;
                    }

                    bool canSeek = CanSeekSafe_NoLock();
                    long remainingFrames = canSeek
                        ? Math.Max(0, (BaseSource.Length - BaseSource.Position) / channels)
                        : long.MaxValue;

                    if (!_overlapStarted)
                    {
                        if (!canSeek || !_enableCrossfadeOverlap || remainingFrames > _crossfadeFrames)
                        {
                            int read = BaseSource.Read(buffer, offset, count);
                            if (read == 0)
                            {
                                SwapToNext_NoLock();
                                switched = TrackSwitched;
                                read = BaseSource != null ? BaseSource.Read(buffer, offset, count) : 0;
                                if (read == 0)
                                    ended = StreamEnded;
                            }

                            InvokeEvents(requestNext, switched, ended);
                            return read;
                        }

                        _overlapStarted = true;
                        _overlapFramePosition = 0;
                    }

                    EnsureBufferCapacity(count);

                    int currentRead = BaseSource.Read(_currentBuffer, 0, count);
                    int nextRead = _nextSource.Read(_nextBuffer, 0, count);

                    int currentFrames = currentRead / channels;
                    int nextFrames = nextRead / channels;
                    int frameCount = Math.Max(currentFrames, nextFrames);

                    if (frameCount <= 0)
                    {
                        SwapToNext_NoLock();
                        switched = TrackSwitched;
                        int read = BaseSource != null ? BaseSource.Read(buffer, offset, count) : 0;
                        if (read == 0)
                            ended = StreamEnded;

                        InvokeEvents(requestNext, switched, ended);
                        return read;
                    }

                    frameCount -= frameCount % 1;
                    int samplesToWrite = frameCount * channels;

                    for (int frame = 0; frame < frameCount; frame++)
                    {
                        float progress = _crossfadeFrames <= 0
                            ? 1.0f
                            : Math.Min(1.0f, (float)(_overlapFramePosition + frame) / _crossfadeFrames);

                        float currentGain = (float)Math.Cos(progress * Math.PI * 0.5);
                        float nextGain = (float)Math.Sin(progress * Math.PI * 0.5);

                        for (int channel = 0; channel < channels; channel++)
                        {
                            int sampleIndex = frame * channels + channel;
                            float currentSample = sampleIndex < currentRead ? _currentBuffer[sampleIndex] : 0f;
                            float nextSample = sampleIndex < nextRead ? _nextBuffer[sampleIndex] : 0f;
                            buffer[offset + sampleIndex] = (currentSample * currentGain) + (nextSample * nextGain);
                        }
                    }

                    _overlapFramePosition += frameCount;

                    if (currentFrames == 0 || !_enableCrossfadeOverlap || _overlapFramePosition >= _crossfadeFrames)
                    {
                        SwapToNext_NoLock();
                        switched = TrackSwitched;
                    }

                    InvokeEvents(requestNext, switched, ended);
                    return samplesToWrite;
                }
            }

            protected override void Dispose(bool disposing)
            {
                lock (_sync)
                {
                    if (_nextSource != null)
                    {
                        try { _nextSource.Dispose(); } catch { }
                        _nextSource = null;
                    }
                }

                base.Dispose(disposing);
            }

            private void TryRaiseNextTrackRequest(ref EventHandler requestNext)
            {
                if (_nextTrackRequestedRaised || _nextSource != null || BaseSource == null)
                    return;

                if (!CanSeekSafe_NoLock())
                    return;

                int channels = Math.Max(1, WaveFormat.Channels);
                long remainingFrames = Math.Max(0, (BaseSource.Length - BaseSource.Position) / channels);
                if (remainingFrames <= _requestLeadFrames)
                {
                    _nextTrackRequestedRaised = true;
                    requestNext = NextTrackRequested;
                }
            }

            private void EnsureBufferCapacity(int required)
            {
                if (_currentBuffer == null || _currentBuffer.Length < required)
                    _currentBuffer = new float[required];

                if (_nextBuffer == null || _nextBuffer.Length < required)
                    _nextBuffer = new float[required];
            }

            private void SwapToNext_NoLock()
            {
                if (_nextSource == null)
                    return;

                try { BaseSource.Dispose(); } catch { }
                BaseSource = _nextSource;
                _nextSource = null;
                _overlapFramePosition = 0;
                _overlapStarted = false;
                _nextTrackRequestedRaised = false;
                _currentBuffer = null;
                _nextBuffer = null;
            }

            private void InvokeEvents(EventHandler requestNext, EventHandler switched, EventHandler ended)
            {
                if (requestNext != null)
                    requestNext(this, EventArgs.Empty);
                if (switched != null)
                    switched(this, EventArgs.Empty);
                if (ended != null)
                    ended(this, EventArgs.Empty);
            }

            private bool CanSeekSafe_NoLock()
            {
                ISampleSource source = BaseSource;
                if (source == null)
                    return false;

                try
                {
                    return source.CanSeek;
                }
                catch
                {
                    return false;
                }
            }

            private static ISampleSource CreateCompatibleCurrentSource(IWaveSource current)
            {
                if (current == null)
                    throw new ArgumentNullException("current");

                return NormalizeSource(current, null).ToSampleSource();
            }

            private static ISampleSource CreateCompatibleCurrentSource(IWaveSource current, WaveFormat targetFormat)
            {
                return NormalizeSource(current, targetFormat).ToSampleSource();
            }

            private static ISampleSource CreateCompatibleNextSource(IWaveSource next, WaveFormat targetFormat)
            {
                if (next == null)
                    return null;

                return NormalizeSource(next, targetFormat).ToSampleSource();
            }

            private static IWaveSource NormalizeSource(IWaveSource source, WaveFormat targetFormat)
            {
                if (source == null)
                    return null;
                if (targetFormat == null)
                    return source;

                IWaveSource normalized = source;

                if (normalized.WaveFormat.SampleRate != targetFormat.SampleRate && normalized.WaveFormat.Channels == targetFormat.Channels)
                {
                    normalized = new DmoResampler(normalized, targetFormat.SampleRate);
                }

                if (normalized.WaveFormat.Channels != targetFormat.Channels)
                {
                    ChannelMatrix matrix = ChannelMatrix.GetMatrix(normalized.WaveFormat, targetFormat);
                    if (matrix != null)
                    {
                        normalized = new DmoChannelResampler(normalized, matrix, targetFormat.SampleRate);
                    }
                    else if (targetFormat.Channels == 1)
                    {
                        normalized = normalized.ToMono();
                        if (normalized.WaveFormat.SampleRate != targetFormat.SampleRate)
                            normalized = new DmoResampler(normalized, targetFormat.SampleRate);
                    }
                    else if (targetFormat.Channels == 2)
                    {
                        normalized = normalized.ToStereo();
                        if (normalized.WaveFormat.SampleRate != targetFormat.SampleRate)
                            normalized = new DmoResampler(normalized, targetFormat.SampleRate);
                    }
                    else if (normalized.WaveFormat.SampleRate != targetFormat.SampleRate)
                    {
                        normalized = new DmoResampler(normalized, targetFormat.SampleRate);
                    }
                }

                if (normalized.WaveFormat.SampleRate != targetFormat.SampleRate)
                {
                    normalized = new DmoResampler(normalized, targetFormat.SampleRate);
                }

                if (normalized.WaveFormat.Channels != targetFormat.Channels)
                    throw new InvalidOperationException("Incompatible audio format for overlap source.");

                return normalized;
            }
        }
    }
}