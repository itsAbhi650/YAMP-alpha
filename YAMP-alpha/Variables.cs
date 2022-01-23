using CSCore.Streams;
using CSCore;
using CSCore.CoreAudioAPI;

namespace YAMP_alpha
{
    public static class YAMPVars
    {
        public static YAMP_Core CORE;
        public static VolumeSource VolumeSource;
        public static GainSource GainSource;
        public static SingleBlockNotificationStream singleBlockNotificationStream;
        public static SimpleNotificationSource simpleNotificationSource;
        public static NotificationSource NotificationSource;
        public static string[] ValidBitrates;
        public static AudioMeterInformation MeterInformation;
        public static CSCore.DSP.FftProvider FftProvider;
        //internal static NotificationSource notificationSource;
    }
}
