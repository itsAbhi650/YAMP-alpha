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
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

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

    public enum CorePlaybackState
    {
        Idle,
        Loading,
        Ready,
        Playing,
        Paused,
        Stopping,
        Ended,
        Error,
        Disposed
    }

    public class YAMP_Core : IDisposable
    {
        internal NewMain UIRef;
        private TrackInfo _curtrack;
        private readonly object _playerLock = new object();
        private CorePlaybackState _state = CorePlaybackState.Idle;
        private float _volume = 1.0f;

        // Restored: global certificate validation bypass (used for some HTTPS streams).
        // This is intentionally global and should be treated as a last resort.
        private static bool _globalCertBypassEnabled;
        private static readonly object _globalCertBypassLock = new object();
        public bool EnableFade;
        public ID3Info TagInfo { get; set; }
        public CSCore.SoundOut.ISoundOut Player { get; private set; }
        public string PlayingFile { get; private set; }
        public bool PlayerStopped { get; set; } = false;
        public StopReason LastStopReason { get; private set; } = StopReason.UserStopped;
        public int NextTrackDirection { get; set; } = 1;
        public IWaveSource PlayerSource { get; private set; }
        public CorePlaybackState State
        {
            get { return _state; }
            private set
            {
                if (_state != value)
                {
                    _state = value;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
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
            get { return (int)(_volume * 100F); }
            set
            {
                _volume = Math.Max(0, Math.Min(100, value)) / 100F;
                if (Player != null)
                {
                    Player.Volume = _volume;
                }
            }
        }

        public event EventHandler TrackChanged;
        /// <summary>
        /// Fired once when a track ends naturally (not on manual stop or track change)
        /// </summary>
        public event EventHandler TrackEnded;

        // Internal flag to ensure TrackEnded is raised only once per track
        private bool _trackEndedRaised = false;
        public event EventHandler StateChanged;

        private void OnTrackChanged()
        {
            TrackChanged?.Invoke(this, EventArgs.Empty);
        }

        public CSCore.SoundOut.PlaybackState PlayerPlaybackState
        {
            get { return Player != null ? Player.PlaybackState : CSCore.SoundOut.PlaybackState.Stopped; }
        }

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
            EnsureGlobalCertificateBypass();
            Player = CreatePlayer();
            YAMPVars.MediaDevice = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            Task.Run(() =>
            {
                YAMPVars.AudioSessionManager = AudioSessionManager2.FromMMDevice(YAMPVars.MediaDevice);
            }).ContinueWith((t) => YAMPVars.SessionEnumerator = YAMPVars.AudioSessionManager.GetSessionEnumerator());
        }

        private static void EnsureGlobalCertificateBypass()
        {
            if (_globalCertBypassEnabled)
                return;

            lock (_globalCertBypassLock)
            {
                if (_globalCertBypassEnabled)
                    return;

                ServicePointManager.ServerCertificateValidationCallback += AlwaysAcceptCertificate;
                _globalCertBypassEnabled = true;
            }
        }

        private static bool AlwaysAcceptCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private CSCore.SoundOut.ISoundOut CreatePlayer()
        {
            var player = new CSCore.SoundOut.WasapiOut();
            //player.Volume = _volume; // Removed as not initialized yet, will set when source is ready
            player.Stopped += Player_Stopped;
            return player;
        }

        private void Player_Stopped(object sender, CSCore.SoundOut.PlaybackStoppedEventArgs e)
        {
            if (LastStopReason == StopReason.TrackFinished)
            {
                State = CorePlaybackState.Ended;
            }
            else if (LastStopReason == StopReason.TrackChanging || LastStopReason == StopReason.PlayerReset)
            {
                State = CorePlaybackState.Ready;
            }
            else
            {
                State = CorePlaybackState.Idle;
            }
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
                    Extensions.SetPosition(PlayerSource, ts);
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
                return LoadTrackInfo(FirstTrack);
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
                try
                {
                    InitializePlayer(nextTrack.Path);
                    CurrentTrack = nextTrack;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public bool LoadTrackInfo(TrackInfo trackInfo)
        {
            if (trackInfo != null)
            {
                try
                {
                    InitializePlayer(trackInfo.Path);
                    CurrentTrack = trackInfo;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void InitializePlayer()
        {
            InitializeCurrentSource();
        }

        public void InitializePlayer(string filename)
        {
            lock (_playerLock)
            {
                State = CorePlaybackState.Loading;
                NetPlay = false;
                try
                {
                    PrepareForNewSource(StopReason.TrackChanging);
                    LoadFile(filename);
                    PlayerSource = AppendEffectSources(PlayerSource);
                    CreateNotificationEvents();
                    InitializeCurrentSource();
                    TagInfo = GetID3Info();
                    PlayerStopped = false;
                    PlayerPaused = false;
                    _trackEndedRaised = false;
                    State = CorePlaybackState.Ready;
                }
                catch
                {
                    CleanupAfterLoadFailure();
                    throw;
                }
            }
        }

        public bool InitializePlayerNet(string StreamURL)
        {
            lock (_playerLock)
            {
                State = CorePlaybackState.Loading;
                NetPlay = true;
                PrepareForNewSource(StopReason.TrackChanging);
                PlayingFile = StreamURL;
                try
                {
                    PlayerSource = CheckStreamSource(StreamURL, out string LocalPath);
                    if (PlayerSource != null)
                    {
                        PlayerSource = AppendEffectSources(PlayerSource);
                        CreateNotificationEvents();
                        InitializeCurrentSource();
                        CurrentTrack = !string.IsNullOrEmpty(LocalPath) && File.Exists(LocalPath)
                            ? new TrackInfo(LocalPath)
                            : new TrackInfo();
                        PlayerStopped = false;
                        PlayerPaused = false;
                        _trackEndedRaised = false;
                        State = CorePlaybackState.Ready;
                        return true;
                    }
                }
                catch
                {
                    CleanupAfterLoadFailure();
                    return false;
                }

                CleanupAfterLoadFailure();
                return false;
            }
        }

        private IWaveSource CheckStreamSource(string StreamUrl, out string LocalURI)
        {
            LocalURI = string.Empty;
            if (string.IsNullOrWhiteSpace(StreamUrl))
                return null;

            try
            {
                return new FfmpegDecoder(StreamUrl);
            }
            catch
            {
                try
                {
                    return CSCore.Codecs.CodecFactory.Instance.GetCodec(new Uri(StreamUrl));
                }
                catch
                {
                    return null;
                }
            }
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
                var priorReason = LastStopReason;
                LastStopReason = StopReason.TrackFinished;
                PlayerPaused = false;

                // Only treat this as a finished track when the stop was not initiated by the user or a track change
                if (priorReason != StopReason.UserStopped && priorReason != StopReason.TrackChanging)
                {
                    // mark stopped for UI/logic that expects natural end and raise the event
                    PlayerStopped = true;
                    RaiseTrackEnded();
                }
            }
        }

        private void RaiseTrackEnded()
        {
            if (_trackEndedRaised)
                return;

            _trackEndedRaised = true;
            State = CorePlaybackState.Ended;
            TrackEnded?.Invoke(this, EventArgs.Empty);
        }

        public void ReleasePlayer()
        {
            lock (_playerLock)
            {
                PrepareForNewSource(StopReason.PlayerReset);
                State = CorePlaybackState.Idle;
            }
        }

        public ISampleSource GetSampleSource()
        {
            return PlayerSource.ToSampleSource();
        }

        public void InitializePlayer(IWaveSource WaveSource)
        {
            lock (_playerLock)
            {
                PrepareForNewSource(StopReason.PlayerReset);
                PlayerSource = WaveSource;
                InitializeCurrentSource();
                State = CorePlaybackState.Ready;
            }
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
            ReleasePlayer();
        }

        public bool PlayerInitialized { get { return Player != null && Player.WaveSource != null; } }

        public float WaveFormLEFT { get; private set; }
        public float WaveFormRIGHT { get; private set; }
        public bool PlayerPaused { get; internal set; }
        public bool NetPlay { get; internal set; }

        public void Play()
        {
            if (!PlayerInitialized)
                return;

            Player.Play();
            PlayerStopped = false;
            PlayerPaused = false;
            State = CorePlaybackState.Playing;
        }

        public void Stop()
        {
            if (!PlayerInitialized)
                return;

            LastStopReason = StopReason.UserStopped;
            State = CorePlaybackState.Stopping;
            if (!NetPlay || PlayerSource.CanSeek)
            {
                AdjustPlayerPosition(0);
            }
            Player.Stop();
            PlayerStopped = false;
            PlayerPaused = false;
        }

        public void Pause()
        {
            if (!PlayerInitialized)
                return;

            Player.Pause();
            PlayerPaused = true;
            State = CorePlaybackState.Paused;
        }

        private void InitializeCurrentSource()
        {
            if (PlayerSource == null)
                return;

            Player.Initialize(PlayerSource);
        }

        private void PrepareForNewSource(StopReason reason)
        {
            LastStopReason = reason;
            UnsubscribeNotificationEvents();
            DisposePlayer();
            DisposePlayerSource();
            Player = CreatePlayer();
            PlayerStopped = false;
            PlayerPaused = false;
            _trackEndedRaised = false;
        }

        private void UnsubscribeNotificationEvents()
        {
            if (YAMPVars.SingleBlockNotificationStream != null)
            {
                YAMPVars.SingleBlockNotificationStream.SingleBlockRead -= NotificationStream_SingleBlockRead;
                YAMPVars.SingleBlockNotificationStream.SingleBlockStreamAlmostFinished -= NotificationStream_SingleBlockStreamAlmostFinished;
                YAMPVars.SingleBlockNotificationStream.SingleBlockStreamFinished -= NotificationStream_SingleBlockStreamFinished;
            }
        }

        private void DisposePlayer()
        {
            if (Player == null)
                return;

            try { Player.Stopped -= Player_Stopped; } catch { }
            try { Player.Stop(); } catch { }
            try { Player.Dispose(); } catch { }
            Player = null;
        }

        private void DisposePlayerSource()
        {
            try { PlayerSource?.Dispose(); } catch { }
            PlayerSource = null;
        }

        private void CleanupAfterLoadFailure()
        {
            UnsubscribeNotificationEvents();
            DisposePlayer();
            DisposePlayerSource();
            Player = CreatePlayer();
            PlayerStopped = false;
            PlayerPaused = false;
            State = CorePlaybackState.Error;
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
                        LastStopReason = StopReason.PlayerReset;
                        UnsubscribeNotificationEvents();
                        DisposePlayer();
                        DisposePlayerSource();
                        State = CorePlaybackState.Disposed;
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
