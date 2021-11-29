using CSCore;
using CSCore.Streams;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace YAMP_alpha
{
    public partial class NewMain : Form
    {
        YAMP_Core YAMPCore;
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
            YAMPCore = new YAMP_Core();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (YAMPCore.PlayerSource == null)
            {
                using (OpenFileDialog OPD = new OpenFileDialog())
                {
                    if (OPD.ShowDialog() == DialogResult.OK)
                    {
                        YAMPCore.InitializePlayer(OPD.FileName);
                        var Dur = Extensions.GetLength(YAMPCore.PlayerSource).TotalSeconds;
                        int durationS = (int)Dur + 1;
                        trackBar2.Value = YAMPCore.SoundOutVolume;
                        trackBar1.Maximum = durationS;

                        pictureBox1.BackgroundImage = YAMPCore.TagInfo.Cover;
                    }
                }
            }
            else
            {
                if (YAMPCore.PlayerPlaybackState != CSCore.SoundOut.PlaybackState.Playing)
                {
                    YAMPCore.Play();
                    PlayTimer.Start();
                }
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            YAMPCore.SoundOutVolume = trackBar2.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (YAMPCore.PlayerPlaybackState == CSCore.SoundOut.PlaybackState.Playing)
            {
                YAMPCore.Pause();
                PlayTimer.Stop();
            }
        }

        private void PlayTimer_Tick(object sender, EventArgs e)
        {
            if (YAMPCore.PlayerPlaybackState == CSCore.SoundOut.PlaybackState.Playing)
            {
                TimeSpan Duration = Extensions.GetPosition(YAMPCore.PlayerSource);
                trackBar1.Value = (Duration.Minutes * 60) + Duration.Seconds;

            }
        }

        private void NewMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (YAMPCore != null)
            {
                YAMPCore.Dispose();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (YAMPCore.PlayerPlaybackState == CSCore.SoundOut.PlaybackState.Playing)
            {
                trackBar1.Value = 0;
                YAMPCore.ReinitializePlayer();
            }
        }

        private void LoadFileStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OPD = new OpenFileDialog())
            {
                if (OPD.ShowDialog() == DialogResult.OK)
                {
                    YAMPCore.ReleasePlayer();
                    YAMPCore.InitializePlayer(OPD.FileName);

                    trackBar2.Value = YAMPCore.SoundOutVolume;
                    trackBar1.Maximum = YAMPCore.PlayerLength;
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
            if (YAMPCore.PlayerInitialized)
            {
                TimeSpan ts = TimeSpan.FromSeconds(trackBar1.Value);
                Extensions.SetPosition(YAMPCore.PlayerSource, ts);
            }
        }

        private void pitchShifterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PitchShiftDialog PSD = new PitchShiftDialog(ref YAMPCore);
            PSD.Show();
        }

        private void echoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EchoSignalDialog ESD = new EchoSignalDialog(ref YAMPCore);
            ESD.Show();

        }

        private void peakMtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var notificationSource = new SingleBlockNotificationStream(YAMPCore.GetSampleSource(), 15000);
            //notificationSource.SingleBlockStreamAlmostFinished += NotificationSource_SingleBlockStreamAlmostFinished; ;
            //notificationSource.SingleBlockRead += NotificationSource_SingleBlockRead;
            //var PeakMeterSampleSource = notificationSource.ToWaveSource(8).ToSampleSource();
            PeakMeterDialog PMD = new PeakMeterDialog(ref YAMPCore);
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
            GargleEffectDialog GED = new GargleEffectDialog(ref YAMPCore);
            GED.Show();
        }

        private void flangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlangerEffectDialog FED = new FlangerEffectDialog(ref YAMPCore);
            FED.Show();
        }

        private void chorusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChorusEffectDialog CED = new ChorusEffectDialog(ref YAMPCore);
            CED.Show();
        }

        private void compressorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompressorEffectDialog CmpED = new CompressorEffectDialog(ref YAMPCore);
            CmpED.Show();
        }

        private void wavesReverbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WavesReverbEffectDialog WRED = new WavesReverbEffectDialog(ref YAMPCore);
            WRED.Show();
        }
    }
}
