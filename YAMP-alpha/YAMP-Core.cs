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
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace YAMP_alpha
{
    /// <summary>
    /// Indicates why the player stopped
    /// </summary>
    public enum StopReason
    {
        /// <summary>No stop has been requested for the current source</summary>
        None,
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
        private TrackInfo _curtrack;
        private readonly object _playerLock = new object();
        private readonly SynchronizationContext _eventContext;
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
        public DmoDistortionEffect DistortionEffect { get; private set; }
        public DmoFlangerEffect FlangerEffect { get; private set; }
        public DmoWavesReverbEffect WavesReverbEffect { get; private set; }
        public DmoEchoEffect EchoEffect { get; private set; }
        public DmoCompressorEffect CompressorEffect { get; private set; }
        public DmoGargleEffect GargleEffect { get; private set; }
        public DmoChorusEffect ChorusEffect { get; private set; }
        public LoopStream TrackLoop { get; private set; }
        public GainSource GainSource { get; private set; }
        public VolumeSource VolumeSource { get; private set; }
        public PeakMeter AudioPeakMeter { get; private set; }
        public PitchShifter PitchShiftEffect { get; private set; }
        public FadeInOut FadeEffect { get; private set; }
        public PanSource ChannelPan { get; private set; }
        public Equalizer EqualizerEffect { get; private set; }
        public NotificationSource NotificationSource { get; private set; }
        public SingleBlockNotificationStream SingleBlockNotificationStream { get; private set; }
        public CorePlaybackState State
        {
            get { return _state; }
            private set
            {
                if (_state != value)
                {
                    _state = value;
                    RaiseEvent(StateChanged, EventArgs.Empty);
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
        public event EventHandler<TrackLoadFailedEventArgs> TrackLoadFailed;

        // Internal flag to ensure TrackEnded is raised only once per track
        private bool _trackEndedRaised = false;
        public event EventHandler StateChanged;

        private void OnTrackChanged()
        {
            RaiseEvent(TrackChanged, EventArgs.Empty);
        }

        private void OnTrackLoadFailed(string path, string error)
        {
            RaiseEvent(TrackLoadFailed, new TrackLoadFailedEventArgs(path, error));
        }

        public CSCore.SoundOut.PlaybackState PlayerPlaybackState
        {
            get { return Player != null ? Player.PlaybackState : CSCore.SoundOut.PlaybackState.Stopped; }
        }

        public TimeSpan Duration
        {
            get
            {
                if (PlayerSource == null)
                    return TimeSpan.Zero;

                try
                {
                    return PlayerSource.GetLength();
                }
                catch
                {
                    return TimeSpan.Zero;
                }
            }
        }

        public TimeSpan CurrentTime
        {
            get
            {
                if (PlayerSource == null)
                    return TimeSpan.Zero;

                try
                {
                    return PlayerSource.GetPosition();
                }
                catch
                {
                    return TimeSpan.Zero;
                }
            }
        }

        public YAMP_Core() : this(SynchronizationContext.Current)
        {
        }

        internal YAMP_Core(SynchronizationContext eventContext)
        {
            _eventContext = eventContext;
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

        public void Seek(TimeSpan position)
        {
            lock (_playerLock)
            {
                if (!PlayerInitialized)
                    return;

                if (!NetPlay || PlayerSource.CanSeek)
                {
                    TimeSpan clampedPosition = position < TimeSpan.Zero ? TimeSpan.Zero : position;
                    if (clampedPosition > Duration)
                        clampedPosition = Duration;

                    Extensions.SetPosition(PlayerSource, clampedPosition);
                }
            }
        }

        public void Seek(int seconds)
        {
            Seek(TimeSpan.FromSeconds(Math.Max(0, seconds)));
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
                if (TryLoadTrack(nextTrack.Path, out string error))
                {
                    CurrentTrack = nextTrack;
                    return true;
                }

                OnTrackLoadFailed(nextTrack.Path, error);
            }
            return false;
        }

        public bool LoadTrackInfo(TrackInfo trackInfo)
        {
            if (trackInfo != null)
            {
                if (TryLoadTrack(trackInfo.Path, out string error))
                {
                    CurrentTrack = trackInfo;
                    return true;
                }

                OnTrackLoadFailed(trackInfo.Path, error);
            }

            return false;
        }

        public void InitializePlayer()
        {
            InitializeCurrentSource();
        }

        public void InitializePlayer(string filename)
        {
            if (!TryLoadTrack(filename, out string error))
                throw new InvalidOperationException(error);
        }

        public bool TryLoadTrack(string path, out string error)
        {
            lock (_playerLock)
            {
                error = string.Empty;
                State = CorePlaybackState.Loading;
                NetPlay = false;

                if (string.IsNullOrWhiteSpace(path))
                {
                    CleanupAfterLoadFailure();
                    error = "No file was selected.";
                    return false;
                }

                if (!File.Exists(path))
                {
                    CleanupAfterLoadFailure();
                    error = "File does not exist.";
                    return false;
                }

                if (!AudioFileSupport.IsSupportedAudioFile(path))
                {
                    CleanupAfterLoadFailure();
                    error = "Unsupported audio format.";
                    return false;
                }

                try
                {
                    PrepareForNewSource(StopReason.TrackChanging);
                    LoadFile(path);
                    PlayerSource = AppendEffectSources(PlayerSource);
                    InitializeCurrentSource();
                    TagInfo = GetID3Info();
                    PlayerStopped = false;
                    PlayerPaused = false;
                    _trackEndedRaised = false;
                    LastStopReason = StopReason.None;
                    State = CorePlaybackState.Ready;
                    return true;
                }
                catch (Exception ex)
                {
                    CleanupAfterLoadFailure();
                    error = ex.Message;
                    return false;
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
                        InitializeCurrentSource();
                        CurrentTrack = !string.IsNullOrEmpty(LocalPath) && File.Exists(LocalPath)
                            ? new TrackInfo(LocalPath)
                            : new TrackInfo();
                        PlayerStopped = false;
                        PlayerPaused = false;
                        _trackEndedRaised = false;
                        LastStopReason = StopReason.None;
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

        private IWaveSource AppendEffectSources(IWaveSource source)
        {
            ResetOwnedEffectReferences();

            source = AppendDecodeStage(source);
            source = AppendDmoEffectStage(source);
            source = AppendLoopStage(source);
            source = AppendGainVolumeStage(source);
            source = AppendPeakMeterStage(source);
            source = AppendPitchFadeStage(source);
            source = AppendPanStage(source);
            source = AppendEqualizerStage(source);
            source = AppendNotificationStage(source);

            return source;
        }

        private IWaveSource AppendDecodeStage(IWaveSource source)
        {
            return source;
        }

        private IWaveSource AppendDmoEffectStage(IWaveSource source)
        {
            DmoDistortionEffect distortionEffect;
            DmoFlangerEffect flangerEffect;
            DmoWavesReverbEffect wavesReverbEffect;
            DmoEchoEffect echoEffect;
            DmoCompressorEffect compressorEffect;
            DmoGargleEffect gargleEffect;
            DmoChorusEffect chorusEffect;

            source = source
                .AppendSource(x => new DmoDistortionEffect(x) { IsEnabled = false }, out distortionEffect)
                .AppendSource(x => new DmoFlangerEffect(x) { IsEnabled = false }, out flangerEffect)
                .AppendSource(x => new DmoWavesReverbEffect(x) { IsEnabled = false }, out wavesReverbEffect)
                .AppendSource(x => new DmoEchoEffect(x) { IsEnabled = false }, out echoEffect)
                .AppendSource(x => new DmoCompressorEffect(x) { IsEnabled = false }, out compressorEffect)
                .AppendSource(x => new DmoGargleEffect(x) { IsEnabled = false }, out gargleEffect)
                .AppendSource(x => new DmoChorusEffect(x) { IsEnabled = false }, out chorusEffect);

            DistortionEffect = YAMPVars.DistortionEffect = distortionEffect;
            FlangerEffect = YAMPVars.FlangerEffect = flangerEffect;
            WavesReverbEffect = YAMPVars.WavesReverbEffect = wavesReverbEffect;
            EchoEffect = YAMPVars.EchoEffect = echoEffect;
            CompressorEffect = YAMPVars.CompressorEffect = compressorEffect;
            GargleEffect = YAMPVars.GargleEffect = gargleEffect;
            ChorusEffect = YAMPVars.ChorusEffect = chorusEffect;

            return source;
        }

        private IWaveSource AppendLoopStage(IWaveSource source)
        {
            LoopStream trackLoop;
            source = source.AppendSource(x => new LoopStream(x) { EnableLoop = false }, out trackLoop);
            TrackLoop = YAMPVars.TrackLoop = trackLoop;
            return source;
        }

        private IWaveSource AppendGainVolumeStage(IWaveSource source)
        {
            GainSource gainSource;
            VolumeSource volumeSource;

            source = source
                .ToSampleSource()
                .AppendSource(x => new GainSource(x) { Volume = 1.0f }, out gainSource)
                .AppendSource(x => new VolumeSource(x) { Volume = 1.0f }, out volumeSource)
                .ToWaveSource();

            GainSource = YAMPVars.GainSource = gainSource;
            VolumeSource = YAMPVars.VolumeSource = volumeSource;

            return source;
        }

        private IWaveSource AppendPeakMeterStage(IWaveSource source)
        {
            PeakMeter audioPeakMeter;

            source = source
                .ToSampleSource()
                .AppendSource(x => new PeakMeter(x) { Interval = 25 }, out audioPeakMeter)
                .ToWaveSource();

            AudioPeakMeter = YAMPVars.AudioPeakMeter = audioPeakMeter;

            return source;
        }

        private IWaveSource AppendPitchFadeStage(IWaveSource source)
        {
            PitchShifter pitchShiftEffect;
            FadeInOut fadeEffect;

            source = source
                .ToSampleSource()
                .AppendSource(x => new PitchShifter(x), out pitchShiftEffect)
                .AppendSource(x => new FadeInOut(x) { FadeStrategy = new LinearFadeStrategy() }, out fadeEffect)
                .ToWaveSource();

            PitchShiftEffect = YAMPVars.PitchShiftEffect = pitchShiftEffect;
            FadeEffect = YAMPVars.FadeEffect = fadeEffect;

            return source;
        }

        private IWaveSource AppendPanStage(IWaveSource source)
        {
            if (source.WaveFormat.Channels <= 1)
                return source;

            PanSource channelPan;

            source = source
                .ToSampleSource()
                .AppendSource(x => new PanSource(x) { Pan = 0.0F }, out channelPan)
                .ToWaveSource();

            ChannelPan = YAMPVars.ChannelPan = channelPan;

            return source;
        }

        private IWaveSource AppendEqualizerStage(IWaveSource source)
        {
            if (source.WaveFormat.SampleRate < 32000)
                return source;

            Equalizer equalizerEffect;

            source = source
                .ToSampleSource()
                .AppendSource(x => Equalizer.Create10BandEqualizer(x), out equalizerEffect)
                .ToWaveSource();

            EqualizerEffect = YAMPVars.EqualizerEffect = equalizerEffect;

            return source;
        }

        private IWaveSource AppendNotificationStage(IWaveSource source)
        {
            ResetOwnedNotificationReferences();

            NotificationSource notificationSource;
            SingleBlockNotificationStream singleBlockNotificationStream;

            source = source
                .ToSampleSource()
                .AppendSource(x => new NotificationSource(x), out notificationSource)
                .ToWaveSource();

            source = source.AppendSource(x => new SingleBlockNotificationStream(x.ToSampleSource(), NOTIFICATION_BLOCK_SIZE), out singleBlockNotificationStream).ToWaveSource();
            NotificationSource = YAMPVars.NotificationSource = notificationSource;
            SingleBlockNotificationStream = YAMPVars.SingleBlockNotificationStream = singleBlockNotificationStream;
            SingleBlockNotificationStream.SingleBlockRead += NotificationStream_SingleBlockRead;
            SingleBlockNotificationStream.SingleBlockStreamAlmostFinished += NotificationStream_SingleBlockStreamAlmostFinished;
            SingleBlockNotificationStream.SingleBlockStreamFinished += NotificationStream_SingleBlockStreamFinished;

            return source;
        }

        public void CreateNotificationEvents()
        {
            PlayerSource = AppendNotificationStage(PlayerSource);
        }

        private void NotificationStream_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {

            WaveFormLEFT = e.Left;
            WaveFormRIGHT = e.Right;
        }

        private void NotificationStream_SingleBlockStreamAlmostFinished(object sender, SingleBlockStreamAlmostFinishedEventArgs e)
        {
            // Apply fade-out effect near the end of the track if enabled
            if (EnableFade && FadeEffect != null)
            {
                // Find the remaining seconds after SingleBlockStreamAlmostFinished event triggers
                // This is the duration over which the fade out will be applied
                TimeSpan REMSEC = PlayerSource.GetLength() - PlayerSource.GetPosition();

                // Starting volume set to null to use default/current volume
                FadeEffect.FadeStrategy.StartFading(null, 0, REMSEC);
            }
        }

        private void NotificationStream_SingleBlockStreamFinished(object sender, SingleBlockStreamFinishedEventArgs e)
        {
            if (!NetPlay || PlayerSource.CanSeek)
            {
                PlayerPaused = false;

                if (LastStopReason == StopReason.None)
                {
                    LastStopReason = StopReason.TrackFinished;
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
            RaiseEvent(TrackEnded, EventArgs.Empty);
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
            lock (_playerLock)
            {
                if (!PlayerInitialized)
                    return;

                LastStopReason = StopReason.None;
                Player.Play();
                PlayerStopped = false;
                PlayerPaused = false;
                State = CorePlaybackState.Playing;
            }
        }

        public void Stop()
        {
            lock (_playerLock)
            {
                if (!PlayerInitialized)
                    return;

                LastStopReason = StopReason.UserStopped;
                State = CorePlaybackState.Stopping;
                if (!NetPlay || PlayerSource.CanSeek)
                {
                    Seek(TimeSpan.Zero);
                }
                Player.Stop();
                PlayerStopped = false;
                PlayerPaused = false;
            }
        }

        public void Pause()
        {
            lock (_playerLock)
            {
                if (!PlayerInitialized)
                    return;

                Player.Pause();
                PlayerPaused = true;
                State = CorePlaybackState.Paused;
            }
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
            LastStopReason = StopReason.None;
            PlayerStopped = false;
            PlayerPaused = false;
            _trackEndedRaised = false;
        }

        private void UnsubscribeNotificationEvents()
        {
            if (SingleBlockNotificationStream != null)
            {
                SingleBlockNotificationStream.SingleBlockRead -= NotificationStream_SingleBlockRead;
                SingleBlockNotificationStream.SingleBlockStreamAlmostFinished -= NotificationStream_SingleBlockStreamAlmostFinished;
                SingleBlockNotificationStream.SingleBlockStreamFinished -= NotificationStream_SingleBlockStreamFinished;
            }
        }

        private void ResetOwnedEffectReferences()
        {
            YAMPVars.ResetEffectVars();
            DistortionEffect = null;
            FlangerEffect = null;
            WavesReverbEffect = null;
            EchoEffect = null;
            CompressorEffect = null;
            GargleEffect = null;
            ChorusEffect = null;
            TrackLoop = YAMPVars.TrackLoop = null;
            GainSource = YAMPVars.GainSource = null;
            VolumeSource = YAMPVars.VolumeSource = null;
            AudioPeakMeter = YAMPVars.AudioPeakMeter = null;
            PitchShiftEffect = YAMPVars.PitchShiftEffect = null;
            FadeEffect = YAMPVars.FadeEffect = null;
            ChannelPan = YAMPVars.ChannelPan = null;
            EqualizerEffect = YAMPVars.EqualizerEffect = null;
        }

        private void ResetOwnedNotificationReferences()
        {
            YAMPVars.ResetStreamNotifications();
            NotificationSource = YAMPVars.NotificationSource = null;
            SingleBlockNotificationStream = YAMPVars.SingleBlockNotificationStream = null;
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

        private void RaiseEvent(EventHandler handler, EventArgs args)
        {
            if (handler == null)
                return;

            if (_eventContext == null || SynchronizationContext.Current == _eventContext)
            {
                handler(this, args);
                return;
            }

            _eventContext.Post(_ => handler(this, args), null);
        }

        private void RaiseEvent<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs args)
        {
            if (handler == null)
                return;

            if (_eventContext == null || SynchronizationContext.Current == _eventContext)
            {
                handler(this, args);
                return;
            }

            _eventContext.Post(_ => handler(this, args), null);
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

    public class TrackLoadFailedEventArgs : EventArgs
    {
        public TrackLoadFailedEventArgs(string path, string error)
        {
            Path = path;
            Error = error;
        }

        public string Path { get; private set; }
        public string Error { get; private set; }
    }
}
