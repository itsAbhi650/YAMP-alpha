using CSCore;
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
        private bool PlayNext;
        private string _curlyrln;
        private event EventHandler LyricLineChanged;
        private string CurrentLyricLine
        {
            get { return _curlyrln; }
            set
            {
                if (_curlyrln != value)
                {
                    _curlyrln = value;
                    LyricLineChanged?.Invoke(value, EventArgs.Empty);
                }
            }
        }

        public NewMain()
        {
            InitializeComponent();
            int ClientTop = RectangleToScreen(ClientRectangle).Top;
            int height = Height - CoverImageBox.Height;
            MinimumSize = new Size(400, height);
        }

        private void NotificationSource_BlockRead(object sender, EventArgs e)
        {
            if (PanelMode == YAMPEnums.PanelMode.Lyrics)
            {
                var TotalSeconds = Extensions.GetPosition(YAMPVars.CORE.PlayerSource).TotalSeconds;
                string LyricLine = YAMPVars.CORE.CurrentTrack.Lyrics?.LastOrDefault(x => x.Key < TotalSeconds).Value;
                CurrentLyricLine = LyricLine;
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

        private void PlayFromStart(bool FadeTrack = true)
        {
            DurationTracker.Value = 0;
            YAMPVars.CORE.Play();
            PlayTimer.Start();
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
            YAMPVars.CORE = new YAMP_Core
            {
                UIRef = this
            };
            YAMPVars.CORE.TrackChanged += CORE_TrackChanged;
            YAMPVars.CORE.Player.Stopped += Player_Stopped;
        }

        private void Player_Stopped(object sender, CSCore.SoundOut.PlaybackStoppedEventArgs e)
        {
            YAMPVars.CORE.PlayerStopped = true;
            if (YAMPVars.PLTRACKFLAG)
            {
                YAMPVars.PLTRACKFLAG = false;
                YAMPVars.CORE.PlayerStopped = false;
                PlayFromStart();

            }
        }

        internal void PlayfromPlaylist(TrackInfo Track)
        {
            if (PlayTimer.Enabled)
            {
                PlayTimer.Stop();
            }
            YAMPVars.CORE.LoadTrackInfo(Track);
            YAMPVars.PLTRACKFLAG = true;
            PlayFromStart();
        }

        private void CORE_TrackChanged(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.CurrentTrack != null && YAMPVars.CORE.PlayerSource != null)
            {
                UpdateTrackers();
                Image cover = YAMPVars.CORE.GetTrackCover();
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
            var Lines = File.ReadAllLines(path);
            YAMPVars.TrackList[0].Lyrics = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<double, string>>();
            foreach (var line in Lines)
            {

                int TimeStampEnd = line.IndexOf(']');
                LyricsHelper.TryParseLrcString(line, 1, TimeStampEnd - 1, out TimeSpan res);
                YAMPVars.TrackList[0].Lyrics.Add(new System.Collections.Generic.KeyValuePair<double, string>(res.TotalSeconds, line.Substring(TimeStampEnd + 1)));
            }
        }

        private void ResizePlayer(Image cover = null)
        {
            if (cover != null)
            {
                int Border = Width - ClientRectangle.Width;
                double dImageAR = cover.Width / cover.Height;
                int _width = ClientRectangle.Width;
                int _height = ClientRectangle.Height;
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
                Size newSize = new Size((int)dImgWidth + Border, (int)dImgHeight + GetAdditionalPlayerHeight());
                Size = new Size((int)dImgWidth + Border, (int)dImgHeight + GetAdditionalPlayerHeight());
                //GetClientRect(r);
                //int width = r.Width();
                //int height = r.Height();
                //if (!m_bCustomImgLoaded)
                //{
                //    // Limit logo size
                //    // TODO: Use vector logo to preserve quality and remove limit.
                //    width = std::min(img.GetWidth(), width);
                //    height = std::min(img.GetHeight(), height);
                //}

                //int height = (int)Math.Floor(cover.Height * percent);
                //if (height<MinimumSize.Height)
                //{
                //    height = (int)(MinimumSize.Height * 1.25F);
                //}
                //int width = (int)Math.Floor(cover.Width * percent);
                //if (width<MinimumSize.Width)
                //{
                //    width = (int)(MinimumSize.Width * 1.25F);
                //}
                //int coverwidthdiff = width - CoverImageBox.ClientSize.Width;
                //width = (coverwidthdiff > 0 ? width + (coverwidthdiff) : coverwidthdiff < 0 ? width - (coverwidthdiff) : width);
                ////width = (int)MinimumSize.
                //Size newSize = new Size(width + Border, height + AdditionalPlayerHeight);
                //Size diffSize = newSize.GetDifference(MinimumSize, out SizeExtensions.Difference diff);
                //switch (diff)
                //{   
                //    case SizeExtensions.Difference.Equal:
                //    case SizeExtensions.Difference.Bigger:
                //        Size = newSize;
                //        break;
                //    case SizeExtensions.Difference.Smaller:
                //        width = (int)Math.Floor(cover.Width * percent)+100;
                //        coverwidthdiff = width - CoverImageBox.ClientSize.Width;
                //        width = (coverwidthdiff > 0 ? width + (coverwidthdiff) : coverwidthdiff < 0 ? width - (coverwidthdiff) : width) + Border;
                //        height = (width - Border) + AdditionalPlayerHeight;
                //        newSize = new Size(width, height);
                //        Size = newSize;
                //        break;
                //    default:
                //        break;
                //}
                //if (Width > width)
                //{
                //    Width = Width - (Width - width);
                //}
                //else
                //{
                //    Image img = CoverImageBox.BackgroundImage;
                //    Width = width + Border;
                //    //if (CoverImageBox.ClientRectangle.Width < Width)
                //    //{
                //    //    Width -= Width - CoverImageBox.ClientRectangle.Width;
                //    //}
                //}
                //if (CoverImageBox.ClientRectangle.Height<CoverImageBox.Height)
                //{
                //    Height -= CoverImageBox.Height - CoverImageBox.ClientRectangle.Height;
                //}
                //Width = (int)(cover.Width * percent) + (Width - ClientRectangle.Width);//) - (Width - CoverImageBox.Width);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.PlayerSource == null)
            {
                bool TrackLoaded = YAMPVars.CORE.GetFirstTrack();
                if (!TrackLoaded)
                {
                    using (OpenFileDialog OPD = new OpenFileDialog() { Filter = "mp3 files (*.mp3)|*.mp3|m4a files (*.m4a)|*.m4a" })
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
                    YAMPVars.CORE.PlayerStopped = false;
                    YAMPVars.CORE.PlayerPaused = false;
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
                YAMPVars.CORE.PlayerPaused = true;
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
            if (!YAMPVars.CORE.PlayerStopped)
            {
                if (!YAMPVars.CORE.NetPlay || YAMPVars.CORE.PlayerSource.CanSeek)
                {
                    Lbl_Duration.Text = TrackDurationText();
                }
                waveformPainter1.AddMax(YAMPVars.CORE.WaveFormLEFT);
            }
            else if (YAMPVars.CORE.PlayerPaused)
            {
                PlayTimer.Stop();
            }
            else
            {
                YAMPVars.CORE.Stop();
                PlayTimer.Stop();
                if (YAMPVars.CORE.PlayNextTrackDirected(YAMPVars.CORE.NextTrackDirection) && PlayNext)
                {
                    PlayFromStart();
                    PlayNext = false;
                }
            }
        }

        private void NewMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (YAMPVars.CORE != null)
            {
                YAMPVars.CORE.Player.Stopped -= Player_Stopped;
                YAMPVars.CORE.Player.Stop();
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
                YAMPVars.CORE.Player.Stop();
            }
        }

        private void LoadFileStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OPD = new OpenFileDialog() { Filter = CSCore.Codecs.CodecFactory.SupportedFilesFilterEn })
            {
                if (OPD.ShowDialog() == DialogResult.OK)
                {
                    YAMPVars.CORE.ReleasePlayer();
                    TrackInfo Track = new TrackInfo(OPD.FileName);
                    YAMPVars.CORE.InitializePlayer(Track.Path);
                    YAMPVars.CORE.CurrentTrack = Track;
                    VolumeTracker.Value = YAMPVars.CORE.SoundOutVolume;
                    YAMPVars.TrackList.Add(YAMPVars.CORE.CurrentTrack);
                    if (YAMPVars.CORE.CurrentTrack.Covers.Count > 0)
                    {
                        CoverImageBox.BackgroundImage = YAMPVars.CORE.CurrentTrack.Covers[0];
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (DurationTracker.Value + 5000 <= DurationTracker.Maximum)
            {
                DurationTracker.Value = DurationTracker.Value + 5000;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (DurationTracker.Value - 5000 >= 0)
            {
                DurationTracker.Value = DurationTracker.Value - 5000;
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
            playlist.Show();
        }

        private void Btns_TrackShift_Click(object sender, EventArgs e)
        {
            if (!YAMPVars.CORE.PlayerStopped)
            {
                YAMPVars.CORE.NextTrackDirection = int.Parse(((Button)sender).Tag.ToString());
                int DestIndex = 0;
                if (YAMPVars.CORE.isValidMove(YAMPVars.CORE.NextTrackDirection, out DestIndex))
                {
                    DurationTracker.Value = 0;
                    YAMPVars.CORE.PlayerSource.Position = 0;
                    YAMPVars.CORE.Player.Stop();
                    PlayNext = true;
                }
            }
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
            YAMPlaylistDialog.LoadDirectory();
            YAMPVars.CORE.GetFirstTrack();
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

        private void filtersToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
            PanelMode = Enum.IsDefined(typeof(YAMPEnums.PanelMode), (int)PanelMode + 1) ? (YAMPEnums.PanelMode)((int)PanelMode + 1) : 0;
            UpdatePanel(PanelMode);
        }

        private void UpdatePanel(YAMPEnums.PanelMode Mode)
        {
            switch (Mode)
            {
                case YAMPEnums.PanelMode.Cover:
                    YAMPVars.NotificationSource.BlockRead -= NotificationSource_BlockRead;
                    visualizer.Stop();
                    visualisation = null;
                    YAMPVars.SingleBlockNotificationStream.SingleBlockRead -= SingleBlockNotificationStream_SingleBlockRead;
                    CoverImageBox.Paint -= CoverImageBox_Paint;
                    CoverImageBox.BackgroundImage = YAMPVars.CORE.CurrentTrack.Covers[0];
                    break;
                case YAMPEnums.PanelMode.Spectrum:
                    YAMPVars.NotificationSource.BlockRead -= NotificationSource_BlockRead;
                    visualisation = new GraphVisualization();
                    YAMPVars.SingleBlockNotificationStream.SingleBlockRead += SingleBlockNotificationStream_SingleBlockRead;
                    CoverImageBox.Paint -= CoverImageBox_Paint;
                    CoverImageBox.BackgroundImage = null;
                    visualizer.Start();
                    break;
                case YAMPEnums.PanelMode.Lyrics:
                    visualizer.Stop();
                    visualisation = null;
                    YAMPVars.SingleBlockNotificationStream.SingleBlockRead -= SingleBlockNotificationStream_SingleBlockRead;
                    CoverImageBox.BackgroundImage = null;
                    CoverImageBox.Paint += CoverImageBox_Paint;
                    LyricLineChanged += NewMain_LyricLineChanged;
                    YAMPVars.NotificationSource.BlockRead += NotificationSource_BlockRead;
                    break;
                default:
                    break;
            }
        }

        private void NewMain_LyricLineChanged(object sender, EventArgs e)
        {
            CoverImageBox.Refresh();
        }

        private void CoverImageBox_Paint(object sender, PaintEventArgs e)
        {
            if (PanelMode == YAMPEnums.PanelMode.Lyrics)
            {
                e.Graphics.DrawString(CurrentLyricLine, LyricsHelper.LyricsFont, LyricsHelper.LyricsWriterBrush, new Rectangle(0, 0, CoverImageBox.Width, CoverImageBox.Height), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
        }

        private void SingleBlockNotificationStream_SingleBlockRead(object sender, CSCore.Streams.SingleBlockReadEventArgs e)
        {
            visualisation.AddSamples(e.Left, e.Right);
        }

        private void visualizer_Tick(object sender, EventArgs e)
        {
            var image = CoverImageBox.BackgroundImage;
            CoverImageBox.BackgroundImage = visualisation.Draw(CoverImageBox.Width, CoverImageBox.Height);
            if (image != null)
            {
                image.Dispose();
            }
        }

        private void NewMain_SizeChanged(object sender, EventArgs e)
        {
            Text = CoverImageBox.ClientSize.ToString();
        }

        private void lyricsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog())
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    ParseLRC(OFD.FileName);
                }
            }
        }

        private void lyricsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (LyricsConfig LCDiag = new LyricsConfig())
            {
                LCDiag.ShowDialog();
                CoverImageBox.Refresh();
            }
        }
    }
}
