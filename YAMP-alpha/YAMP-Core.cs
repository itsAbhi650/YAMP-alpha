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
    /// <summary>
    /// Indicates why the player stopped
    /// </summary>
    public enum StopReason
    {
        /// <summary>Track finished naturally (reached end)</summary>
        TrackFinished,
        /// <summary>User manually stopped playback</summary>
        UserStopped,
        /// <summary>Switching to next/previous track</summary>
        TrackChanging,
        /// <summary>Player being reset/reinitialized</summary>
        PlayerReset
    }

    public class YAMP_Core : IDisposable
    {
        internal NewMain UIRef;
        private TrackInfo _curtrack;
        public bool EnableFade;
        public ID3Info TagInfo { get; set; }
        public CSCore.SoundOut.ISoundOut Player { get; private set; }
        public string PlayingFile { get; private set; }
        public bool PlayerStopped { get; set; } = false;
        public StopReason LastStopReason { get; private set; } = StopReason.UserStopped;
        public int NextTrackDirection { get; set; } = 1;
        public IWaveSource PlayerSource { get; private set; }
        public TrackInfo CurrentTrack
        {
            get { return _curtrack; }
            set { _curtrack = value; OnTrackChanged(); }
        }

        // Magic number documentation: 200000 samples represents the block size for audio notification events.
        // This size determines how frequently the SingleBlockRead event fires during playback.
        private const int NOTIFICATION_BLOCK_SIZE = 200000;

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

        public int PlayerLength 
        { 
            get { return PlayerSource?.Length > 0 ? (int)PlayerSource.Length : 0; } 
        }

        public int CurrentPosition 
        { 
            get { return PlayerSource != null ? (int)PlayerSource.Position : 0; } 
        }

        public YAMP_Core()
        {
            Player = new CSCore.SoundOut.WasapiOut();
            YAMPVars.MediaDevice = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            Task.Run(() =>
            {
                YAMPVars.AudioSessionManager = AudioSessionManager2.FromMMDevice(YAMPVars.MediaDevice);
            }).ContinueWith((t) => YAMPVars.SessionEnumerator = YAMPVars.AudioSessionManager.GetSessionEnumerator());
        }

        // Safer GetTrackCover with bounds check
        public Image GetTrackCover(int index = 0)
        {
            if (CurrentTrack?.Covers != null && CurrentTrack.Covers.Count > index && index >= 0)
            {
                return CurrentTrack.Covers[index];
            }
            return null;
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
                CoverImage = CreateImageFromBytes(CoverPicture.Data.Data);
            }
            return CoverImage;
        }

        private Image GetCoverImage(TagLib.Picture Picture)
        {
            Image CoverImage = null;
            if (Picture != null)
            {
                CoverImage = CreateImageFromBytes(Picture.Data.Data);
            }
            return CoverImage;
        }

        /// <summary>
        /// Creates an Image from byte data, ensuring the MemoryStream lifetime is managed safely.
        /// The MemoryStream is not kept alive after this method returns; instead, a Bitmap copy is created.
        /// </summary>
        private Image CreateImageFromBytes(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            try
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    using (Image tempImage = Image.FromStream(ms))
                    {
                        // Create a permanent Bitmap copy so the Image doesn't depend on the disposed MemoryStream
                        return new Bitmap(tempImage);
                    }
                }
            }
            catch
            {
                // If image creation fails, return null rather than throwing
                return null;
            }
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
            var file = new FileInfo(PlayingFile);
            ID3Info info = new ID3Info
            {
                CompleteName = PlayingFile,
                TrackName = file.Exists ? file.Name : PlayingFile,
                FileSize = file.Exists ? file.Length.ToString() : string.Empty,
                Format = file.Extension
            };

            try
            {
                using (TagLib.File AudioFile = TagLib.File.Create(PlayingFile))
                {
                    TagLib.Picture Picture = GetCoverPicture(AudioFile);
                    info.TrackName = string.IsNullOrEmpty(AudioFile.Tag.Title) ? info.TrackName : AudioFile.Tag.Title;
                    info.Album = AudioFile.Tag.Album;
                    info.Artists = AudioFile.Tag.JoinedPerformers;
                    info.AlbumArtist = AudioFile.Tag.JoinedAlbumArtists;
                    info.Bitrate = AudioFile.Properties.AudioBitrate;
                    info.Duration = AudioFile.Properties.Duration.ToString("mm\\:ss");
                    info.FileSize = AudioFile.FileAbstraction.ReadStream.Length.ToString();
                    info.Genre = AudioFile.Tag.JoinedGenres;
                    info.Date = AudioFile.Tag.DateTagged;
                    info.Format = AudioFile.Properties.Description;
                    info.CompleteName = AudioFile.Name;

                    if (Picture != null)
                    {
                        info.Cover = GetCoverImage(Picture);
                        info.CoverMIME = Picture.MimeType;
                        info.CoverType = Picture.Type.ToString();
                    }
                }
            }
            catch
            {
                if (PlayerSource != null)
                {
                    try
                    {
                        info.Duration = PlayerSource.GetLength().ToString("mm\\:ss");
                    }
                    catch
                    {
                    }
                }
            }

            return info;
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

        /// <summary>
        /// Gets the index of the current track in the playlist.
        /// Returns -1 if current track is not in the list or list is empty.
        /// </summary>
        public int GetCurrentTrackIndex()
        {
            if (CurrentTrack == null || YAMPVars.TrackList == null || YAMPVars.TrackList.Count == 0)
                return -1;

            // Use built-in IndexOf for reference equality check
            return YAMPVars.TrackList.IndexOf(CurrentTrack);
        }

        /// <summary>
        /// Checks if a track index is valid within the current playlist bounds.
        /// </summary>
        private bool IsValidTrackIndex(int index)
        {
            return YAMPVars.TrackList != null && 
                   index >= 0 && 
                   index < YAMPVars.TrackList.Count;
        }

        /// <summary>
        /// Gets the track at the specified direction from the current track.
        /// </summary>
        /// <param name="direction">Direction to move (1 for next, -1 for previous)</param>
        /// <returns>TrackInfo if found, null otherwise</returns>
        private TrackInfo GetTrackAt(int direction)
        {
            int currentIndex = GetCurrentTrackIndex();
            if (currentIndex < 0)
                return null;

            int targetIndex = currentIndex + direction;
            
            return IsValidTrackIndex(targetIndex) 
                ? YAMPVars.TrackList[targetIndex] 
                : null;
        }

        /// <summary>
        /// Plays the next track in the specified direction.
        /// </summary>
        /// <param name="direction">Direction to move (1 for next, -1 for previous)</param>
        /// <returns>True if next track was loaded and started, false if no track available</returns>
        public bool PlayNextTrackDirected(int direction)
        {
            TrackInfo nextTrack = GetTrackAt(direction);
            
            if (nextTrack != null)
            {
                // Mark this as a track change to prevent Player_Stopped event from interfering
                LastStopReason = StopReason.TrackChanging;
                Player.Stop();

                // Wait for stop to complete to ensure all event handlers finish
                // This prevents race condition with Player_Stopped event
                //int timeout = 100; // 100ms max wait
                //while (Player.PlaybackState != CSCore.SoundOut.PlaybackState.Stopped && timeout > 0)
                //{
                //    System.Threading.Thread.Sleep(5);
                //    timeout -= 5;
                //}

                // Wait for stop to complete before initializing new track
                // This ensures all event handlers complete
                while (Player.PlaybackState != CSCore.SoundOut.PlaybackState.Stopped)
                {
                    System.Threading.Thread.Sleep(1);  // Small delay to let stop complete
                }

                InitializePlayer(nextTrack.Path);
                CurrentTrack = nextTrack;
                PlayerStopped = false;
                return true;
            }
            return false;
        }

        public bool LoadTrackInfo(TrackInfo trackInfo)
        {
            if (trackInfo != null)
            {
                LastStopReason = StopReason.TrackChanging;
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
                    var TotalMegaBytes = TotalBytes / 1024F / 1024F;
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
            PlayerSource = PlayerSource.AppendSource(x => new SingleBlockNotificationStream(x.ToSampleSource(), NOTIFICATION_BLOCK_SIZE), out YAMPVars.SingleBlockNotificationStream).ToWaveSource();
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
            // Apply fade-out effect near the end of the track if enabled
            if (EnableFade && YAMPVars.FadeEffect != null)
            {
                // Find the remaining seconds after SingleBlockStreamAlmostFinished event triggers
                // This is the duration over which the fade out will be applied
                TimeSpan REMSEC = PlayerSource.GetLength() - PlayerSource.GetPosition();

                // Starting volume set to null to use default/current volume
                YAMPVars.FadeEffect.FadeStrategy.StartFading(null, 0, REMSEC);
            }
        }

        private void NotificationStream_SingleBlockStreamFinished(object sender, SingleBlockStreamFinishedEventArgs e)
        {
            if (!NetPlay || PlayerSource.CanSeek)
            {
                LastStopReason = StopReason.TrackFinished;
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

        public void ResetPlayer()
        {
            LastStopReason = StopReason.PlayerReset;
            ReleasePlayer();
            Player = new CSCore.SoundOut.WasapiOut();
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
            LastStopReason = StopReason.UserStopped;
            Player.Stop();
            AdjustPlayerPosition(0);
        }

        public void Pause()
        {
            Player.Pause();
        }

        private bool _disposed = false;
        private readonly object _disposeLock = new object();

        // Replace Dispose() with full Dispose pattern and event cleanup
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            lock (_disposeLock)
            {
                if (_disposed) return;
                if (disposing)
                {
                    // managed cleanup
                    try
                    {
                        // Stop and unsubscribe notification stream events
                        if (YAMPVars.SingleBlockNotificationStream != null)
                        {
                            YAMPVars.SingleBlockNotificationStream.SingleBlockRead -= NotificationStream_SingleBlockRead;
                            YAMPVars.SingleBlockNotificationStream.SingleBlockStreamAlmostFinished -= NotificationStream_SingleBlockStreamAlmostFinished;
                            YAMPVars.SingleBlockNotificationStream.SingleBlockStreamFinished -= NotificationStream_SingleBlockStreamFinished;
                        }

                        // Stop player safely
                        try { Player?.Stop(); } catch { }
                        try { PlayerSource?.Dispose(); } catch { }
                        try { Player?.Dispose(); } catch { }
                    }
                    catch
                    {
                        // log if you have logging, otherwise swallow to avoid throwing from Dispose
                    }
                }

                // unmanaged cleanup (none here)
                _disposed = true;
            }
        }

        ~YAMP_Core()
        {
            Dispose(false);
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