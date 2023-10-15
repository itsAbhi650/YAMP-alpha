using CSCore.CoreAudioAPI;
using CSCore.DSP;
using CSCore.Streams;
using CSCore.Streams.Effects;
using KoenZomers.OneDrive.Api;
using System.Collections.Generic;

namespace YAMP_alpha
{
    public static class YAMPVars
    {
        public static YAMP_Core CORE;
        public static DownloadProgress DownloadProgress = new DownloadProgress();
        internal static string LoadedPlaylist;
        public static MMDevice MediaDevice;
        public static DmoCompressorEffect CompressorEffect;
        public static DmoWavesReverbEffect WavesReverbEffect;
        public static PeakMeter AudioPeakMeter = null;
        public static DmoFlangerEffect FlangerEffect;
        public static DmoDistortionEffect DistortionEffect;
        internal static FftProvider FftProvider;
        internal static BiQuadFiltersSource biQuadFilterSrc;
        public static DmoEchoEffect EchoEffect;
        public static DmoChorusEffect ChorusEffect;
        public static Equalizer EqualizerEffect;
        public static DmoGargleEffect GargleEffect;
        public static PitchShifter PitchShiftEffect;
        public static GainSource GainSource;
        internal static FadeInOut FadeEffect;
        public static VolumeSource VolumeSource;
        public static AudioMeterInformation MeterInformation;
        public static AudioSessionManager2 AudioSessionManager;
        public static AudioSessionEnumerator SessionEnumerator;
        public static SingleBlockNotificationStream SingleBlockNotificationStream;
        public static SimpleNotificationSource SimpleNotificationSource;
        public static NotificationSource NotificationSource;
        public static List<TrackInfo> TrackList = new List<TrackInfo>();
        public static string[] ValidBitrates;
        internal static bool PLTRACKFLAG;
        internal static LoopStream TrackLoop;
        internal static PanSource ChannelPan;
        internal static PositionLoop TrackPositionLoop;
        internal static OneDriveConsumerApi OneDriveApi;

        public static void ResetEffectVars()
        {
            CompressorEffect = null;
            WavesReverbEffect = null;
            FlangerEffect = null;
            EchoEffect = null;
            ChorusEffect = null;
            EqualizerEffect = null;
            GargleEffect = null;
            PitchShiftEffect = null;
        }

        public static void ResetStreamNotifications()
        {
            SimpleNotificationSource = null;
            SingleBlockNotificationStream = null;
        }
    }
}


