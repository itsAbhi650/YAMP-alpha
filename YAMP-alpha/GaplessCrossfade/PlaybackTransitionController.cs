using CSCore;
using System;

namespace YAMP_alpha
{
    public enum AutoAdvanceFadeMode
    {
        NoFading = 0,
        AllSongs = 1,
        ShuffledSongs = 2
    }

    public enum TransitionEnginePhase
    {
        GaplessAutoAdvance = 1,
        CrossfadeWindow = 2,
        TransitionGainNormalization = 3,
        EdgeCaseGuards = 4
    }

    public sealed class TransitionEngineSettings
    {
        public bool Enabled { get; set; } = true;
        public bool EnableGaplessPlayback { get; set; } = true;
        public bool EnableCrossfadeOverlap { get; set; } = false;
        public bool AutoAdvanceOnNaturalEnd { get; set; } = true;
        public AutoAdvanceFadeMode AutoAdvanceFadeMode { get; set; } = AutoAdvanceFadeMode.AllSongs;
        // Request next track early enough to ensure overlap is ready before crossfade starts.
        public int RequestNextTrackMilliseconds { get; set; } = 10000;
        public int CrossfadeMilliseconds { get; set; } = 8000;
        public bool EnableTransitionFadeIn { get; set; } = true;
        public int FadeInMilliseconds { get; set; } = 2500;
        public bool EnableTransitionGainNormalization { get; set; } = false;
        public bool UseEdgeCaseGuards { get; set; } = true;

        public bool EnableCrossfadeWindow
        {
            get { return EnableCrossfadeOverlap; }
            set { EnableCrossfadeOverlap = value; }
        }
    }

    public sealed class PlaybackTransitionController
    {
        private readonly object _transitionLock = new object();
        private bool _transitionInProgress;

        public TransitionEngineSettings Settings { get; } = new TransitionEngineSettings();

        public TransitionEnginePhase CurrentPhase
        {
            get
            {
                if (!Settings.EnableCrossfadeWindow)
                    return TransitionEnginePhase.GaplessAutoAdvance;

                if (!Settings.EnableTransitionGainNormalization)
                    return TransitionEnginePhase.CrossfadeWindow;

                return TransitionEnginePhase.EdgeCaseGuards;
            }
        }

        public bool HandleAlmostFinished(YAMP_Core core)
        {
            if (core == null || !Settings.Enabled || !Settings.EnableGaplessPlayback)
                return false;

            if (!ShouldApplyAutoAdvanceFade(core))
                return false;

            core.SchedulePrefetchNextTrackForTransition();

            if (!Settings.EnableCrossfadeOverlap)
                return false;

            if (Settings.UseEdgeCaseGuards)
            {
                if (core.PlayerSource == null)
                    return false;

                if (core.NetPlay && !core.PlayerSource.CanSeek)
                    return false;
            }

            if (!core.EnableFade || core.FadeEffect == null)
                return false;

            TimeSpan remaining;
            try
            {
                remaining = Extensions.GetLength(core.PlayerSource) - Extensions.GetPosition(core.PlayerSource);
            }
            catch
            {
                return false;
            }

            if (remaining <= TimeSpan.Zero)
                return false;

            double fadeMs = Math.Max(50, Settings.CrossfadeMilliseconds);
            if (remaining.TotalMilliseconds < fadeMs)
            {
                fadeMs = Math.Max(remaining.TotalMilliseconds, 50D);
            }

            core.FadeEffect.FadeStrategy.StartFading(null, 0, TimeSpan.FromMilliseconds(fadeMs));
            return true;
        }

        public bool HandleNaturalTrackFinished(YAMP_Core core)
        {
            if (core == null || !Settings.Enabled || !Settings.EnableGaplessPlayback || !Settings.AutoAdvanceOnNaturalEnd)
                return false;

            if (!ShouldApplyAutoAdvanceFade(core))
                return false;

            lock (_transitionLock)
            {
                if (_transitionInProgress)
                    return true;

                _transitionInProgress = true;
            }

            try
            {
                if (Settings.UseEdgeCaseGuards)
                {
                    if (!core.IsTransitionAdvanceSafe())
                        return false;
                }

                bool advanced = core.TryAdvanceToNextTrackForTransition(Settings);
                if (!advanced)
                    return false;

                return true;
            }
            finally
            {
                lock (_transitionLock)
                {
                    _transitionInProgress = false;
                }
            }
        }

        private bool ShouldApplyAutoAdvanceFade(YAMP_Core core)
        {
            switch (Settings.AutoAdvanceFadeMode)
            {
                case AutoAdvanceFadeMode.NoFading:
                    return false;
                case AutoAdvanceFadeMode.ShuffledSongs:
                    return YAMPVars.ShuffleEnabled;
                case AutoAdvanceFadeMode.AllSongs:
                default:
                    return true;
            }
        }
    }
}
