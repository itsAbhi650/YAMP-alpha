using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.Streams;
using CSCore.Streams.Effects;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YAMP_alpha
{
    public class YAMP_Core : IDisposable
    {
        private MMDevice MediaDevice;
        public ID3Info TagInfo { get; set; }
        public CSCore.SoundOut.ISoundOut Player { get; private set; }
        private AudioSessionEnumerator AudioSessionEnum;
        private AudioSessionManager2 SessionManager;
        private PitchShifter _PitchShift;
        private PeakMeter _AudioPeakMeter = null;
        public PeakMeter AudioPeakMeter { get { return _AudioPeakMeter; } private set { _AudioPeakMeter = value; } }
        public PitchShifter PitchShift { get { return _PitchShift; } private set { _PitchShift = value; } }
        private DmoEchoEffect _DCE;
        public DmoEchoEffect DCE { get { return _DCE; } private set { _DCE = value; } }
        public string PlayingFile { get; private set; }
        public CSCore.DSP.FftProvider FFTP;
        public DmoGargleEffect _GargleEffect;

        public IWaveSource PlayerSource { get; private set; }

        public int SoundOutVolume
        {
            get { return (int)(Player.Volume * 100F); }
            set { Player.Volume = value / 100F; }
        }

        public CSCore.SoundOut.PlaybackState PlayerPlaybackState { get { return Player.PlaybackState; } }

        public int PlayerLength { get { return (int)PlayerSource.Length; } }

        public int CurrentPosition { get { return (int)PlayerSource.Position; } }

        public YAMP_Core()
        {
            Player = new CSCore.SoundOut.DirectSoundOut();
            MediaDevice = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            Task.Run(() =>
            {
                SessionManager = AudioSessionManager2.FromMMDevice(MediaDevice);
            }).ContinueWith((t) => AudioSessionEnum = SessionManager.GetSessionEnumerator());

        }

        public void LoadFile(string Filename)
        {
            PlayingFile = Filename;
            PlayerSource = CSCore.Codecs.CodecFactory.Instance.GetCodec(PlayingFile);

        }

        private Image GetCoverImage(TagLib.File Audiofile)
        {
            Image CoverImage = null;
            TagLib.Picture CoverPicture = GetCoverPicture(Audiofile);
            if (CoverPicture != null)
            {
                MemoryStream MStream = new MemoryStream(CoverPicture.Data.Data);
                CoverImage = Image.FromStream(MStream);
            }
            return CoverImage;
        }

        private Image GetCoverImage(TagLib.Picture Picture)
        {
            Image CoverImage = null;
            if (Picture != null)
            {
                MemoryStream MStream = new MemoryStream(Picture.Data.Data);
                CoverImage = Image.FromStream(MStream);
            }
            return CoverImage;
        }

        private TagLib.Picture GetCoverPicture(TagLib.File AudioFile)
        {
            TagLib.Picture Picture = null;
            if (AudioFile.Tag.Pictures.Count() > 0)
            {
                Picture = new TagLib.Picture(AudioFile.Tag.Pictures[0]);
            }
            return Picture;
        }

        private ID3Info GetID3Info()
        {
            using (TagLib.File AudioFile = TagLib.File.Create(PlayingFile))
            {
                TagLib.Picture Picture = GetCoverPicture(AudioFile);
                ID3Info info = new ID3Info
                {
                    TrackName = AudioFile.Tag.Title,
                    Album = AudioFile.Tag.Album,
                    Artists = AudioFile.Tag.JoinedPerformers,
                    AlbumArtist = AudioFile.Tag.JoinedAlbumArtists,
                    Bitrate = AudioFile.Properties.AudioBitrate,
                    Cover = GetCoverImage(Picture),
                    CoverMIME = Picture.MimeType,
                    CoverType = Picture.Type.ToString(),
                    Duration = AudioFile.Properties.Duration.ToString("mm\\:ss"),
                    FileSize = AudioFile.FileAbstraction.ReadStream.Length.ToString(),
                    Genre = AudioFile.Tag.JoinedGenres,
                    Date = AudioFile.Tag.DateTagged,
                    Format = AudioFile.Properties.Description,
                    CompleteName = AudioFile.Name,
                };
                return info;
            }
        }

        public void InitializePlayer()
        {
            Task.Run(() =>
            {
                Player.Initialize(PlayerSource);
            }).Wait();
        }

        public void InitializePlayer(string filename)
        {
            // Task.Run(() =>
            // {
            LoadFile(filename);
            FFTP = new CSCore.DSP.FftProvider(PlayerSource.WaveFormat.Channels, CSCore.DSP.FftSize.Fft64);
            PlayerSource = PlayerSource
            .ToSampleSource()
            .AppendSource(x => new PitchShifter(x), out _PitchShift)
            .ToWaveSource()
            .AppendSource(x => new DmoEchoEffect(x) { WetDryMix = 0 }, out _DCE)
            .ToSampleSource()
            .AppendSource(x => new PeakMeter(x), out _AudioPeakMeter)
            .ToWaveSource()
            .AppendSource(x => new DmoGargleEffect(x), out _GargleEffect);
            var NotificationStream = new SingleBlockNotificationStream(PlayerSource.ToSampleSource());
            NotificationStream.SingleBlockRead += NotificationStream_SingleBlockRead;
            PlayerSource = NotificationStream.ToWaveSource();
            //
            Player.Initialize(PlayerSource);

            // }).Wait();
            //_GargleEffect.WaveShape = GargleWaveShape.
            _AudioPeakMeter.Interval = 25;

            //TagInfo = GetID3Info();
        }

        private void NotificationStream_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {
            FFTP.Add(e.Left, e.Right);
            //FFTCalc();
        }

        public void ReleasePlayer()
        {
            if (PlayerInitialized)
            {
                Player.Stop();
                Player.Dispose();
                PlayerSource.Dispose();
            }
        }

        public ISampleSource GetSampleSource()
        {
            return PlayerSource.ToSampleSource();
        }

        public void InitializePlayer(IWaveSource WaveSource)
        {
            Task.Run(() => { Player.Initialize(WaveSource); });
        }

        public void FFTCalc()
        {
            var samplebuffer = new float[(int)FFTP.FftSize/2];
            bool calculated = FFTP.GetFftData(samplebuffer);
            if (calculated)
            {
                foreach (var samp in samplebuffer)
                {
                    var amp = samp;
                    var y = Remap(amp, 0, 256, 0, 150);
                }
            }
        }

        public float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
        {
            var fromAbs = from - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var to = toAbs + toMin;

            return to;
        }

        public void ReinitializePlayer()
        {
            ReleasePlayer();
            Player = new CSCore.SoundOut.DirectSoundOut();
            LoadFile(PlayingFile);
            InitializePlayer();
        }

        public bool PlayerInitialized { get { return Player.WaveSource != null; } }

        public void Play()
        {
            Player.Play();
        }

        public void Pause()
        {
            Player.Pause();
        }

        public void Dispose()
        {
            PlayingFile = string.Empty;
            if (PlayerSource != null)
            {
                PlayerSource.Dispose();
            }
            Player.Dispose();
        }
    }

    public struct ID3Info
    {
        public string CompleteName;
        public string Format;
        public string FileSize;
        public string Duration;
        public int Bitrate;
        public string TrackName;
        public string Album;
        public string AlbumArtist;
        public int Position;
        public string Artists;
        public string Genre;
        public DateTime? Date;
        public Image Cover;
        public string CoverType;
        public string CoverMIME;
    }
}