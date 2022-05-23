using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.Ffmpeg;
using CSCore.Streams;
using CSCore.Streams.Effects;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
namespace YAMP_alpha
{
    public class YAMP_Core : IDisposable
    {
        internal NewMain UIRef;
        private TrackInfo _curtrack;
        public bool EnableFade;
        public ID3Info TagInfo { get; set; }
        public CSCore.SoundOut.ISoundOut Player { get; private set; }
        public string PlayingFile { get; private set; }
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
            Player = new CSCore.SoundOut.WasapiOut();
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
            if (CurrentTrack != null && CurrentTrack.Covers.Count > 0)
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
            if (PlayerInitialized)
            {
                if (!NetPlay || PlayerSource.CanSeek)
                {
                    TimeSpan ts = TimeSpan.FromSeconds(Value);
                    Extensions.SetPosition(YAMPVars.CORE.PlayerSource, ts);
                }
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

        public int GetCurrentTrackIndex()
        {
            if (!NetPlay)
            {
                return YAMPVars.TrackList.Select((t, i) => new { track = t, index = i }).First(x => x.track.Title == YAMPVars.CORE.CurrentTrack.Title).index;
            }
            else
            {
                return -1;
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
            NetPlay = false;
            LoadFile(filename);
            PlayerSource = AppendEffectSources(PlayerSource);
            CreateNotificationEvents();
            Player.Initialize(PlayerSource);
            TagInfo = GetID3Info();
        }

        public bool InitializePlayerNet(string StreamURL)
        {
            NetPlay = true;
            PlayingFile = StreamURL;
            PlayerSource = CheckStreamSource(StreamURL, out string LocalPath);
            if (PlayerSource != null)
            {
                CurrentTrack = new TrackInfo(LocalPath);
                Player.Initialize(PlayerSource);
                return true;
            }
            else
            {
                return false;
            }
        }

        private IWaveSource CheckStreamSource(string StreamUrl, out string LocalURI)
        {
            LocalURI = string.Empty;
            string knownType = "";
            string DLFilePath = "";
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            bool AllowDownload = false;
            var ffSource = new FfmpegDecoder(StreamUrl);
            if (ffSource.WaveFormat != null && ffSource.CanSeek)
            {
                ffSource = null;
                if (StreamUrl.Contains("mp3"))
                {
                    knownType = "mp3";
                }

                using (WebClient WebC = new WebClient())
                {
                    WebC.OpenRead(StreamUrl);
                    long TotalBytes = Convert.ToInt64(WebC.ResponseHeaders["Content-Length"]);
                    var TotalMegaBytes = TotalBytes / 1024 / 1024F;
                    using (StreamDialog Sdiag = new StreamDialog(TotalMegaBytes))
                    {
                        if (Sdiag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            WebC.DownloadFileCompleted += Wc_DownloadFileCompleted;
                            WebC.DownloadProgressChanged += Wc_DownloadProgressChanged;
                            AllowDownload = true;
                        }
                    }
                    if (AllowDownload)
                    {
                        DLFilePath = AppContext.BaseDirectory + "temp." + knownType;
                        LocalURI = DLFilePath;
                        WebC.DownloadFileAsync(new Uri(StreamUrl), DLFilePath);
                        new FileInfo(DLFilePath).Attributes = FileAttributes.Hidden;
                        YAMPVars.DownloadProgress.ShowDialog();
                        return CSCore.Codecs.CodecFactory.Instance.GetCodec(DLFilePath);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return ffSource;
            }
        }

        private void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            YAMPVars.DownloadProgress.Close();
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            YAMPVars.DownloadProgress.Percent = e.ProgressPercentage;
        }

        private IWaveSource AppendEffectSources(IWaveSource Source)
        {
            YAMPVars.ResetEffectVars();
            Source = Source
            .AppendSource(x => new DmoDistortionEffect(x) { IsEnabled = false }, out YAMPVars.DistortionEffect)
            .AppendSource(x => new DmoFlangerEffect(x) { IsEnabled = false }, out YAMPVars.FlangerEffect)
            .AppendSource(x => new DmoWavesReverbEffect(x) { IsEnabled = false }, out YAMPVars.WavesReverbEffect)
            .AppendSource(x => new DmoEchoEffect(x) { IsEnabled = false }, out YAMPVars.EchoEffect)
            .AppendSource(x => new DmoCompressorEffect(x) { IsEnabled = false }, out YAMPVars.CompressorEffect)
            .AppendSource(x => new DmoGargleEffect(x) { IsEnabled = false }, out YAMPVars.GargleEffect)
            .AppendSource(x => new DmoChorusEffect(x) { IsEnabled = false }, out YAMPVars.ChorusEffect)
            .AppendSource(x => new LoopStream(x) { EnableLoop = false }, out YAMPVars.TrackLoop)
            .ToSampleSource()
            .AppendSource(x => new GainSource(x) { Volume = 1.0f }, out YAMPVars.GainSource)
            .AppendSource(x => new VolumeSource(x) { Volume = 1.0f }, out YAMPVars.VolumeSource)
            .AppendSource(x => new PeakMeter(x) { Interval = 25 }, out YAMPVars.AudioPeakMeter)
            .AppendSource(x => new PitchShifter(x), out YAMPVars.PitchShiftEffect)
            .AppendSource(x => new FadeInOut(x) { FadeStrategy = new LinearFadeStrategy() }, out YAMPVars.FadeEffect)
            .AppendSource(x => new NotificationSource(x), out YAMPVars.NotificationSource)
            .ToWaveSource();

            if (Source.WaveFormat.Channels > 1)
            {
                Source = Source.ToSampleSource().AppendSource(x => new PanSource(x) { Pan = 0.0F }, out YAMPVars.ChannelPan).ToWaveSource();
            }

            if (Source.WaveFormat.SampleRate >= 32000)
            {
                Source = Source.ToSampleSource().AppendSource(x => Equalizer.Create10BandEqualizer(x), out YAMPVars.EqualizerEffect).ToWaveSource();
            }
            return Source;
        }

        public void CreateNotificationEvents()
        {
            YAMPVars.ResetStreamNotifications();
            PlayerSource = PlayerSource.AppendSource(x => new SingleBlockNotificationStream(x.ToSampleSource(), 200000), out YAMPVars.SingleBlockNotificationStream).ToWaveSource();
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
            if (!NetPlay || PlayerSource.CanSeek)
            {
                FetchTrackDirected(1);
            }

            if (EnableFade)
            {
                if (YAMPVars.FadeEffect != null)
                {
                    //This will find the remaining seconds after SingleBlockStreamAlmostFinished event triggers on which the fade out will be applied.
                    TimeSpan REMSEC = PlayerSource.GetLength() - PlayerSource.GetPosition();

                    //Starting volume set to null to use default/current volume.
                    YAMPVars.FadeEffect.FadeStrategy.StartFading(null, 0, REMSEC);
                }
            }
        }

        private void NotificationStream_SingleBlockStreamFinished(object sender, SingleBlockStreamFinishedEventArgs e)
        {
            if (!NetPlay || PlayerSource.CanSeek)
            {
                PlayerStopped = true;
            }
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
            Player = new CSCore.SoundOut.WasapiOut();
            LoadFile(PlayingFile);
            InitializePlayer();
        }

        public bool PlayerInitialized { get { return Player.WaveSource != null; } }

        public float WaveFormLEFT { get; private set; }
        public float WaveFormRIGHT { get; private set; }
        public bool PlayerPaused { get; internal set; }
        public bool NetPlay { get; internal set; }

        public void Play()
        {
            Player.Play();
        }

        public void Stop()
        {
            Player.Stop();
            AdjustPlayerPosition(0);
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