using CSCore.CoreAudioAPI;
using CSCore.DSP;
using CSCore.Streams;
using CSCore.Streams.Effects;
//using KoenZomers.OneDrive.Api;
using System.Collections.Generic;
using YAMP_alpha.Controls;

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
        public static DmoFlangerEffect FlangerEffect;
        public static DmoDistortionEffect DistortionEffect;
        internal static FftProvider FftProvider;
        internal static BiQuadFiltersSource biQuadFilterSrc;
        public static DmoEchoEffect EchoEffect;
        public static DmoChorusEffect ChorusEffect;
        public static DmoGargleEffect GargleEffect;
        public static PitchShifter PitchShiftEffect;
        public static AudioMeterInformation MeterInformation;
        public static AudioSessionManager2 AudioSessionManager;
        public static AudioSessionEnumerator SessionEnumerator;
        public static SimpleNotificationSource SimpleNotificationSource;
        public static List<TrackInfo> TrackList = new List<TrackInfo>();
        public static string[] ValidBitrates;
        internal static bool PLTRACKFLAG;
        public static bool DrawLeftChannelSpectrum = true;
        public static bool DrawRightChannelSpectrum = true;
        internal static PositionLoop TrackPositionLoop;
        internal static EQBand[] FrequencyBands = null;
        //internal static OneDriveConsumerApi OneDriveApi;

        public static void ResetEffectVars()
        {
            CompressorEffect = null;
            WavesReverbEffect = null;
            FlangerEffect = null;
            EchoEffect = null;
            ChorusEffect = null;
            GargleEffect = null;
            PitchShiftEffect = null;
        }

        public static void ResetStreamNotifications()
        {
            SimpleNotificationSource = null;
        }
    }
}


