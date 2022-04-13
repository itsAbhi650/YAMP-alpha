using CSCore.CoreAudioAPI;
using CSCore.Streams;
using CSCore.Streams.Effects;
using System.Collections.Generic;

namespace YAMP_alpha
{
    public static class YAMPVars
    {
        public static YAMP_Core CORE;
        public static DownloadProgress DownloadProgress = new DownloadProgress();
        public static MMDevice MediaDevice;
        public static DmoCompressorEffect CompressorEffect;
        public static DmoWavesReverbEffect WavesReverbEffect;
        public static PeakMeter AudioPeakMeter = null;
        public static DmoFlangerEffect FlangerEffect;
        public static DmoDistortionEffect DistortionEffect;
        public static DmoEchoEffect EchoEffect;
        public static DmoChorusEffect ChorusEffect;
        public static Equalizer EqualizerEffect;
        public static DmoGargleEffect GargleEffect;
        public static PitchShifter PitchShiftEffect;
        public static GainSource GainSource;
        public static VolumeSource VolumeSource;
        public static AudioMeterInformation MeterInformation;
        public static AudioSessionManager2 AudioSessionManager;
        public static AudioSessionEnumerator SessionEnumerator;
        public static SingleBlockNotificationStream SingleBlockNotificationStream;
        public static SimpleNotificationSource SimpleNotificationSource;
        public static NotificationSource NotificationSource;
        public static List<TrackInfo> TrackList = new List<TrackInfo>();
        public static string[] ValidBitrates;
        public static CSCore.DSP.FftProvider FftProvider;
        internal static bool PLTRACKFLAG;
        internal static LoopStream TrackLoop;

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
            //NotificationSource = null;
        }
    }
}


