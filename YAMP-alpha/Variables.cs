using CSCore.Streams;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.Streams.Effects;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YAMP_alpha
{
    public static class YAMPVars
    {
        public static YAMP_Core CORE;
        public static MMDevice MediaDevice;
        public static DmoCompressorEffect CompressorEffect;
        public static DmoWavesReverbEffect WavesReverbEffect;
        public static PeakMeter AudioPeakMeter = null;
        public static DmoFlangerEffect FlangerEffect;
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
    }
}
