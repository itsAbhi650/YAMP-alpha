using CSCore;
using System;

namespace YAMP_alpha
{
    public sealed class TransitionTrackSwitchedEventArgs : EventArgs
    {
        public TransitionTrackSwitchedEventArgs(TrackInfo track)
        {
            Track = track;
        }

        public TrackInfo Track { get; private set; }
    }

    public sealed class PlaybackTransitionEngine : IDisposable
    {
        private readonly object _sync = new object();
        private GaplessCrossfadeWaveSource _transitionSource;
        private IWaveSource _source;
        private TrackInfo _queuedTrack;
        private bool _disposed;

        public event EventHandler NextTrackRequested;
        public event EventHandler<TransitionTrackSwitchedEventArgs> TrackSwitched;
        public event EventHandler StreamEnded;

        public bool IsActive
        {
            get { return _transitionSource != null; }
        }

        public IWaveSource Source
        {
            get
            {
                lock (_sync)
                {
                    return _source;
                }
            }
        }

        public void Initialize(IWaveSource current, TransitionEngineSettings settings)
        {
            ReplaceCurrent(current, settings);
        }

        public void ReplaceCurrent(IWaveSource current, TransitionEngineSettings settings)
        {
            if (current == null)
                throw new ArgumentNullException("current");

            lock (_sync)
            {
                EnsureNotDisposed();

                if (settings != null && settings.EnableGaplessPlayback)
                {
                    if (_transitionSource != null)
                    {
                        // Keep the existing wrapper object so downstream player/effect chains remain valid.
                        _transitionSource.ReplaceCurrent(current);
                    }
                    else
                    {
                        DisposeCurrent_NoLock();
                        _transitionSource = new GaplessCrossfadeWaveSource(
                            current,
                            settings.EnableCrossfadeOverlap,
                            settings.RequestNextTrackMilliseconds,
                            settings.CrossfadeMilliseconds);
                        _transitionSource.NextTrackRequested += TransitionSource_NextTrackRequested;
                        _transitionSource.TrackSwitched += TransitionSource_TrackSwitched;
                        _transitionSource.StreamEnded += TransitionSource_StreamEnded;
                    }

                    _source = _transitionSource;
                }
                else
                {
                    DisposeCurrent_NoLock();
                    _source = current;
                }

                _queuedTrack = null;
            }
        }

        public bool QueueNext(TrackInfo track, IWaveSource nextSource)
        {
            if (track == null || nextSource == null)
                return false;

            lock (_sync)
            {
                EnsureNotDisposed();

                if (_transitionSource == null)
                    return false;

                if (_transitionSource.QueueNext(nextSource))
                {
                    _queuedTrack = track;
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            lock (_sync)
            {
                DisposeCurrent_NoLock();
                _queuedTrack = null;
            }
        }

        public void Dispose()
        {
            lock (_sync)
            {
                if (_disposed)
                    return;

                _disposed = true;
                DisposeCurrent_NoLock();
                _queuedTrack = null;
            }
        }

        private void TransitionSource_NextTrackRequested(object sender, EventArgs e)
        {
            EventHandler handler = NextTrackRequested;
            if (handler != null)
                handler(this, e);
        }

        private void TransitionSource_TrackSwitched(object sender, EventArgs e)
        {
            TransitionTrackSwitchedEventArgs args = null;
            lock (_sync)
            {
                args = new TransitionTrackSwitchedEventArgs(_queuedTrack);
                _queuedTrack = null;
            }

            EventHandler<TransitionTrackSwitchedEventArgs> handler = TrackSwitched;
            if (handler != null)
                handler(this, args);
        }

        private void TransitionSource_StreamEnded(object sender, EventArgs e)
        {
            EventHandler handler = StreamEnded;
            if (handler != null)
                handler(this, e);
        }

        private void DisposeCurrent_NoLock()
        {
            IWaveSource oldSource = _source;
            GaplessCrossfadeWaveSource oldTransitionSource = _transitionSource;

            if (_transitionSource != null)
            {
                try { _transitionSource.NextTrackRequested -= TransitionSource_NextTrackRequested; } catch { }
                try { _transitionSource.TrackSwitched -= TransitionSource_TrackSwitched; } catch { }
                try { _transitionSource.StreamEnded -= TransitionSource_StreamEnded; } catch { }
                try { _transitionSource.Dispose(); } catch { }
            }

            if (oldSource != null && !ReferenceEquals(oldSource, oldTransitionSource))
            {
                try { oldSource.Dispose(); } catch { }
            }

            _transitionSource = null;
            _source = null;
        }

        private void EnsureNotDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("PlaybackTransitionEngine");
        }
    }
}
