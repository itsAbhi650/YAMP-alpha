using CSCore;
using CSCore.DSP;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YAMP_alpha
{
    public static class Encoder
    {

        #region properties
        public static int[] MpegSampleRates
        {
            get
            {
                return GetSupportedSampleRates(AudioSubTypes.Mpeg).ToArray();
            }
        }

        public static int[] Mpeg3SampleRates
        {
            get
            {
                return GetSupportedSampleRates(AudioSubTypes.MpegLayer3).ToArray();
            }
        }

        public static int[] MpegHEAACSampleRates
        {
            get
            {
                return GetSupportedSampleRates(AudioSubTypes.MPEG_HEAAC).ToArray();
            }
        }

        public static int[] FLACSampleRates
        {
            get
            {
                return GetSupportedSampleRates(AudioSubTypes.WAVE_FORMAT_FLAC).ToArray();
            }
        }

        public static int[] WmaSampleRates
        {
            get
            {
                return GetSupportedSampleRates(AudioSubTypes.WindowsMediaAudio).ToArray();
            }
        }

        public static int[] WmaProffesionSampleRates
        {
            get
            {
                return GetSupportedSampleRates(AudioSubTypes.WindowsMediaAudioProfessional).ToArray();
            }
        }

        public static int[] WmaVoice9SampleRates
        {
            get
            {
                return GetSupportedSampleRates(AudioSubTypes.WmaVoice9).ToArray();
            }
        }
        #endregion

        #region methods
        public static IEnumerable<int> GetSupportedSampleRates(Guid subType)
        {
            return
                   (from mediaTyoe in CSCore.MediaFoundation.MediaFoundationEncoder.GetEncoderMediaTypes(subType)
                    where mediaTyoe.Channels == 2
                    orderby mediaTyoe.SampleRate ascending
                    select mediaTyoe.SampleRate).Distinct();
        }

        public static IEnumerable<int> GetSupportedBitRates(Guid subType)
        {
            return
                   (from mediaTyoe in CSCore.MediaFoundation.MediaFoundationEncoder.GetEncoderMediaTypes(subType)
                    where mediaTyoe.Channels == 2
                    orderby mediaTyoe.SampleRate ascending
                    select mediaTyoe.AverageBytesPerSecond * 8).Distinct();
        }

        public static CSCore.MediaFoundation.MediaFoundationEncoder GetEncoder(string sourceFile, Stream targetStream, out IWaveSource waveSource, int bitRate = 192000)
        {
            var extension = new FileInfo(sourceFile).Extension;
            waveSource = CSCore.Codecs.CodecFactory.Instance.GetCodec(sourceFile);
            return GetEncoder(extension, waveSource.WaveFormat, targetStream, bitRate);
        }

        public static CSCore.MediaFoundation.MediaFoundationEncoder GetEncoder(string sourceFile, string targetFile, out IWaveSource source, int bitRate = 192000)
        {
            source = CSCore.Codecs.CodecFactory.Instance.GetCodec(sourceFile);
            return GetEncoder(new FileInfo(sourceFile).Extension, source.WaveFormat, targetFile, bitRate);
        }

        public static CSCore.MediaFoundation.MediaFoundationEncoder GetEncoder(string type, WaveFormat waveformat, Stream targetStream, int bitRate = 192000)
        {
            CSCore.MediaFoundation.MediaFoundationEncoder encoder = null;
            switch (type)
            {
                case ".aac":
                case ".adt":
                case ".mp2":
                case ".3g2":
                case ".3gp":
                case ".m4a":
                case ".m4v":
                case ".mp4":
                case ".mov":
                case ".m2ts":
                case ".adts":
                case ".3gp2":
                case ".3gpp":
                case ".mp4v":
                    encoder = CSCore.MediaFoundation.MediaFoundationEncoder.CreateAACEncoder(waveformat, targetStream, bitRate);
                    break;
                case ".mp3":
                case ".mpeg3":
                    encoder = CSCore.MediaFoundation.MediaFoundationEncoder.CreateMP3Encoder(waveformat, targetStream, bitRate);
                    break;
                case ".asf":
                case ".wm":
                case ".wmv":
                case ".wma":
                    encoder = CSCore.MediaFoundation.MediaFoundationEncoder.CreateWMAEncoder(waveformat, targetStream, bitRate);
                    break;
            }
            if (encoder == null)
            {
                throw new Exception("Suitable encoder for the file type not found.");
            }
            return encoder;
        }

        public static CSCore.MediaFoundation.MediaFoundationEncoder GetEncoder(string type, WaveFormat waveformat, string targetFile, int bitRate = 192000)
        {
            FileStream targetStream = new FileStream(targetFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            return GetEncoder(type, waveformat, targetStream, bitRate);
        }

        public static void PerformOperation(CSCore.MediaFoundation.MediaFoundationEncoder encoder, IReadableAudioSource<byte> sourceToEncode, IProgress<int> p = null)
        {
            bool resetOnce = true;
            byte[] buffer = new byte[sourceToEncode.WaveFormat.BytesPerSecond];
            long totalWritten = 0;
            int readcount;

            while ((readcount = sourceToEncode.Read(buffer, 0, buffer.Length)) > 0)
            {
                encoder.Write(buffer, 0, readcount);
                totalWritten += readcount;
                if (p != null)
                {
                    if (resetOnce)
                    {
                        p.Report(0);
                        resetOnce = false;
                    }

                    p.Report(GetProgressPercentage(sourceToEncode.Position, sourceToEncode.Length));
                }
            }

            Debug.Assert(sourceToEncode.Length <= 0 || totalWritten <= sourceToEncode.Length, "Encoder wrote more bytes than the source exposed.");
        }

        public static int GetSampleRate(string SourcePath)
        {
            if (string.IsNullOrEmpty(SourcePath))
            {
                throw new ArgumentNullException("Source file cannot be null or empty");
            }

            using (IWaveSource source = CSCore.Codecs.CodecFactory.Instance.GetCodec(SourcePath))
            {
                return source.WaveFormat.SampleRate;
            }
        }

        public static int GetBitRate(string SourcePath)
        {
            if (string.IsNullOrEmpty(SourcePath))
            {
                throw new ArgumentNullException("Source file cannot be null or empty");
            }

            try
            {
                using (TagLib.File tagFile = TagLib.File.Create(SourcePath))
                {
                    if (tagFile.Properties != null && tagFile.Properties.AudioBitrate > 0)
                    {
                        return tagFile.Properties.AudioBitrate * 1000;
                    }
                }
            }
            catch
            {
            }

            using (IWaveSource source = CSCore.Codecs.CodecFactory.Instance.GetCodec(SourcePath))
            {
                return source.WaveFormat.BytesPerSecond * 8;
            }
        }

        public static void Resample(string SourcePath, string DestinationPath, int SampleRate, IProgress<int> p = null)
        {
            string extension = new FileInfo(SourcePath).Extension;
            Guid subType = GetSubTypeForExtension(extension);
            if (!GetSupportedSampleRates(subType).Contains(SampleRate))
            {
                throw new ArgumentOutOfRangeException(nameof(SampleRate), "The selected sample rate is not supported by the target encoder.");
            }

            using (IWaveSource source = CSCore.Codecs.CodecFactory.Instance.GetCodec(SourcePath))
            using (DmoResampler resampler = new DmoResampler(source, SampleRate))
            using (CSCore.MediaFoundation.MediaFoundationEncoder encoder = GetEncoder(extension, resampler.WaveFormat, DestinationPath, GetPreferredBitRate(extension, GetBitRate(SourcePath))))
            {
                PerformOperation(encoder, resampler, p);
            }
        }

        public static async Task TrackCutAsync(string SourcePath, string DestPath, double CutFrom, double CutTo, bool CopyTag = false, IProgress<int> p = null)
        {
            if (CutTo <= CutFrom)
            {
                throw new ArgumentOutOfRangeException(nameof(CutTo), "Cut end must be greater than cut start.");
            }

            if (p != null)
            {
                p.Report(0);
            }

            string extension = new FileInfo(SourcePath).Extension;
            int preferredBitRate = GetPreferredBitRate(extension, GetBitRate(SourcePath));

            using (IWaveSource source = CSCore.Codecs.CodecFactory.Instance.GetCodec(SourcePath))
            {
                long sourceLength = source.Length;
                long cutBeginElement = Extensions.GetRawElements(source, (long)TimeSpan.FromSeconds(CutFrom).TotalMilliseconds);
                long cutEndElement = Math.Min(Extensions.GetRawElements(source, (long)TimeSpan.FromSeconds(CutTo).TotalMilliseconds), sourceLength);

                if (cutBeginElement < 0 || cutBeginElement >= sourceLength)
                {
                    throw new ArgumentOutOfRangeException(nameof(CutFrom), "Cut start is outside the source stream.");
                }

                IWaveSource cutSource = await Task.Run(() => GetCutSource(source, cutBeginElement, cutEndElement, p)).ConfigureAwait(false);
                if (cutSource == null)
                {
                    throw new InvalidOperationException("Unable to read the requested audio range.");
                }

                using (cutSource)
                using (CSCore.MediaFoundation.MediaFoundationEncoder cutter = GetEncoder(extension, source.WaveFormat, DestPath, preferredBitRate))
                {
                    PerformOperation(cutter, cutSource, p);
                }

                if (CopyTag)
                {
                    TagCopy(SourcePath, DestPath);
                }
            }
        }

        /// <summary>
        /// Copy ID3 Tags from Source audio files to Target audio file.
        /// </summary>
        /// <param name="CopyFrom"></param>
        /// <param name="CopyTo"></param>
        public static void TagCopy(string CopyFrom, string CopyTo)
        {
            TagLib.File TagSource = TagLib.File.Create(CopyFrom);
            TagLib.File TagDest = TagLib.File.Create(CopyTo);

            try
            {
                if (TagDest.Writeable)
                {
                    TagSource.Tag.CopyTo(TagDest.Tag, true);
                    TagDest.Save();
                }
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Cannot write tags", ex);
            }
            finally
            {
                TagDest.Dispose();
                TagSource.Dispose();
            }
        }

        /// <summary>
        /// Get an IWaveSource containing cut portion of the original audio.
        /// </summary>
        /// <param name="Source">Original audio to cut from.</param>
        /// <param name="CutFrom">Position from where the cut begin.</param>
        /// <param name="CutTo">Position to where the cut end.</param>
        /// <param name="p">Report progress of cut operation. (Optional)</param>
        /// <returns></returns>
        private static IWaveSource GetCutSource(IWaveSource Source, long CutFrom, long CutTo, IProgress<int> p = null)
        {
            if (CutTo <= CutFrom)
            {
                throw new ArgumentOutOfRangeException(nameof(CutTo), "Cut end must be greater than cut start.");
            }

            bool resetOnce = true;
            Source.Position = CutFrom;
            MemoryStream memStream = new MemoryStream();
            byte[] buffer = new byte[Source.WaveFormat.BytesPerSecond];

            while (Source.Position < CutTo)
            {
                int bytesRemaining = (int)Math.Min(buffer.Length, CutTo - Source.Position);
                int count = Source.Read(buffer, 0, bytesRemaining);
                if (count <= 0)
                {
                    break;
                }
                memStream.Write(buffer, 0, count);
                if (p != null)
                {
                    if (resetOnce)
                    {
                        p.Report(0);
                        resetOnce = false;
                    }

                    p.Report(GetProgressPercentage(Source.Position - CutFrom, CutTo - CutFrom));
                }
            }

            if (memStream.Length == 0)
            {
                memStream.Dispose();
                return null;
            }

            memStream.Position = 0;
            return new CSCore.Codecs.RAW.RawDataReader(memStream, Source.WaveFormat);
        }

        private static int GetProgressPercentage(long position, long total)
        {
            if (total <= 0)
            {
                return 100;
            }

            double ratio = Math.Min(1D, Math.Max(0D, position / (double)total));
            return (int)Math.Floor(ratio * 100D);
        }

        private static Guid GetSubTypeForExtension(string extension)
        {
            switch (extension.ToLowerInvariant())
            {
                case ".aac":
                case ".adt":
                case ".mp2":
                case ".3g2":
                case ".3gp":
                case ".m4a":
                case ".m4v":
                case ".mp4":
                case ".mov":
                case ".m2ts":
                case ".adts":
                case ".3gp2":
                case ".3gpp":
                case ".mp4v":
                    return AudioSubTypes.Mpeg;
                case ".mp3":
                case ".mpeg3":
                    return AudioSubTypes.MpegLayer3;
                case ".asf":
                case ".wm":
                case ".wmv":
                case ".wma":
                    return AudioSubTypes.WindowsMediaAudio;
                default:
                    throw new NotSupportedException("Suitable encoder for the file type not found.");
            }
        }

        private static int GetPreferredBitRate(string extension, int requestedBitRate)
        {
            int[] supportedBitRates = GetSupportedBitRates(GetSubTypeForExtension(extension)).OrderBy(x => x).ToArray();
            if (supportedBitRates.Length == 0)
            {
                return requestedBitRate;
            }

            return supportedBitRates
                .OrderBy(x => Math.Abs((long)x - requestedBitRate))
                .ThenBy(x => x)
                .First();
        }
        #endregion
    }
}
