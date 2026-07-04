using CSCore;
using CSCore.DSP;
using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class NewMain : Form
    {
        YAMPEnums.PanelMode PanelMode = YAMPEnums.PanelMode.Cover;
        private static readonly Regex TimestampRegex = new Regex(@"^(?'minutes'\d+):(?'seconds'\d+(\.\d+)?)$");
        GraphVisualization visualisation = null;
        private string _curlyrln = "";
        private bool RefreshBrushes;
        private BasicSpectrumProvider _circularSpectrumProvider;
        private PeakHoldSpectrumProvider peakHoldProvider;
        private SmoothingSpectrumProvider smoothingProvider;
        private CircularSpectrum _circularSpectrum;
        private HorizontalBarSpectrum _horizontalBarSpectrum;
        private ModernWaveformSpectrum _modernWaveform;

        private event EventHandler LyricLineChanged;
        private string CurrentLyricLine
        {
            get { return _curlyrln; }
            set
            {
                // Only trigger change event if value actually changed
                // Normalize empty/null to empty string to prevent spurious events
                string normalizedValue = string.IsNullOrEmpty(value) ? string.Empty : value;
                
                if (_curlyrln != normalizedValue)
                {
                    _curlyrln = normalizedValue;
                    LyricLineChanged?.Invoke(normalizedValue, EventArgs.Empty);
                }
            }
        }

        public NewMain()
        {
            InitializeComponent();
            int ClientTop = RectangleToScreen(ClientRectangle).Top;
            int height = Height - CoverImageBox.Height;
            MinimumSize = new Size(400, height);
            UpdateVisualSpectrumChannelCheck();
            leftChannelToolStripMenuItem.CheckStateChanged += SpectrumDrawChannel_CheckedChanged;
            rightChannelToolStripMenuItem.CheckStateChanged += SpectrumDrawChannel_CheckedChanged;
        }

        private void NotificationSource_BlockRead(object sender, EventArgs e)
        {
            // Only update lyrics if we're in lyrics mode and have valid data
            if (PanelMode == YAMPEnums.PanelMode.Lyrics && 
                YAMPVars.CORE?.CurrentTrack?.Lyrics != null &&
                YAMPVars.CORE.PlayerSource != null)
            {
                try
                {
                    var TotalSeconds = Extensions.GetPosition(YAMPVars.CORE.PlayerSource).TotalSeconds;
                    string LyricLine = YAMPVars.CORE.CurrentTrack.Lyrics.LastOrDefault(x => x.Key < TotalSeconds).Value;
                    
                    // Only set if there's actual lyric text, otherwise don't trigger unnecessary repaints
                    if (!string.IsNullOrEmpty(LyricLine))
                    {
                        CurrentLyricLine = LyricLine;
                    }
                    else if (!string.IsNullOrEmpty(CurrentLyricLine))
                    {
                        // Only clear if we previously had text (prevents repeated empty->empty refreshes)
                        CurrentLyricLine = string.Empty;
                    }
                }
                catch
                {
                    // Silently ignore errors in event handler to prevent exceptions from breaking playback
                }
            }
        }

        private void UpdateTrackers()
        {
            DurationTracker.Value = 0;
            VolumeTracker.Value = YAMPVars.CORE.SoundOutVolume;
            if (YAMPVars.CORE.PlayerSource.CanSeek || !YAMPVars.CORE.NetPlay)
            {
                DurationTracker.Maximum = (int)Extensions.GetLength(YAMPVars.CORE.PlayerSource).TotalSeconds;
            }
        }

        private void UpdateVisualSpectrumChannelCheck()
        {
            leftChannelToolStripMenuItem.Checked = YAMPVars.DrawLeftChannelSpectrum;
            rightChannelToolStripMenuItem.Checked = YAMPVars.DrawRightChannelSpectrum;
        }

        private void PlayFromStart(bool FadeTrack = true)
        {
            DurationTracker.Value = 0;
            PlayTimer.Start();
            YAMPVars.CORE.Play();
            if (YAMPVars.CORE.EnableFade && FadeTrack)
            {
                YAMPVars.FadeEffect.FadeStrategy.StartFading(0, 1, 5000D);
            }
        }

        private void ThreadSafeCall(MethodInvoker method)
        {
            if (InvokeRequired)
            {
                BeginInvoke(method);
            }
            else
            {
                method.Invoke();
            }
        }

        private void NewMain_Load(object sender, EventArgs e)
        {
            YAMPVars.CORE = new YAMP_Core();
            YAMPVars.DrawLeftChannelSpectrum = leftChannelToolStripMenuItem.Checked;
            YAMPVars.DrawRightChannelSpectrum = rightChannelToolStripMenuItem.Checked;
            YAMPVars.CORE.TrackChanged += CORE_TrackChanged;
            YAMPVars.CORE.TrackEnded += CORE_TrackEnded;
            //YAMPVars.NotificationSource.BlockRead += NotificationSource_BlockRead;
        }

        private void CORE_TrackEnded(object sender, EventArgs e)
        {
            // Handle UI updates on the UI thread when a track ends naturally.
            ThreadSafeCall(() =>
            {
                if (PlayTimer.Enabled)
                    PlayTimer.Stop();
                // final UI updates can be added here if needed
            });
        }

        internal void PlayfromPlaylist(TrackInfo Track)
        {
            if (PlayTimer.Enabled)
            {
                PlayTimer.Stop();
            }
            if (YAMPVars.CORE.LoadTrackInfo(Track))
            {
                PlayFromStart();
            }
        }

        private void CORE_TrackChanged(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.CurrentTrack != null && YAMPVars.CORE.PlayerSource != null)
            {
                UpdateTrackers();

                // Get cover image (belongs to TrackInfo, don't dispose)
                Image cover = GetUsableTrackCover();

                // Only dispose the previous background if it was temporary (not a track cover)
                var prevImage = CoverImageBox.BackgroundImage;
                if (prevImage != null && !IsTrackCoverImage(prevImage))
                {
                    prevImage.Dispose();
                }

                CoverImageBox.BackgroundImage = cover;
                ResizePlayer(cover);
                Lbl_PlayerLabel.Text = string.Format(">  {0}", YAMPVars.CORE.CurrentTrack.Title);
                Lbl_Duration.Text = TrackDurationText();
            }
        }

        private int GetAdditionalPlayerHeight()
        {
            return ClientRectangle.Height - CoverImageBox.Height + (Height - ClientRectangle.Height);
        }

        private void ParseLRC(string path)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("Lyrics file path is empty.", "Parse Error");
                return;
            }

            if (YAMPVars.TrackList == null || YAMPVars.TrackList.Count == 0)
            {
                MessageBox.Show("No track loaded. Please load a track first.", "Parse Error");
                return;
            }

            try
            {
                var Lines = File.ReadAllLines(path);
                YAMPVars.TrackList[0].Lyrics = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<double, string>>();
                foreach (var line in Lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue; // Skip empty lines

                    int TimeStampEnd = line.IndexOf(']');
                    if (TimeStampEnd <= 0)
                        continue; // Skip lines without valid timestamp

                    if (LyricsHelper.TryParseLrcString(line, 1, TimeStampEnd - 1, out TimeSpan res))
                    {
                        YAMPVars.TrackList[0].Lyrics.Add(
                            new System.Collections.Generic.KeyValuePair<double, string>(
                                res.TotalSeconds, 
                                line.Substring(TimeStampEnd + 1)));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing lyrics file: {ex.Message}", "Parse Error");
            }
        }

        private void ResizePlayer(Image cover = null)
        {
            if (IsUsableImage(cover))
            {
                int Border = Width - ClientRectangle.Width;
                double dImageAR = cover.Width / (double)cover.Height;
                if (dImageAR <= 0)
                    return;

                int _width = ClientRectangle.Width;
                int _height = ClientRectangle.Height;
                if (_width <= 0 || _height <= 0)
                    return;

                double dImgWidth = _height * dImageAR;
                double dImgHeight;
                if (_width < dImgWidth)
                {
                    dImgWidth = _width;
                    dImgHeight = dImgWidth / dImageAR;
                }
                else
                {
                    dImgHeight = _height;
                }
                Size = new Size((int)dImgWidth + Border, (int)dImgHeight + GetAdditionalPlayerHeight());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.PlayerSource == null)
            {
                bool TrackLoaded = YAMPVars.CORE.GetFirstTrack();
                if (!TrackLoaded)
                {
                    using (OpenFileDialog OPD = new OpenFileDialog() { Filter = AudioFileSupport.OpenFileFilter })
                    {
                        if (OPD.ShowDialog() == DialogResult.OK)
                        {
                            //
                            //
                            // Try to create a cutter with repositioning and reading till cut part 
                            //
                            //
                            TrackInfo track = new TrackInfo(OPD.FileName);
                            YAMPVars.TrackList.Add(track);

                            TrackLoaded = YAMPVars.CORE.GetFirstTrack();
                        }
                    }
                }
                if (TrackLoaded)
                {
                    PlayFromStart();
                }
            }
            else
            {
                if (YAMPVars.CORE.PlayerPlaybackState != CSCore.SoundOut.PlaybackState.Playing)
                {
                    YAMPVars.CORE.Play();
                    PlayTimer.Start();
                }
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.PlayerInitialized)
            {
                YAMPVars.CORE.SoundOutVolume = VolumeTracker.Value;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.PlayerPlaybackState == CSCore.SoundOut.PlaybackState.Playing)
            {
                YAMPVars.CORE.Pause();
                PlayTimer.Stop();
            }
        }

        private string TrackDurationText()
        {
            TimeSpan Duration = Extensions.GetPosition(YAMPVars.CORE.PlayerSource);
            DurationTracker.Value = (Duration.Minutes * 60) + Duration.Seconds;
            return string.Format("{0}\\{1}", Duration.ToString(@"mm\:ss"), TimeSpan.FromSeconds(DurationTracker.Maximum).ToString(@"mm\:ss"));
        }

        private void PlayTimer_Tick(object sender, EventArgs e)
        {
            // Priority 1: Check pause FIRST (was unreachable in old position)
            if (YAMPVars.CORE.PlayerPaused)
            {
                PlayTimer.Stop();
                return;
            }

            // Priority 2: Check if track finished naturally
            if (YAMPVars.CORE.PlayerStopped)
            {
                PlayTimer.Stop();
                
                // Auto-play next track (forward direction only)
                // If no next track exists, playback stops here
                if (YAMPVars.CORE.PlayNextTrackDirected(1))
                {
                    PlayFromStart();
                }
                return;
            }

            // Priority 3: Normal playback - update UI
            if (!YAMPVars.CORE.NetPlay || YAMPVars.CORE.PlayerSource.CanSeek)
            {
                Lbl_Duration.Text = TrackDurationText();
            }
        }

        private void NewMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (YAMPVars.CORE != null)
            {
                YAMPVars.CORE.TrackEnded -= CORE_TrackEnded;
                YAMPVars.CORE.Stop();
                YAMPVars.CORE.Dispose();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!YAMPVars.CORE.PlayerStopped)
            {
                if (!YAMPVars.CORE.NetPlay || YAMPVars.CORE.PlayerSource.CanSeek)
                {
                    DurationTracker.Value = 0;
                    YAMPVars.CORE.PlayerSource.Position = 0;
                }
                YAMPVars.CORE.Stop();
            }
        }

        private void LoadFileStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OPD = new OpenFileDialog() { Filter = AudioFileSupport.OpenFileFilter })
            {
                if (OPD.ShowDialog() == DialogResult.OK)
                {
                    TrackInfo Track = new TrackInfo(OPD.FileName);
                    if (YAMPVars.CORE.LoadTrackInfo(Track))
                    {
                        VolumeTracker.Value = YAMPVars.CORE.SoundOutVolume;
                        YAMPVars.TrackList.Add(YAMPVars.CORE.CurrentTrack);
                        CoverImageBox.BackgroundImage = GetUsableTrackCover();
                    }
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            YAMPVars.CORE.AdjustPlayerPosition(DurationTracker.Value);
        }

        private void pitchShifterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PitchShiftDialog PSD = new PitchShiftDialog();
            PSD.Show();
        }

        private void echoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EchoSignalDialog ESD = new EchoSignalDialog();
            ESD.Show();
        }

        private void peakMtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PeakMeterDialog PMD = new PeakMeterDialog();
            PMD.Show();
        }

        private void gargleEffectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GargleEffectDialog GED = new GargleEffectDialog();
            GED.Show();
        }

        private void flangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlangerEffectDialog FED = new FlangerEffectDialog();
            FED.Show();
        }

        private void chorusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChorusEffectDialog CED = new ChorusEffectDialog();
            CED.Show();
        }

        private void compressorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompressorEffectDialog CmpED = new CompressorEffectDialog();
            CmpED.Show();
        }

        private void wavesReverbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WavesReverbEffectDialog WRED = new WavesReverbEffectDialog();
            WRED.Show();
        }

        private void waveformNAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Waveform WF = new Waveform();
            WF.Show();
        }

        private void vUMeterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VUMeterDialog VUDialog = new VUMeterDialog();
            VUDialog.Show();
        }

        private void equalizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EqualizerDialog EQDialog = new EqualizerDialog();
            EQDialog.Show();
        }

        private void playlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YAMPlaylistDialog playlist = new YAMPlaylistDialog();
            playlist.TrackSelected += Playlist_TrackSelected;
            playlist.ShowDialog(this);
            playlist.TrackSelected -= Playlist_TrackSelected;
        }

        private void Playlist_TrackSelected(object sender, TrackSelectedEventArgs e)
        {
            PlayfromPlaylist(e.Track);
        }

        private void Btns_TrackShift_Click(object sender, EventArgs e)
        {
            int direction = int.Parse(((Button)sender).Tag.ToString());
            
            // Directly play next/previous track
            // PlayNextTrackDirected already validates boundaries (first/last track, single track, etc.)
            if (YAMPVars.CORE.PlayNextTrackDirected(direction))
            {
                PlayFromStart();
            }
            // If no track exists in that direction, PlayNextTrackDirected returns false
            // and nothing happens (stays on current track)
        }

        private void BtnSkipSec_Click(object sender, EventArgs e)
        {
            int SecToSkip = int.Parse(((Button)sender).Tag.ToString());
            if (DurationTracker.Value + SecToSkip < 0)
            {
                DurationTracker.Value = 0;
            }
            else if ((DurationTracker.Value + SecToSkip > DurationTracker.Maximum) == false)
            {
                DurationTracker.Value += SecToSkip;
            }
            YAMPVars.CORE.AdjustPlayerPosition(DurationTracker.Value);
        }

        private void distortionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DistortionEffectDialog distortionEffect = new DistortionEffectDialog();
            distortionEffect.Show();
        }

        private void LoadDirStripMenuItem_Click(object sender, EventArgs e)
        {
            bool shouldLoadFirstTrack =
                YAMPVars.CORE.PlayerSource == null &&
                (YAMPVars.TrackList == null || YAMPVars.TrackList.Count == 0);

            YAMPlaylistDialog.LoadDirectory();

            if (shouldLoadFirstTrack && YAMPVars.TrackList != null && YAMPVars.TrackList.Count > 0)
            {
                YAMPVars.CORE.GetFirstTrack();
            }
        }

        private void streamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string StreamURL = Interaction.InputBox("Input Stream URL", "Stream URL");
            if (YAMPVars.CORE.InitializePlayerNet(StreamURL))
            {
                UpdateTrackers();
                PlayFromStart();
            }
        }

        private void Btn_ToggleExtras_Click(object sender, EventArgs e)
        {
            Pnl_Extras.Visible = !Pnl_Extras.Visible;
            Btn_ToggleExtras.Text = Pnl_Extras.Visible ? "-" : "+";
        }

        private void CB_ToggleTrackLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (YAMPVars.TrackLoop != null)
            {
                YAMPVars.TrackLoop.EnableLoop = CB_ToggleTrackLoop.Checked;
            }
        }

        private void Btn_ToggleFade_CheckedChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.EnableFade = Btn_ToggleFade.Checked;
        }

        private void Btn_ChannelPan_Click(object sender, EventArgs e)
        {
            PanSlider PanSlide = new PanSlider
            {
                StartPosition = FormStartPosition.Manual,
            };
            PanSlide.Show();
        }

        private void DurationTracker_ValueChanged(object sender, EventArgs e)
        {
            if (YAMPVars.TrackPositionLoop != null && Btn_PosLoop.Tag.ToString() == "B")
            {
                if (DurationTracker.Value >= YAMPVars.TrackPositionLoop.B)
                {
                    DurationTracker.Value = YAMPVars.TrackPositionLoop.A;
                    YAMPVars.CORE.AdjustPlayerPosition(YAMPVars.TrackPositionLoop.A);
                }
            }
        }

        private void Btn_PosLoop_Click(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.Player != null)
            {
                Button Btn = sender as Button;
                string BtnTag = Btn.Tag.ToString();
                switch (BtnTag)
                {
                    case "*":
                        YAMPVars.TrackPositionLoop = new PositionLoop() { A = DurationTracker.Value };
                        Btn.Text = "A→";
                        Btn.Tag = "A";
                        break;
                    case "A":
                        YAMPVars.TrackPositionLoop.B = DurationTracker.Value;
                        Btn.Text = "A↔B";
                        Btn.Tag = "B";
                        DurationTracker.Value = YAMPVars.TrackPositionLoop.A;
                        YAMPVars.CORE.AdjustPlayerPosition(YAMPVars.TrackPositionLoop.A);
                        break;
                    default:
                        YAMPVars.TrackPositionLoop = null;
                        Btn.Text = "AB";
                        Btn.Tag = "*";
                        break;
                }
            }
        }

        private void audioCutterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CutterDialog() { StartPosition = FormStartPosition.CenterParent }.ShowDialog();
        }

        private void sampleRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ResamplerDialog()
            {
                StartPosition = FormStartPosition.CenterParent
            }.ShowDialog();
        }

        private void bitRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new BitrateDialog()
            {
                StartPosition = FormStartPosition.CenterParent
            }.ShowDialog();
        }

        private void signalFilteringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SignalFilterDialog().Show();
        }

        private void tagEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TagEditorDialog(YAMPVars.CORE.PlayingFile).ShowDialog();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.PlayerInitialized)
            {
                PanelMode = Enum.IsDefined(typeof(YAMPEnums.PanelMode), (int)PanelMode + 1) ? (YAMPEnums.PanelMode)((int)PanelMode + 1) : 0;
                UpdatePanel(PanelMode);
            }
        }

        private void UpdatePanel(YAMPEnums.PanelMode mode)
        {
            // Clean up event handlers first
            //YAMPVars.NotificationSource.BlockRead -= NotificationSource_BlockRead;
            YAMPVars.SingleBlockNotificationStream.SingleBlockRead -= SingleBlockNotificationStream_SingleBlockRead;
            CoverImageBox.Paint -= CoverImageBox_Paint;
            LyricLineChanged -= NewMain_LyricLineChanged;

            // Stop visualizer
            visualizer.Stop();
            visualisation = null;

            // Only dispose if BackgroundImage is a temporary/generated image, not a reference from TrackInfo
            var currentBgImage = CoverImageBox.BackgroundImage;
            if (currentBgImage != null && !IsTrackCoverImage(currentBgImage))
            {
                currentBgImage.Dispose();
            }
            CoverImageBox.BackgroundImage = null;

            // Set up the new mode
            switch (mode)
            {
                case YAMPEnums.PanelMode.Cover:
                    SetupCoverMode();
                    break;

                case YAMPEnums.PanelMode.Waveform:
                    SetupSpectrumMode();
                    break;

                case YAMPEnums.PanelMode.Circular:
                    SetupCircularMode();
                    break;

                case YAMPEnums.PanelMode.Lyrics:
                    SetupLyricsMode();
                    break;

                case YAMPEnums.PanelMode.Bars:
                    SetupHorizontalBarsMode();
                    break;

                case YAMPEnums.PanelMode.ModernWaveform:
                    SetupModernWaveformMode();
                    break;
            }
        }

        private void SetupCircularMode()
        {
            const FftSize fftSize = FftSize.Fft4096;
            
            // Create spectrum provider for circular visualization
            _circularSpectrumProvider = new BasicSpectrumProvider(YAMPVars.CORE.Player.WaveSource.WaveFormat.Channels,
                YAMPVars.CORE.Player.WaveSource.WaveFormat.SampleRate, fftSize);

            // Create circular spectrum with configurable style
            _circularSpectrum = new CircularSpectrum(fftSize)
            {
                SpectrumProvider = _circularSpectrumProvider,
                UseAverage = true,
                BarCount = 60, // More bars for circular looks better
                BarWidth = 3.0f,
                InnerRadius = 50,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Sqrt,
                
                // Style options:
                // - FullCircle: Standard full 360° circle (may look unbalanced due to frequency differences)
                // - SymmetricMirror: Mirrors frequency data at 180° for perfectly balanced look
                // - MusicalRange: Uses frequency cutoff (60Hz-8kHz) for more balanced visualization
                // - SemiCircle, HalfArcBottom, HalfArcTop: Various arc styles
                // - MirrorMode: Bars grow both inward and outward
                // - DualRing: Two concentric rings
                Style = CircularSpectrumStyle.SymmetricMirror, // Best for balanced circular appearance
                
                EnableRotation = false // Set to true for rotating effect
            };

            YAMPVars.SingleBlockNotificationStream.SingleBlockRead += SingleBlockNotificationStream_SingleBlockRead;

            visualizer.Start();
        }

        private void SetupHorizontalBarsMode()
        {
            const FftSize fftSize = FftSize.Fft4096;

            // Choose spectrum provider type:
            // Option 1: PeakHoldSpectrumProvider - shows falling peak indicators  
            // NOTE: This is kept for backward compatibility, but HorizontalBarSpectrum
            // now has its own built-in peak tracking that respects bar scaling
            //peakHoldProvider = new PeakHoldSpectrumProvider(
            //    YAMPVars.CORE.Player.WaveSource.WaveFormat.Channels,
            //    YAMPVars.CORE.Player.WaveSource.WaveFormat.SampleRate,
            //    fftSize,
            //    peakHoldFrames: 10,      // Hold peaks for 10 frames before decay
            //    peakDecayRate: 0.9f      // Decay rate (0.9 = fast, 0.98 = slow)
            //);

            // Option 2: SmoothingSpectrumProvider - smooth, flowing bars
            smoothingProvider = new SmoothingSpectrumProvider(
                YAMPVars.CORE.Player.WaveSource.WaveFormat.Channels,
                YAMPVars.CORE.Player.WaveSource.WaveFormat.SampleRate,
                fftSize,
                attackTime: 0.05f,   // 50ms attack
                releaseTime: 0.01f,   // 100ms release
                frameRate: 60       // 60 FPS
            );
            // Or use preset: smoothingProvider.SetSmoothingPreset(SmoothingPreset.Medium);

            // Create horizontal bar spectrum with built-in peak tracking
            _horizontalBarSpectrum = new HorizontalBarSpectrum(fftSize)
            {
                SpectrumProvider = smoothingProvider,  // Use any provider
                UseAverage = true,
                BarCount = 30,
                BarSpacing = 2,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Sqrt,
                MinimumFrequency = 60,    // 60Hz minimum for better visual balance
                MaximumFrequency = 7000,  // 8kHz maximum to avoid excessive high-frequency bars
                ShowPeakIndicators = false,
                PeakIndicatorColor = Color.Red,
                
                // Peak modes:
                // - FallingPeak: Peaks decay gradually (classic analyzer behavior)
                // - NeverFall: Peaks stay at maximum forever (session peak tracking)
                // - InstantFall: Peaks follow bar height instantly (visual accent)
                // - NoPeaks: No peak indicators displayed (clean bars only)
                PeakMode = PeakHoldMode.FallingPeak,  // Try NeverFall, InstantFall, or NoPeaks

                PeakHoldFrames = 15,      // Hold for 15 frames (~250ms @ 60fps)
                PeakDecayRate = 0.98f,    // Decay rate (0.9 = fast, 0.98 = slow)
                
                RenderDirection = BarSpectrumRenderDirection.VerticalBottomToTop
            };

            YAMPVars.SingleBlockNotificationStream.SingleBlockRead += SingleBlockNotificationStream_SingleBlockRead;

            visualizer.Start();
        }

        private void SetupModernWaveformMode()
        {
            // Dispose previous instance if exists
            _modernWaveform?.Dispose();

            // Get sample rate from player for accurate frequency filtering
            int sampleRate = YAMPVars.CORE.Player.WaveSource.WaveFormat.SampleRate;

            // Create modern waveform with configurable style
            _modernWaveform = new ModernWaveformSpectrum(4096)
            {
                // Visual style options: Line, FilledMirror, Bars, Points, AreaFill, MirroredBars
                Style = WaveformStyle.Line,

                // Colors
                LeftChannelColor = Color.FromArgb(0, 200, 255),    // Cyan
                RightChannelColor = Color.FromArgb(255, 100, 150), // Pink
                BackgroundColor = Color.Black,

                // Channel rendering
                RenderChannel = WaveformChannel.Left,

                // Visual effects
                EnableGlow = false,
                EnableGradientFill = false,
                ShowCenterLine = false,
                CenterLineColor = Color.FromArgb(60, 255, 255, 255),
                ShowGrid = false,

                // Rendering quality
                LineThickness = 1f,
                AmplitudeScale = 0.8f,
                EnableAntiAliasing = true,
                RenderResolution = 128,       // Lower = smoother curves (fewer points)

                // Smoothing and decay
                SmoothingFactor = 0.4f,       // 0 = sharp, 1 = very smooth
                EnableDecay = true,           // Smooth amplitude falloff animation
                DecayRate = 0.90f,            // 0 = instant, 0.99 = very slow decay
                AttackRate = 0.75f,           // How fast amplitude rises (0 = instant)

                // Curved lines - makes peaks smooth and organic
                UseCurvedLines = true,        // Use bezier curves instead of straight lines
                CurveTension = 0.5f,          // 0 = angular, 1 = very curvy (0.5 is balanced)

                // Frequency filtering
                SampleRate = sampleRate,
                EnableFrequencyFilter = true,
                MinimumFrequency = 5000f,
                MaximumFrequency = 16000f,

                // Labels
                ShowAmplitudeLabels = false
            };

            YAMPVars.SingleBlockNotificationStream.SingleBlockRead += SingleBlockNotificationStream_SingleBlockRead;

            visualizer.Start();
        }

        /// <summary>
        /// Determines if the given image is owned by a track in the playlist.
        /// Track cover images should NOT be disposed as they are owned by TrackInfo objects.
        /// </summary>
        private bool IsTrackCoverImage(Image image)
        {
            if (image == null || YAMPVars.TrackList == null)
                return false;

            return YAMPVars.TrackList.Any(track =>
                track?.Covers != null &&
                track.Covers.Any(cover => ReferenceEquals(cover, image)));
        }

        private Image GetUsableTrackCover()
        {
            Image cover = YAMPVars.CORE?.GetTrackCover();
            return IsUsableImage(cover) ? cover : null;
        }

        private bool IsUsableImage(Image image)
        {
            if (image == null)
                return false;

            try
            {
                return image.Width > 0 && image.Height > 0;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private void SetupCoverMode()
        {
            Image cover = GetUsableTrackCover();
            if (cover != null)
            {
                // Use the actual track cover (no disposal needed - it belongs to TrackInfo)
                CoverImageBox.BackgroundImage = cover;
            }
            else
            {
                // Create a temporary black background (this one CAN be disposed later)
                Bitmap bmp = new Bitmap(CoverImageBox.Width, CoverImageBox.Height);
                using (Graphics gBmp = Graphics.FromImage(bmp))
                {
                    gBmp.Clear(Color.Black);
                }
                CoverImageBox.BackgroundImage = bmp;
            }
        }

        private void SetupSpectrumMode()
        {
            leftChannelToolStripMenuItem.Checked = YAMPVars.DrawLeftChannelSpectrum;
            rightChannelToolStripMenuItem.Checked = YAMPVars.DrawRightChannelSpectrum;

            visualisation = new GraphVisualization();
            YAMPVars.SingleBlockNotificationStream.SingleBlockRead += SingleBlockNotificationStream_SingleBlockRead;
            visualizer.Start();
        }

        private void SetupLyricsMode()
        {
            CoverImageBox.Paint += CoverImageBox_Paint;
            LyricLineChanged += NewMain_LyricLineChanged;

            if (YAMPVars.CORE?.CurrentTrack?.Lyrics != null)
            {
                //YAMPVars.NotificationSource.BlockRead += NotificationSource_BlockRead;
            }
            else
            {
                CurrentLyricLine = "No lyrics found. Load a file...";
            }
        }

        private void NewMain_LyricLineChanged(object sender, EventArgs e)
        {
            ThreadSafeCall(CoverImageBox.Refresh);
        }

        private void CoverImageBox_Paint(object sender, PaintEventArgs e)
        {
            if (PanelMode == YAMPEnums.PanelMode.Lyrics)
            {
                var LyricsRect = LyricsHelper.UpdateLyricRect(CurrentLyricLine, CoverImageBox.DisplayRectangle, LyricsHelper.LyricsFont);

                LyricsHelper.UpdateLyricsWriterBrush(LyricsHelper.GetTextRectangle(CoverImageBox.DisplayRectangle, CurrentLyricLine, LyricsHelper.LyricsFont));
                LyricsHelper.UpdateLyricsBorderBrush(LyricsHelper.UpdateLyricRect(CurrentLyricLine, CoverImageBox.DisplayRectangle, LyricsHelper.LyricsFont));
                LyricsHelper.UpdateLyricsHighlightBrush(ref LyricsHelper.LyricsHighlightBrush, LyricsRect, LyricsHelper.EnableLyricsHighlightGradient);

                if (LyricsHelper.EnableLyricsHighlight)
                {
                    e.Graphics.FillRectangle(LyricsHelper.LyricsHighlightBrush, LyricsRect);
                }
                if (LyricsHelper.EnableLyricsBorder)
                {
                    e.Graphics.DrawRectangle(new Pen(LyricsHelper.LyricsBorderBrush), LyricsRect);
                }
                e.Graphics.DrawString(CurrentLyricLine, LyricsHelper.LyricsFont, LyricsHelper.LyricsWriterBrush, CoverImageBox.DisplayRectangle, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
        }

        private void SingleBlockNotificationStream_SingleBlockRead(object sender, CSCore.Streams.SingleBlockReadEventArgs e)
        {
            switch (PanelMode)
            {
                case YAMPEnums.PanelMode.Waveform:
                    visualisation?.AddSamples(e.Left, e.Right);
                    break;
                case YAMPEnums.PanelMode.Circular:
                    _circularSpectrumProvider?.Add(e.Left, e.Right);
                    break;
                case YAMPEnums.PanelMode.Bars:
                    smoothingProvider?.Add(e.Left, e.Right);
                    break;
                case YAMPEnums.PanelMode.ModernWaveform:
                    _modernWaveform?.AddSamples(e.Left, e.Right);
                    break;
            }
        }

        private void visualizer_Tick(object sender, EventArgs e)
        {
            Image image = CoverImageBox.Image;
            Image newImage = null;
            if (PanelMode == YAMPEnums.PanelMode.Waveform)
            {
                newImage = visualisation?.Draw(CoverImageBox.Width, CoverImageBox.Height);
            }
            else if (PanelMode == YAMPEnums.PanelMode.Circular)
            {
                if (_circularSpectrum != null)
                {
                    // Optional: Add rotation animation
                    if (_circularSpectrum.EnableRotation)
                    {
                        _circularSpectrum.Rotation += 2.0f; // Rotate 2 degrees per frame
                    }
                    
                    newImage = _circularSpectrum.CreateCircularSpectrum(CoverImageBox.Size, Color.Cyan, Color.Purple, Color.Black, true);
                }
            }
            else if (PanelMode == YAMPEnums.PanelMode.Bars)
            {
                if (_horizontalBarSpectrum != null)
                {
                    newImage = _horizontalBarSpectrum.CreateHorizontalBarSpectrum(
                        CoverImageBox.Size, 
                        Color.Lime,      // Start color (left)
                        Color.Red,       // End color (right)
                        Color.Black,     // Background
                        true             // High quality
                    );
                }
            }
            else if (PanelMode == YAMPEnums.PanelMode.ModernWaveform)
            {
                if (_modernWaveform != null)
                {
                    newImage = _modernWaveform.Draw(CoverImageBox.Width, CoverImageBox.Height);
                }
            }
            if (newImage != null)
            {
                CoverImageBox.BackgroundImage = newImage;
                if (image != null)
                    image.Dispose();
            }
        }

        private void NewMain_SizeChanged(object sender, EventArgs e)
        {

        }

        private void lyricsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog())
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    ParseLRC(OFD.FileName);
                    if (PanelMode == YAMPEnums.PanelMode.Lyrics)
                    {
                        //YAMPVars.NotificationSource.BlockRead += NotificationSource_BlockRead;
                    }
                }
            }
        }

        private void lyricsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (LyricsConfig LCDiag = new LyricsConfig())
            {
                RefreshBrushes = false;
                LCDiag.ShowDialog();
                //LyricsHelper.UpdateLyricsBorderBrush(LyricsHelper.UpdateLyricRect(CurrentLyricLine, CoverImageBox.DisplayRectangle, LyricsHelper.LyricsFont));
                //LyricsHelper.UpdateLyricsWriterBrushArea(DisplayRectangle);
                //LyricsHelper.UpdateLyricsBorderBrush(DisplayRectangle);
                RefreshBrushes = true;
                CoverImageBox.Refresh();
            }
        }

        private void oneDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OneDriveIntegrationDialog ODID = new OneDriveIntegrationDialog())
            {
                ODID.ShowDialog();
            }
        }

        private void SpectrumDrawChannel_CheckedChanged(object sender, EventArgs e)
        {
            if (visualisation != null)
            {
                var menuItem = sender as ToolStripMenuItem;
                //visualisation.DrawLeftChannel = leftChannelToolStripMenuItem.Checked;
                //visualisation.DrawRightChannel = rightChannelToolStripMenuItem.Checked;
            }
        }

    }
}
