using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.DSP;
using CSCore.Streams;
using CSCore.Tags.ID3;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class Form1 : Form
    {
        //Creating a codec factory
        FileInfo PlayingFile;
        CSCore.SoundOut.DirectSoundOut directSound;//= new CSCore.SoundOut.DirectSoundOut() { };
        MMDevice MMDEV = null;
        AudioSessionManager2 AuSeM = null;
        private SimpleAudioVolume ActiveSimpleVolume;
        private AudioSessionControl2 ActiveAudioSession;
        private AudioSessionEnumerator AuSeEn;
        private AudioMeterInformation PeakMeterInfo = null;
        private BasicSpectrumProvider BSP;
        private LineSpectrum LS;
        private BindingList<FileInfo> PlayList = new BindingList<FileInfo>();
        PeakMeter meter;

        public bool dragging { get; private set; }
        public int[] PeakChannelValues { get { return new int[] { Tb_PeakMeterLeft.Value, Tb_PeakMeterRight.Value }; } set { Tb_PeakMeterLeft.Value = value[0]; Tb_PeakMeterRight.Value = value[1]; } }
        private Point StartPoint;
        private bool AlmostEndOfStream;
        private bool EndOfStream;
        Random Random = new Random();
        private Color SpectrumBarColor = Color.Green;

        IWaveSource Player { get; set; } = null;
        public Form1()
        {
            InitializeComponent();
            Lb_PlayList.DataSource = PlayList;
            Lb_PlayList.DisplayMember = "Name";
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

        private void UpdatePlayer(string filename)
        {
            try
            {
                Player = CSCore.Codecs.CodecFactory.Instance.GetCodec(filename);
                ISampleSource PlayerSampleSource = Player.ToSampleSource();
                PlayingFile = new FileInfo(filename);

                BSP = new BasicSpectrumProvider(Player.WaveFormat.Channels, Player.WaveFormat.SampleRate, FftSize.Fft16384);
                LS = new LineSpectrum(BSP.FftSize, BSP)
                {
                    BarCount = 25,
                    BarSpacing = 1,
                    ScaleStrategy = LineSpectrum.ScalingStrategy.Sqrt
                };
                propertyGrid1.SelectedObject = LS;
                var notificationSource = new SingleBlockNotificationStream(PlayerSampleSource, 150000);
                notificationSource.SingleBlockRead += (s, a) => BSP.Add(a.Left, a.Right);
                notificationSource.SingleBlockStreamAlmostFinished += (s, a) => AlmostEndOfStream = true;
                notificationSource.SingleBlockStreamFinished += (s, a) => EndOfStream = true;
                PlayerSampleSource = notificationSource.ToWaveSource(8).ToSampleSource();
                meter = new PeakMeter(PlayerSampleSource)
                {
                    Interval = 25
                };
                meter.PeakCalculated += Meter_PeakCalculated;
                Player = meter.ToWaveSource();

                ID3v2 ID3Data = ID3v2.FromFile(filename);
                TagLib.Tag fileTag = TagLib.File.Create(filename).Tag;
                //
                Image AlbumCover = null;
                if (fileTag.Pictures.Length > 0)
                {
                    var AlbumArts = fileTag.Pictures[0];
                    MemoryStream ms = new MemoryStream(AlbumArts.Data.Data);
                    AlbumCover = Image.FromStream(ms);
                    spectrumBox.BackgroundImage = AlbumCover;
                }
                else
                {
                    spectrumBox.BackgroundImage = null;
                }
                //Id3.Mp3 mp3 = new Id3.Mp3(new FileInfo(filename));
                //Id3.PictureFrame pframe = new Id3.PictureFrame();
                //var AllTags = mp3.GetAllTags();
                //List<Id3.Id3Tag> taglist = new List<Id3.Id3Tag>();
                //foreach (Id3.Id3Tag item in AllTags)
                //{
                //    taglist.Add(item);
                //}


                //pictureBox1.Image = null;
                //Frame f = ID3Data[FrameID.AttachedPicutre];
                //if ((f = ID3Data[FrameID.AttachedPicutre]) != null)
                //{
                //    pictureBox1.Image = (f as PictureFrame).Image;
                //}

                //CSCore.Codecs.MP3.Mp3Frame mp3 = CSCore.Codecs.MP3.Mp3Frame.FromStream(new FileInfo(filename).OpenRead());
                //CSCore.Tags.ID3.Frames.PictureFrame pf = new CSCore.Tags.ID3.Frames.PictureFrame(fhead, ID3Data.Header.Version);
                //if (ID3Data != null)
                //{
                //    var ID3Info = new ID3v2QuickInfo(ID3Data);
                //    if (ID3Info.Image != null)
                //    {
                //        //pictureBox1.Image = ID3Info.Image;// new Bitmap(ID3Info.Image, new Size(200, 200));
                //        spectrumBox.BackgroundImage = ID3Info.Image;
                //    }
                //    else
                //    {
                //        //pictureBox1.Image = LbumRt;// new Bitmap(LbumRt, new Size(200, 200));
                //        LbumRt;// new Bitmap(LbumRt, new Size(200, 200));
                //    }
                //}
                //else
                //{
                //    spectrumBox.BackgroundImage = null;
                //}
                ThreadSafeCall(delegate { trackBar1.Value = 0; trackBar1.Maximum = (int)Player.Length; });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float VolVal = 0.25F;


            ThreadSafeCall(delegate { button2.Text = ">"; directSound.Stop(); });
            if (string.IsNullOrEmpty((string)Lb_PlayList.SelectedValue))
            {
                throw new ArgumentException("Filename cannot be empty", "Filename");
            }
            UpdatePlayer((string)Lb_PlayList.SelectedValue);
            //Task.Run(() => { }).Wait();

            Task.Run(() => { directSound.Initialize(Player); }).Wait();
            //MMDEV = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            //Guid DSGUID = AuSeM.GetType().GUID;

            //MMDEV.Activate(AuSeM.GetType().GUID, CSCore.Win32.CLSCTX.CLSCTX_ALL, IntPtr.Zero);
            VolVal = directSound.Volume * 100F;
            DSVolTracker.Value = Convert.ToInt32(VolVal);

            //MessageBox.Show("Hello");


            //BufferSource buff = new BufferSource(Player, 10000);
            //VolumeSource vs = new VolumeSource(
            //float[] buffer = new float[2048];
            //meter.Read(buffer, 2, 1024);// (buffer, 10, 1024);
            //buff.WriteToStream
            //SampleAggregatorBase samp = new SampleAggregatorBase(Player.ToSampleSource());
            //samp.BaseSource.

            //var Elems = samp.GetRawElements(10000);
            //GetActiveSessionSimpleAudioVolume();
            //UpdateCurrentVolumeInfo();
            //}
            //}

            //foreach (var SessionItem in AuSeM.GetSessionEnumerator())
            //{
            //    SessionItem.StateChanged += SessionItem_StateChanged;
            //}

            //ActiveAudioSession.SimpleVolumeChanged += ActiveAudioSession_SimpleVolumeChanged;
        }

        private void Meter_PeakCalculated(object sender, PeakEventArgs e)
        {
            if (directSound.PlaybackState == CSCore.SoundOut.PlaybackState.Playing)
            {
                trackBar1.Value = (int)Player.Position;
            }
            if (Cb_EnablePeakMeter.Checked)
            {
                var peakval = meter.ChannelPeakValues;
                int[] visiblepeaks = new int[] { (int)(peakval[0] * 100F), (int)(peakval[1] * 100F) };
                ThreadSafeCall(delegate { Tb_PeakMeterLeft.Value = visiblepeaks[0]; Tb_PeakMeterRight.Value = visiblepeaks[1]; });
            }
            if (Cb_RenderSpectrum.Checked)
            {
                GenerateLineSpectrum();
            }

        }


        private void GenerateLineSpectrum()
        {
            if (Cb_BarColorRandom.Checked)
            {
                int max = byte.MaxValue + 1; // 256
                int r = Random.Next(max);
                int g = Random.Next(max);
                int b = Random.Next(max);
                SpectrumBarColor = Color.FromArgb(r, g, b);
            }
            Image image = spectrumBox.Image;
            var newImage = LS.CreateSpectrumLine(spectrumBox.Size, SpectrumBarColor, Color.FromArgb(45, 45, 48), true);
            if (newImage != null)
            {
                spectrumBox.Image = newImage;
                if (image != null)
                {
                    image.Dispose();
                }
            }
        }

        private void InitSessionManager()
        {
            MMDEV = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            AuSeM = AudioSessionManager2.FromMMDevice(MMDEV);
            AuSeEn = AuSeM.GetSessionEnumerator();
            AuSeM.SessionCreated += AuSeM_SessionCreated;
           // PeakMeterInfo = new AudioMeterInformation(MMDEV.Activate(typeof(AudioMeterInformation).GUID, CSCore.Win32.CLSCTX.CLSCTX_ALL, IntPtr.Zero));
            directSound = new CSCore.SoundOut.DirectSoundOut();
            
        }

        private void AuSeM_SessionCreated(object sender, SessionCreatedEventArgs e)
        {
            //throw new NotImplementedException();
            //GetActiveSessionSimpleAudioVolume();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (directSound.WaveSource != null)
            {
                if (directSound.PlaybackState == CSCore.SoundOut.PlaybackState.Paused || directSound.PlaybackState == CSCore.SoundOut.PlaybackState.Stopped)
                {
                    directSound.Play();
                    button2.Text = "| |";
                    playTimer.Start();

                    //backgroundWorker1.RunWorkerAsync();
                    UpdateCurrentVolumeInfo();
                    //Task task = Task.Run(() => { while (true) { UpdatePeakMeter(); } });
                    //task.
                    //UpdatePeakMeter();
                }
                else
                {
                    directSound.Pause();
                    button2.Text = ">";
                    playTimer.Stop();
                    //backgroundWorker1.CancelAsync();
                }
            }
            else
            {
                button1.PerformClick();
            }
            //GetActiveSessionSimpleAudioVolume();
            //UpdateCurrentVolumeInfo();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Lb_PlayList.DisplayMember = "Name";
            Lb_PlayList.ValueMember = "FullName";
            Task.Run(() => { InitSessionManager(); }).Wait();

            //ActiveAudioSession = AuSeM.GetSessionEnumerator().First().QueryInterface<AudioSessionControl2>();
            //ActiveSimpleVolume = ActiveAudioSession.QueryInterface<SimpleAudioVolume>();

            MMDEV.Activate(AuSeM.GetType().GUID, CSCore.Win32.CLSCTX.CLSCTX_ALL, IntPtr.Zero);
            ActiveAudioSession = AuSeM.GetAudioSessionControl(AuSeEn.First().GroupingParam, 65536).QueryInterface<AudioSessionControl2>();
            ActiveSimpleVolume = ActiveAudioSession.QueryInterface<SimpleAudioVolume>();
            //Debug.Assert(AuSeM != null, "AuSem cannot be NULL");
            //Guid SessionGUID = AuSeM.GetType().GUID;
            //MMDEV.Activate(Guid.Empty, CSCore.Win32.CLSCTX.CLSCTX_ALL, IntPtr.Zero);
            //GetActiveSessionSimpleAudioVolume();
            //UpdateCurrentVolumeInfo();
        }

        private void UpdatePeakMeter()
        {
            var peakval = PeakMeterInfo.GetChannelsPeakValues();
            ThreadSafeCall(delegate { Tb_PeakMeterLeft.Value = Convert.ToInt32(peakval[0] * 100F); });
        }

        /// <summary>
        /// It will fetch SimpleAudioVolume object from Session based of guid. Additionally generates a SimpleVolumeChanged event.
        /// </summary>
        //// <param name="SessionGUID">Guid of the session</param>
        private void GetActiveSessionSimpleAudioVolume()
        {
            ActiveAudioSession = AuSeM.GetAudioSessionControl(AuSeM.GetType().GUID, 65536).QueryInterface<AudioSessionControl2>();
            //var ProcID = Process.GetCurrentProcess().Id;
            //AuSeEn = AuSeM.GetSessionEnumerator();
            //foreach (AudioSessionControl ASC in AuSeEn)
            //{
            //    AudioSessionControl2 AuSeC = ASC.QueryInterface<AudioSessionControl2>();
            //    if (AuSeC.Process.Id == ProcID)
            //    {
            //        ActiveAudioSession = AuSeC;
            //        break;
            //    }
            //}

            //ActiveSimpleVolume = ActiveAudioSession.QueryInterface<SimpleAudioVolume>();

            //ActiveSimpleVolume = ActiveAudioSession.QueryInterface<SimpleAudioVolume>();
            //ActiveSimpleVolume = AuSeM.GetSimpleAudioVolume(SessionGUID, true);
            //AuSeM.VolumeDuckNotification += AuSeM_VolumeDuckNotification;
            //ActiveAudioSession.SimpleVolumeChanged += ActiveAudioSession_SimpleVolumeChangedEvent;

            //Debug.Assert(ActiveSimpleVolume != null, ActiveSimpleVolume.ToString() + " Cannot be NULL");

            //AudioSessionNotification sessionNotification = new AudioSessionNotification();
            //sessionNotification.SessionCreated += SessionNotification_SessionCreated; 
        }

        /// <summary>
        /// It will fetch SimpleAudioVolume object from Session based of guid. Additionally generates a SimpleVolumeChanged event.
        /// </summary>
        //// <param name="SessionGUID">Guid of the session</param>
        private void GetActiveSessionSimpleAudioVolume(Guid SessionGUID)
        {
            ActiveAudioSession = AuSeM.GetAudioSessionControl(SessionGUID, 65536).QueryInterface<AudioSessionControl2>();
            GetActiveSessionSimpleAudioVolume();
            //Guid.TryParse("{604E2AE7-C4C5-4032-93FF-88B26CDD75B8}", out Guid IID);

            //var Wisdom = ActiveAudioSession.QueryInterface<IID>();
            //var ProcID = Process.GetCurrentProcess().Id;
            //AuSeEn = AuSeM.GetSessionEnumerator();
            //foreach (AudioSessionControl ASC in AuSeEn)
            //{
            //    AudioSessionControl2 AuSeC = ASC.QueryInterface<AudioSessionControl2>();
            //    if (AuSeC.Process.Id == ProcID)
            //    {
            //        ActiveAudioSession = AuSeC;
            //        break;
            //    }
            //}

            //ActiveSimpleVolume = ActiveAudioSession.QueryInterface<SimpleAudioVolume>();

            //ActiveSimpleVolume = ActiveAudioSession.QueryInterface<SimpleAudioVolume>();
            //ActiveSimpleVolume = AuSeM.GetSimpleAudioVolume(SessionGUID, true);
            //AuSeM.VolumeDuckNotification += AuSeM_VolumeDuckNotification;
            //ActiveAudioSession.SimpleVolumeChanged += ActiveAudioSession_SimpleVolumeChangedEvent;

            //Debug.Assert(ActiveSimpleVolume != null, ActiveSimpleVolume.ToString() + " Cannot be NULL");

            //AudioSessionNotification sessionNotification = new AudioSessionNotification();
            //sessionNotification.SessionCreated += SessionNotification_SessionCreated; 
        }

        private void ActiveAudioSession_SimpleVolumeChangedEvent(object sender, AudioSessionSimpleVolumeChangedEventArgs e)
        {
            ThreadSafeCall(delegate { UpdateCurrentVolumeInfo(); });
        }

        private void UpdateCurrentVolumeInfo()
        {

            voltracker.Value = Convert.ToInt32(ActiveSimpleVolume.MasterVolume * 100F);
            //chkMute.Checked = MuteState;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            directSound.Stop();
            directSound.Dispose();
        }

        private void playTimer_Tick(object sender, EventArgs e)
        {
            if (directSound.PlaybackState == CSCore.SoundOut.PlaybackState.Playing)
            {
                trackBar1.Value = (int)Player.Position;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (directSound != null)
            {
                Player.Position = trackBar1.Value;
            }
        }

        private void ActiveAudioSession_SimpleVolumeChanged(object sender, AudioSessionSimpleVolumeChangedEventArgs e)
        {
            MessageBox.Show("Event Triggered");
        }


        private void chkMute_CheckedChanged(object sender, EventArgs e)
        {
            voltracker.Enabled = !chkMute.Checked;
            ActiveSimpleVolume.IsMuted = chkMute.Checked;
        }

        private void voltracker_ValueChanged(object sender, EventArgs e)
        {
            ActiveSimpleVolume.MasterVolume = voltracker.Value / 100f;
        }

        private void voltracker_Scroll(object sender, EventArgs e)
        {
            ActiveSimpleVolume.MasterVolume = voltracker.Value / 100f;
            UpdateText();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

            directSound.Volume = DSVolTracker.Value / 100f;
            UpdateText();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            int val = voltracker.Value;
            voltracker.Value = DSVolTracker.Value;
            DSVolTracker.Value = val;
        }

        private void DSVolTracker_ValueChanged(object sender, EventArgs e)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            textBox1.Text = "Volume: " + ActiveSimpleVolume.MasterVolume.ToString("F2") + Environment.NewLine + "DirectSound: " + directSound.Volume.ToString("F2");
        }

        //private void button5_Click(object sender, EventArgs e)
        //{
        //    UpdatePeakMeter();
        //}

        //private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    BackgroundWorker worker = (BackgroundWorker)sender;
        //    while (!worker.CancellationPending)
        //    {
        //        var peakval = meter.ChannelPeakValues;// PeakMeterInfo.GetChannelsPeakValues();
        //        int[] visiblepeaks = new int[] { Convert.ToInt32(peakval[0] * 100F), Convert.ToInt32(peakval[1] * 100F) };
        //        //int LeftPeak = Convert.ToInt32(peakval[0] * 100F);
        //        //int
        //        ThreadSafeCall(delegate { Tb_PeakMeterLeft.Value = visiblepeaks[0]; Tb_PeakMeterRight.Value = visiblepeaks[1]; });
        //    }
        //}

        private void Cb_EnablePeakMeter_CheckedChanged(object sender, EventArgs e)
        {
            if (Cb_EnablePeakMeter.Checked)
            {
                //backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                PeakChannelValues = new int[] { 0, 0 };
            }
        }



        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            StartPoint = e.Location;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - StartPoint.X, p.Y - StartPoint.Y);
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog() { IsFolderPicker = true };
            OpenFileDialog OFD = new OpenFileDialog() { CheckFileExists = false, FileName = "Select.Folder" };
            //CommonOpenFileDialog fileDialog = new CommonOpenFileDialog() { IsFolderPicker = true };
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //var DirPath = new FileInfo(fileDialog.FileName).DirectoryName;
                PlayList.Clear();
                DirectoryInfo Dir = new DirectoryInfo(fileDialog.FileName);
                var SongEnum = Dir.EnumerateFileSystemInfos().Where(x => x is FileInfo);
                foreach (FileInfo song in SongEnum)
                {
                    if (song.Extension == ".mp3")
                    {
                        PlayList.Add(song);
                    }
                }

            }
            //Lb_PlayList.DisplayMember = PlayList.FirstOrDefault().Name;
        }

        private void Lb_PlayList_DoubleClick(object sender, EventArgs e)
        {
            button1.PerformClick();
        }

        private void spectrumBox_DoubleClick(object sender, EventArgs e)
        {
            PictureBox PicSpecBox = sender as PictureBox;
            if (PicSpecBox.BackgroundImage != null)
            {
                using (BigArt BG = new BigArt())
                {
                    BG.AlbumArt = PicSpecBox.BackgroundImage;
                    BG.ShowDialog();
                }
            }
        }

        private void Cb_RenderSpectrum_CheckedChanged(object sender, EventArgs e)
        {
            if (!Cb_RenderSpectrum.Checked)
            {
                spectrumBox.Image = null;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                SpectrumBarColor = colorDialog1.Color;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SelectNextTrack();
            button1.PerformClick();
            button2.PerformClick();
        }

        private void SelectNextTrack()
        {
            if (Lb_PlayList.SelectedIndex >= 0 && Lb_PlayList.SelectedIndex < Lb_PlayList.Items.Count - 1)
            {
                Lb_PlayList.SelectedIndex = Lb_PlayList.SelectedIndex + 1;
            }
        }

        private void SelectPreviousTrack()
        {
            if (Lb_PlayList.SelectedIndex >= 0 && Lb_PlayList.SelectedIndex < Lb_PlayList.Items.Count - 1)
            {
                Lb_PlayList.SelectedIndex = Lb_PlayList.SelectedIndex - 1;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SelectPreviousTrack();
            button1.PerformClick();
            button2.PerformClick();
        }
    }
}
