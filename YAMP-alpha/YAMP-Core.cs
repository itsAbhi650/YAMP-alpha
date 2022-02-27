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
        internal NewMain UIRef;
        private TrackInfo _curtrack;
        public ID3Info TagInfo { get; set; }
        public CSCore.SoundOut.ISoundOut Player { get; private set; }
        public string PlayingFile { get; private set; }
        public bool StopRequested { get; set; } = false;
        public bool PlayerStopped { get; set; } = false;
        public int NextTrackDirection { get; set; } = 1;
        public IWaveSource PlayerSource { get; private set; }
        public TrackInfo CurrentTrack
        {
            get { return _curtrack; }
            set { _curtrack = value; OnTrackChanged(); }
        }
        public TrackInfo NextTrack = null;
        private bool MoveValidated;

        public int SoundOutVolume
        {
            get { return (int)(Player.Volume * 100F); }
            set { Player.Volume = value / 100F; }
        }

        public event EventHandler TrackChanged;

        private void OnTrackChanged()
        {
            TrackChanged?.Invoke(this, EventArgs.Empty);
        }

        public CSCore.SoundOut.PlaybackState PlayerPlaybackState { get { return Player.PlaybackState; } }

        public int PlayerLength { get { return (int)PlayerSource.Length; } }

        public int CurrentPosition { get { return (int)PlayerSource.Position; } }

        public YAMP_Core()
        {

            //ChorusParameters chor = new ChorusParameters();
            //TrackChanged += YAMP_Core_TrackChanged;
            Player = new CSCore.SoundOut.DirectSoundOut();
            //Player.Stopped += Player_Stopped;
            YAMPVars.MediaDevice = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            Task.Run(() =>
            {
                YAMPVars.AudioSessionManager = AudioSessionManager2.FromMMDevice(YAMPVars.MediaDevice);
            }).ContinueWith((t) => YAMPVars.SessionEnumerator = YAMPVars.AudioSessionManager.GetSessionEnumerator());

        }

        private void Player_Stopped(object sender, CSCore.SoundOut.PlaybackStoppedEventArgs e)
        {
            PlayerStopped = true;
        }

        public bool isValidMove(int direction, out int DestIndex)
        {
            int CurrTrackIndex = GetCurrentTrackIndex();
            DestIndex = CurrTrackIndex + direction;
            if (DestIndex >= 0 && DestIndex < YAMPVars.TrackList.Count)
            {
                MoveValidated = true;

            }
            else
            {
                MoveValidated = false;
            }
            return MoveValidated;
        }

        public Image GetTrackCover(int index = 0)
        {
            if (CurrentTrack != null)
            {
                return CurrentTrack.Covers[index];
            }
            else
            {
                return null;
            }
        }

        public void AdjustPlayerPosition(int Value)
        {
            if (YAMPVars.CORE.PlayerInitialized)
            {
                TimeSpan ts = TimeSpan.FromSeconds(Value);
                Extensions.SetPosition(YAMPVars.CORE.PlayerSource, ts);
            }
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
                    Duration = AudioFile.Properties.Duration.ToString("mm\\:ss"),
                    FileSize = AudioFile.FileAbstraction.ReadStream.Length.ToString(),
                    Genre = AudioFile.Tag.JoinedGenres,
                    Date = AudioFile.Tag.DateTagged,
                    Format = AudioFile.Properties.Description,
                    CompleteName = AudioFile.Name,
                };
                if (Picture != null)
                {
                    info.Cover = GetCoverImage(Picture);
                    info.CoverMIME = Picture.MimeType;
                    info.CoverType = Picture.Type.ToString();
                }
                return info;
            }
        }

        public bool GetFirstTrack()
        {
            if (YAMPVars.TrackList != null && YAMPVars.TrackList.Count > 0)
            {
                TrackInfo FirstTrack = YAMPVars.TrackList[0];
                YAMPVars.CORE.InitializePlayer(FirstTrack.Path);
                CurrentTrack = FirstTrack;
                return true;
            }
            else { return false; }
        }

        public bool PlayNextTrackDirected(int direction)
        {
            if (FetchTrackDirected(direction))
            {
                Player.Stop();
                InitializePlayer(NextTrack.Path);
                CurrentTrack = NextTrack;
                PlayerStopped = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool LoadTrackInfo(TrackInfo trackInfo)
        {
            if (trackInfo != null)
            {
                Stop();
                InitializePlayer(trackInfo.Path);
                CurrentTrack = trackInfo;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool FetchTrackDirected(int direction)
        {
            if (isValidMove(direction, out int DestIndex))
            {
                YAMPVars.CORE.NextTrack = YAMPVars.TrackList.ElementAtOrDefault(DestIndex);
            }
            else
            {
                YAMPVars.CORE.NextTrack = null;
            }
            return YAMPVars.CORE.NextTrack != null;
        }

        public int GetCurrentTrackIndex() => YAMPVars.TrackList.Select((t, i) => new { track = t, index = i }).First(x => x.track.Title == YAMPVars.CORE.CurrentTrack.Title).index;

        public void InitializePlayer()
        {
            Task.Run(() =>
            {
                Player.Initialize(PlayerSource);
            }).Wait();
        }

        public void InitializePlayer(string filename)
        {
            LoadFile(filename);
            YAMPVars.ResetEffectVars();
            YAMPVars.ResetStreamNotifications();
            PlayerSource = PlayerSource
            .AppendSource(x => new DmoDistortionEffect(x) { IsEnabled = false }, out YAMPVars.DistortionEffect)
            .AppendSource(x => new DmoFlangerEffect(x) { IsEnabled = false }, out YAMPVars.FlangerEffect)
            .AppendSource(x => new DmoWavesReverbEffect(x) { IsEnabled = false }, out YAMPVars.WavesReverbEffect)
            .AppendSource(x => new DmoEchoEffect(x) { IsEnabled = false }, out YAMPVars.EchoEffect)
            .AppendSource(x => new DmoCompressorEffect(x) { IsEnabled = false }, out YAMPVars.CompressorEffect)
            .AppendSource(x => new DmoGargleEffect(x) { IsEnabled = false }, out YAMPVars.GargleEffect)
            .AppendSource(x => new DmoChorusEffect(x) { IsEnabled = false }, out YAMPVars.ChorusEffect)
            .ToSampleSource()
            .AppendSource(x => new PeakMeter(x), out YAMPVars.AudioPeakMeter)
            .AppendSource(x => new PitchShifter(x), out YAMPVars.PitchShiftEffect)
            .AppendSource(x => Equalizer.Create10BandEqualizer(x), out YAMPVars.EqualizerEffect)
            .AppendSource(x => new SingleBlockNotificationStream(x), out YAMPVars.SingleBlockNotificationStream)
            .ToWaveSource();
            //

            CreateNotificationEvents();

            Player.Initialize(PlayerSource);
            YAMPVars.AudioPeakMeter.Interval = 25;
            TagInfo = GetID3Info();
        }

        public void CreateNotificationEvents()
        {
            YAMPVars.SingleBlockNotificationStream.SingleBlockRead += NotificationStream_SingleBlockRead;
            YAMPVars.SingleBlockNotificationStream.SingleBlockStreamAlmostFinished += NotificationStream_SingleBlockStreamAlmostFinished;
            YAMPVars.SingleBlockNotificationStream.SingleBlockStreamFinished += NotificationStream_SingleBlockStreamFinished;
        }

        private void NotificationStream_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {
            WaveFormLEFT = e.Left;
            WaveFormRIGHT = e.Right;
        }

        private void NotificationStream_SingleBlockStreamAlmostFinished(object sender, SingleBlockStreamAlmostFinishedEventArgs e)
        {
            FetchTrackDirected(1);
        }

        private void NotificationStream_SingleBlockStreamFinished(object sender, SingleBlockStreamFinishedEventArgs e)
        {
            PlayerStopped = true;
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

        //public float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
        //{
        //    var fromAbs = from - fromMin;
        //    var fromMaxAbs = fromMax - fromMin;

        //    var normal = fromAbs / fromMaxAbs;

        //    var toMaxAbs = toMax - toMin;
        //    var toAbs = toMaxAbs * normal;

        //    var to = toAbs + toMin;

        //    return to;
        //}

        public void ReinitializePlayer()
        {
            ReleasePlayer();
            Player = new CSCore.SoundOut.DirectSoundOut();
            LoadFile(PlayingFile);
            InitializePlayer();
        }

        public bool PlayerInitialized { get { return Player.WaveSource != null; } }

        public float WaveFormLEFT { get; private set; }
        public float WaveFormRIGHT { get; private set; }
        public bool PlayerPaused { get; internal set; }

        public void Play()
        {
            Player.Play();
        }

        public void Stop()
        {
            Player.Stop();
            AdjustPlayerPosition(0);
            //PlayerStopped = true;
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