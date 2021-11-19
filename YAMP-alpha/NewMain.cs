using CSCore;
using CSCore.Streams;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace YAMP_alpha
{
    public partial class NewMain : Form
    {


        YAMP_Core YAMPCore;
        //PeakMeterDialog PeakDialog = new PeakMeterDialog();
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

        private void NewMain_Load(object sender, System.EventArgs e)
        {
            YAMPCore = new YAMP_Core();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            if (YAMPCore.PlayerSource == null)
            {
                using (OpenFileDialog OPD = new OpenFileDialog())
                {
                    if (OPD.ShowDialog() == DialogResult.OK)
                    {
                        YAMPCore.InitializePlayer(OPD.FileName);
                        //DoResize(YAMPCore.TagInfo.Cover);
                        trackBar2.Value = YAMPCore.SoundOutVolume;
                        trackBar1.Maximum = YAMPCore.PlayerLength;

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

        private void trackBar2_Scroll(object sender, System.EventArgs e)
        {
            YAMPCore.SoundOutVolume = trackBar2.Value;
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            if (YAMPCore.PlayerPlaybackState == CSCore.SoundOut.PlaybackState.Playing)
            {
                YAMPCore.Pause();
                PlayTimer.Stop();
            }
        }

        private void PlayTimer_Tick(object sender, System.EventArgs e)
        {
            if (YAMPCore.PlayerPlaybackState == CSCore.SoundOut.PlaybackState.Playing)
            {
                trackBar1.Value = YAMPCore.CurrentPosition;
            }
        }

        private void NewMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (YAMPCore != null)
            {
                YAMPCore.Dispose();
            }
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            if (YAMPCore.PlayerPlaybackState == CSCore.SoundOut.PlaybackState.Playing)
            {
                //YAMPCore.ReleasePlayer();
                trackBar1.Value = 0;
                YAMPCore.ReinitializePlayer();
                //YAMPCore.Player.Initialize(YAMPCore.PlayerSource);
            }
        }

        private void LoadFileStripMenuItem_Click(object sender, System.EventArgs e)
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

        //private void DoResize(Image image)
        //{
        //    int width = image.Width;
        //    int height = image.Height;
        //    //Size sz = new Size(Width, Height);
        //    width = pictureBox1.Width - pictureBox1.Image.Width;
        //    Size = new Size(Width - width, height);
            
        //}

        private void button7_Click(object sender, System.EventArgs e)
        {
            if (trackBar1.Value + 5000 <= trackBar1.Maximum)
            {
                trackBar1.Value = trackBar1.Value + 5000;
            }
        }

        private void button6_Click(object sender, System.EventArgs e)
        {
            if (trackBar1.Value - 5000 >= 0)
            {
                trackBar1.Value = trackBar1.Value-5000;
            }
        }

        private void trackBar1_Scroll(object sender, System.EventArgs e)
        {
            if (YAMPCore.PlayerInitialized)
            {
                YAMPCore.PlayerSource.Position = trackBar1.Value;
            }
        }

        private void pitchShifterToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (PitchShiftDialog PSD = new PitchShiftDialog() { Core = YAMPCore })
            {
                PSD.ShowDialog();
            }
        }

        private void echoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (EchoSignalDialog ESD = new EchoSignalDialog() {Core = YAMPCore })
            {
                ESD.ShowDialog();
            }
        }

        private void peakMtToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //var notificationSource = new SingleBlockNotificationStream(YAMPCore.GetSampleSource(), 15000);
            //notificationSource.SingleBlockStreamAlmostFinished += NotificationSource_SingleBlockStreamAlmostFinished; ;
            //notificationSource.SingleBlockRead += NotificationSource_SingleBlockRead;
            //var PeakMeterSampleSource = notificationSource.ToWaveSource(8).ToSampleSource();
            using (PeakMeterDialog PMD = new PeakMeterDialog() { Core = YAMPCore})
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
    }
}
