using CSCore;
using CSCore.Streams;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace YAMP_alpha
{
    public partial class NewMain : Form
    {
        private bool PlayNext;
        int playdirection;
        private int PlayNextDirection;

        public NewMain()
        {
            InitializeComponent();
            int ClientTop = RectangleToScreen(ClientRectangle).Top;
            int height = Height - pictureBox1.Height;
            MinimumSize = new Size(387, height);
        }

        private void SetTrackBar()
        {
            DurationTracker.Value = 0;
            var Dur = Extensions.GetLength(YAMPVars.CORE.PlayerSource).TotalSeconds;
            int durationS = (int)Dur + 1;
            DurationTracker.Maximum = durationS;
        }

        private void PlayFromStart()
        {
            DurationTracker.Value = 0;
            pictureBox1.BackgroundImage = YAMPVars.CORE.GetTrackCover();
            YAMPVars.CORE.Play();
            PlayTimer.Start();
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
                YAMPVars.PLTRACKFLAG = false; //Resetting flag to prevent unwamted call to play.
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
            if (YAMPVars.CORE.CurrentTrack != null)
            {
                SetTrackBar();
                Lbl_PlayerLabel.Text = string.Format(">  {0}", YAMPVars.CORE.CurrentTrack.Title);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.PlayerSource == null)
            {
                bool TrackLoaded = YAMPVars.CORE.GetFirstTrack();
                if (!TrackLoaded)
                {
                    using (OpenFileDialog OPD = new OpenFileDialog())
                    {
                        if (OPD.ShowDialog() == DialogResult.OK)
                        {
                            TrackInfo track = new TrackInfo(OPD.FileName);
                            YAMPVars.TrackList.Add(track);
                            TrackLoaded = YAMPVars.CORE.GetFirstTrack();

                        }
                    }
                }

                if (TrackLoaded)
                {
                    pictureBox1.BackgroundImage = YAMPVars.CORE.CurrentTrack.Covers[0];
                    VolumeTracker.Value = YAMPVars.CORE.SoundOutVolume;
                    YAMPVars.CORE.Play();
                    PlayTimer.Start();
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
            YAMPVars.CORE.SoundOutVolume = VolumeTracker.Value;
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

        private void PlayTimer_Tick(object sender, EventArgs e)
        {
            if (!YAMPVars.CORE.PlayerStopped)
            {
                TimeSpan Duration = Extensions.GetPosition(YAMPVars.CORE.PlayerSource);
                DurationTracker.Value = (Duration.Minutes * 60) + Duration.Seconds;
                Lbl_Duration.Text = string.Format("{0}\\{1}", Duration.ToString(@"mm\:ss"), YAMPVars.CORE.CurrentTrack.Duration);
                waveformPainter1.AddMax(YAMPVars.CORE.WaveFormLEFT);
            }
            else if (YAMPVars.CORE.PlayerPaused)
            {
                PlayTimer.Stop();
            }
            else
            {
                PlayTimer.Stop();
                YAMPVars.CORE.Stop();
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
                YAMPVars.CORE.Dispose();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!YAMPVars.CORE.PlayerStopped)
            {
                YAMPVars.CORE.StopRequested = true;
                DurationTracker.Value = 0;
                YAMPVars.CORE.PlayerSource.Position = 0;
                YAMPVars.CORE.Player.Stop();
            }
        }

        private void LoadFileStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OPD = new OpenFileDialog())
            {
                if (OPD.ShowDialog() == DialogResult.OK)
                {
                    YAMPVars.CORE.ReleasePlayer();
                    YAMPVars.CORE.InitializePlayer(OPD.FileName);

                    VolumeTracker.Value = YAMPVars.CORE.SoundOutVolume;
                    DurationTracker.Maximum = YAMPVars.CORE.PlayerLength;
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
            //var notificationSource = new SingleBlockNotificationStream(YAMPCore.GetSampleSource(), 15000);
            //notificationSource.SingleBlockStreamAlmostFinished += NotificationSource_SingleBlockStreamAlmostFinished; ;
            //notificationSource.SingleBlockRead += NotificationSource_SingleBlockRead;
            //var PeakMeterSampleSource = notificationSource.ToWaveSource(8).ToSampleSource();
            PeakMeterDialog PMD = new PeakMeterDialog();
            PMD.Show();

        }

        private void NotificationSource_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        private void NotificationSource_SingleBlockStreamAlmostFinished(object sender, SingleBlockStreamAlmostFinishedEventArgs e)
        {
            //throw new System.NotImplementedException();
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

        private void changeSampleRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SampleConverterDialog SCD = new SampleConverterDialog(YAMPCore.PlayerSource);
            //SCD.Show();
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
                if (YAMPVars.CORE.isValidMove(YAMPVars.CORE.NextTrackDirection, out int DestIndex))
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
                //DurationTracker.Value = DurationTracker.Maximum;
                DurationTracker.Value += SecToSkip;
            }
            YAMPVars.CORE.AdjustPlayerPosition(DurationTracker.Value);
        }

        private void distortionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DistortionEffectDialog distortionEffect = new DistortionEffectDialog();
            distortionEffect.Show();
        }
    }
}
