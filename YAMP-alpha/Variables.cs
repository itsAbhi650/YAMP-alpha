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
        // Shared spectrum/provider objects so multiple dialogs reuse the same data
        public static BasicSpectrumProvider SharedSpectrumProvider;
        public static VoicePrint3DSpectrum SharedVoicePrint3DSpectrum;
        public static bool SpectrumProviderSubscribed = false;
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

        public static void InitializeSharedSpectrum(YAMP_Core core)
        {
            if (core == null || core.PlayerSource == null)
                return;

            int channels = core.PlayerSource.WaveFormat.Channels;
            int sampleRate = core.PlayerSource.WaveFormat.SampleRate;

            // Ensure FFT provider exists
            if (FftProvider == null)
            {
                FftProvider = new FftProvider(channels, FftSize.Fft4096);
            }

            var fftSize = FftProvider.FftSize;

            if (SharedSpectrumProvider == null)
            {
                SharedSpectrumProvider = new BasicSpectrumProvider(channels, sampleRate, fftSize);
            }

            if (SharedVoicePrint3DSpectrum == null)
            {
                SharedVoicePrint3DSpectrum = new VoicePrint3DSpectrum(fftSize)
                {
                    SpectrumProvider = SharedSpectrumProvider,
                    UseAverage = true,
                    PointCount = 200,
                    IsXLogScale = true,
                    ScalingStrategy = ScalingStrategy.Sqrt
                };
            }

            // Subscribe once to the core notification stream to feed the shared provider
            if (!SpectrumProviderSubscribed && core.SingleBlockNotificationStream != null)
            {
                core.SingleBlockNotificationStream.SingleBlockRead += (s, e) =>
                {
                    // Add samples into the shared provider
                    SharedSpectrumProvider?.Add(e.Left, e.Right);
                };
                SpectrumProviderSubscribed = true;
            }
        }
    }
}


