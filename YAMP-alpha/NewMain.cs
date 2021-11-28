using CSCore;
using CSCore.Streams;
using System.Drawing;
using System.Windows.Forms;
using System;


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
                        var Duration = TimeConverterFactory.Instance.GetTimeConverterForSource(YAMPCore.PlayerSource).
                            ToTimeSpan(YAMPCore.PlayerSource.WaveFormat, YAMPCore.PlayerSource.Length).TotalSeconds;
                        int durationS = (int)Duration + 1;
                        //DoResize(YAMPCore.TagInfo.Cover);
                        trackBar2.Value = YAMPCore.SoundOutVolume;
                        trackBar1.Maximum = durationS;// YAMPCore.PlayerLength;

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
                YAMPCore.PlayerSource.Position = trackBar1.Value;
            }
        }

        private void pitchShifterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PitchShiftDialog PSD = new PitchShiftDialog(ref YAMPCore))
            {
                PSD.ShowDialog();
            }
        }

        private void echoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (EchoSignalDialog ESD = new EchoSignalDialog(ref YAMPCore))
            {
                ESD.ShowDialog();
            }
        }

        private void peakMtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var notificationSource = new SingleBlockNotificationStream(YAMPCore.GetSampleSource(), 15000);
            //notificationSource.SingleBlockStreamAlmostFinished += NotificationSource_SingleBlockStreamAlmostFinished; ;
            //notificationSource.SingleBlockRead += NotificationSource_SingleBlockRead;
            //var PeakMeterSampleSource = notificationSource.ToWaveSource(8).ToSampleSource();
            using (PeakMeterDialog PMD = new PeakMeterDialog(ref YAMPCore))
            {
                PMD.ShowDialog();
            }
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
            
            GED.ShowDialog();
        }

        private void flangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FlangerEffectDialog FED = new FlangerEffectDialog(ref YAMPCore))
            {
                FED.ShowDialog();
            }
        }

        private void chorusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ChorusEffectDialog CED = new ChorusEffectDialog(ref YAMPCore))
            {
                CED.ShowDialog();
            }
        }

        private void compressorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CompressorEffectDialog CmpED = new CompressorEffectDialog(ref YAMPCore))
            {
                CmpED.ShowDialog();
            }
        }

        private void wavesReverbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (WavesReverbEffectDialog WRED = new WavesReverbEffectDialog(ref YAMPCore))
            {
                WRED.ShowDialog();
            }
        }
    }
}
