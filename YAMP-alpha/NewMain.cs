using CSCore;
using CSCore.Streams;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace YAMP_alpha
{
    public partial class NewMain : Form
    {
        //YAMP_Core YAMPCore;
        public NewMain()
        {
            InitializeComponent();
            int ClientTop = RectangleToScreen(ClientRectangle).Top;
            int height = Height - pictureBox1.Height;
            MinimumSize = new Size(387, height);
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.PlayerSource == null)
            {
                using (OpenFileDialog OPD = new OpenFileDialog())
                {
                    if (OPD.ShowDialog() == DialogResult.OK)
                    {
                        YAMPVars.CORE.InitializePlayer(OPD.FileName);
                        var Dur = Extensions.GetLength(YAMPVars.CORE.PlayerSource).TotalSeconds;
                        int durationS = (int)Dur + 1;
                        trackBar2.Value = YAMPVars.CORE.SoundOutVolume;
                        trackBar1.Maximum = durationS;
                        pictureBox1.BackgroundImage = YAMPVars.CORE.TagInfo.Cover;
                    }
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
            YAMPVars.CORE.SoundOutVolume = trackBar2.Value;
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
                trackBar1.Value = (Duration.Minutes * 60) + Duration.Seconds;
                waveformPainter1.AddMax(YAMPVars.CORE.WaveFormLEFT);
            }
            else if(YAMPVars.CORE.PlayerPaused)
            {
                PlayTimer.Stop();
            }
            else
            {
                trackBar1.Value = 0;
                YAMPVars.CORE.PlayerSource.Position = 0;
                YAMPVars.CORE.Player.Stop();
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
                trackBar1.Value = 0;
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

                    trackBar2.Value = YAMPVars.CORE.SoundOutVolume;
                    trackBar1.Maximum = YAMPVars.CORE.PlayerLength;
                }
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value + 5000 <= trackBar1.Maximum)
            {
                trackBar1.Value = trackBar1.Value + 5000;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value - 5000 >= 0)
            {
                trackBar1.Value = trackBar1.Value - 5000;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.PlayerInitialized)
            {
                TimeSpan ts = TimeSpan.FromSeconds(trackBar1.Value);
                Extensions.SetPosition(YAMPVars.CORE.PlayerSource, ts);
            }
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
    }
}
