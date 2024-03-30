using CSCore;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            CSCore.MediaFoundation.MediaFoundationEncoder encoder = GetEncoder(extension, waveSource.WaveFormat, targetStream, bitRate);
            return encoder;
        }

        public static CSCore.MediaFoundation.MediaFoundationEncoder GetEncoder(string sourceFile, string targetFile, out IWaveSource source, int bitRate = 192000) => GetEncoder(sourceFile, new FileStream(targetFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None), out source, bitRate);

        public static CSCore.MediaFoundation.MediaFoundationEncoder GetEncoder(string type, WaveFormat Waveformat, Stream targetStream, int bitRate = 192000)
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
                    encoder = CSCore.MediaFoundation.MediaFoundationEncoder.CreateAACEncoder(Waveformat, targetStream, bitRate);
                    break;
                case ".mp3":
                case ".mpeg3":
                    encoder = CSCore.MediaFoundation.MediaFoundationEncoder.CreateMP3Encoder(Waveformat, targetStream, bitRate);
                    break;
                case ".asf":
                case ".wm":
                case ".wmv":
                case ".wma":
                    encoder = CSCore.MediaFoundation.MediaFoundationEncoder.CreateWMAEncoder(Waveformat, targetStream, bitRate);
                    break;
            }
            if (encoder == null)
            {
                throw new Exception("Suitable encoder for the file type not found.");
            }
            return encoder;
        }

        public static CSCore.MediaFoundation.MediaFoundationEncoder GetEncoder(string type, WaveFormat Waveformat, string targetFile, int bitRate = 192000)
        {
            FileStream targetStream = null;
            if (File.Exists(targetFile))
            {
                targetStream = File.Open(targetFile, FileMode.Open);
                targetStream.SetLength(0);
                targetStream.Close();
            }
            targetStream = new FileStream(targetFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            return GetEncoder(type, Waveformat, targetStream, bitRate);
        }

        public static void PerformOperation(CSCore.MediaFoundation.MediaFoundationEncoder encoder, IReadableAudioSource<byte> sourceToEncode, IProgress<int> p = null)
        {
            bool ResetOnce = true;
            byte[] buffer = new byte[sourceToEncode.WaveFormat.BytesPerSecond];
            while (buffer.Length != 0)
            {
                int readcount = sourceToEncode.Read(buffer, 0, buffer.Length);
                encoder.Write(buffer, 0, buffer.Length);
                if (p != null)
                {
                    if (ResetOnce)
                    {
                        p.Report(0);
                        ResetOnce = false;
                    }
                    float perc = (float)(sourceToEncode.Position / (double)sourceToEncode.Length);
                    p.Report((int)Math.Floor(perc * 100));
                }
                if (sourceToEncode.Position + buffer.Length > sourceToEncode.Length)
                {
                    buffer = new byte[sourceToEncode.Length - sourceToEncode.Position];
                }
            }
            encoder.Dispose();
        }

        public static int GetSampleRate(string SourcePath)
        {
            if (string.IsNullOrEmpty(SourcePath))
            {
                IWaveSource source = CSCore.Codecs.CodecFactory.Instance.GetCodec(SourcePath);
                return source.WaveFormat.SampleRate;
            }
            else
            {
                throw new ArgumentNullException("Source file cannot be null or empty");
            }
        }

        public static int GetBitRate(string SourcePath)
        {
            if (!string.IsNullOrEmpty(SourcePath))
            {
                IWaveSource source = CSCore.Codecs.CodecFactory.Instance.GetCodec(SourcePath);
                return source.WaveFormat.BytesPerSecond * source.WaveFormat.Channels;
            }
            else
            {
                throw new ArgumentNullException("Source file cannot be null or empty");
            }
        }

        public static void Resample(string SourcePath, string DestinationPath, int SampleRate, IProgress<int> p = null)
        {
            var resampler = GetEncoder(new FileInfo(SourcePath).Extension, DestinationPath, out IWaveSource source);
            PerformOperation(resampler, source, p: p);
        }

        public static async Task TrackCutAsync(string SourcePath, string DestPath, double CutFrom, double CutTo, bool CopyTag = false, IProgress<int> p = null)
        {
            var Source = CSCore.Codecs.CodecFactory.Instance.GetCodec(SourcePath);
            IWaveSource cutSource = null;
            CSCore.MediaFoundation.MediaFoundationEncoder Cutter;
            if (p != null)
            {
                p.Report(0);
            }
            await Task.Run(delegate
            {
                var CutBeginElement = Extensions.GetRawElements(Source, (long)TimeSpan.FromSeconds(CutFrom).TotalMilliseconds);
                var CutEndElement = Extensions.GetRawElements(Source, (long)TimeSpan.FromSeconds(CutTo).TotalMilliseconds);
                cutSource = GetCutSource(Source, CutBeginElement, CutEndElement, p);
            }).ContinueWith(delegate
            {
                Cutter = GetEncoder(new FileInfo(SourcePath).Extension, Source.WaveFormat, DestPath, Source.WaveFormat.BytesPerSecond * 2);
                PerformOperation(Cutter, cutSource, p: p);
                cutSource.Dispose();
            }).ContinueWith(delegate
            {
                if (CopyTag)
                {
                    TagCopy(SourcePath, DestPath);
                }
            });
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
            bool ResetOnce = true;
            Source.Position = CutFrom;
            MemoryStream memStream = new MemoryStream();
            byte[] buffer = new byte[Source.WaveFormat.BytesPerSecond];
            while (buffer.Length != 0)
            {
                int count = Source.Read(buffer, 0, buffer.Length);
                if (count <= 0)
                {
                    return null;
                }
                memStream.Write(buffer, 0, count);
                if (p != null)
                {
                    if (ResetOnce)
                    {
                        p.Report(0);
                        ResetOnce = false;
                    }
                    float perc = (float)(Source.Position / (double)CutTo);
                    p.Report((int)Math.Floor(perc * 100));
                }
                if (CutTo - Source.Position < Source.WaveFormat.BytesPerSecond)
                {
                    buffer = new byte[CutTo - Source.Position];
                }
            }
            memStream.Position = 0;
            return new CSCore.Codecs.RAW.RawDataReader(memStream, Source.WaveFormat);
        }
        #endregion
    }
}
