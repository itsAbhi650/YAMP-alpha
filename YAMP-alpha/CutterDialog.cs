using CSCore;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class CutterDialog : Form
    {
        IWaveSource source;
        private string AudioType;
        private string progAlert
        {
            get { return _progAlert; }
            set
            {
                _progAlert = value;
                OnAlertUpdate();
            }
        }

        long CutLength;
        private long CutEnd;
        private long CutBegin;
        private string _progAlert;

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

        public CutterDialog()
        {
            InitializeComponent();
            AlertUpdate += CutterDialog_AlertUpdate;
        }

        private void CutterDialog_AlertUpdate(object sender, EventArgs e)
        {
            ThreadSafeCall(delegate { StatusLabel.Text = progAlert; });
        }

        private event EventHandler AlertUpdate;
        private void OnAlertUpdate()
        {
            AlertUpdate?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Cut a part from an audio source and write in a stream.
        /// </summary>
        /// <param name="Source">Audio on which cut will run.</param>
        /// <param name="CutFrom">Position in raw bytes from where to begin the cut.</param>
        /// <param name="CutTo">Position in raw bytes till where to cut.</param>
        /// <param name="stream">Stream object to store the cut audio.</param>
        /// <param name="p">Report progess of the cut.</param>
        public Stream GetCutStream(IWaveSource Source, long CutFrom, long CutTo, IProgress<int> p = null)
        {
            //IReadableAudioSource<float> aad = source as IReadableAudioSource<float>;
            Stream CutStream = new MemoryStream();
            Source.Position = CutFrom;
            byte[] buffer = new byte[Source.WaveFormat.BytesPerSecond];
            while (buffer.Length != 0)
            {
                int count = source.Read(buffer, 0, buffer.Length);
                if (count <= 0)
                {
                    return CutStream;
                }
                CutStream.Write(buffer, 0, count);
                if (p != null)
                {
                    float perc = (float)(source.Position / (double)CutEnd);
                    p.Report((int)Math.Floor(perc * 100));
                }
                if (CutTo - Source.Position < buffer.Length)
                {
                    buffer = new byte[CutTo - Source.Position];
                }
            }
            CutStream.Position = 0;
            return CutStream;
        }

        public IWaveSource CutSource(long CutFrom, long CutTo, IProgress<int> p = null)
        {
            WriteableBufferingSource _destsource = new WriteableBufferingSource(source.WaveFormat, source.WaveFormat.BytesPerSecond * (TrackSeekBar.Right - TrackSeekBar.Left));
            byte[] buffer = new byte[source.WaveFormat.BytesPerSecond];
            while (buffer.Length != 0)
            {
                int count = source.Read(buffer, 0, buffer.Length);
                if (count <= 0)
                {
                    return null;
                }
                _destsource.Write(buffer, 0, count);
                if (p != null)
                {
                    float perc = (float)(source.Position / (double)CutTo);
                    p.Report((int)Math.Floor(perc * 100));
                }
                if (CutTo - source.Position < source.WaveFormat.BytesPerSecond)
                {
                    buffer = new byte[CutTo - source.Position];
                }
            }
            return _destsource;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog() { Filter = "mp3 files (*.mp3)|*.mp3|m4a files (*.m4a)|*.m4a|wav files (*.wav)|*.wav" };
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                await Encoder.TrackCutAsync(TrackPathBox.Text, SFD.FileName, TrackSeekBar.ValueLeft, TrackSeekBar.ValueRight, Cb_RetainTags.Checked, new Progress<int>(per => { StatusProgress.Value = per; }));
            }
        }

        private void doubleTrackBar1_LeftValueChanged(object sender, EventArgs e)
        {
            if (source != null)
            {
                label3.Text = TimeSpan.FromSeconds(TrackSeekBar.ValueLeft).ToString(@"mm\:ss");
            }
        }

        private void doubleTrackBar1_RightValueChanged(object sender, EventArgs e)
        {
            if (source != null)
            {
                //  CutEnd = Extensions.GetRawElements(source, new TimeSpan(0, 0, doubleTrackBar1.ValueRight));
                label4.Text = TimeSpan.FromSeconds(TrackSeekBar.ValueRight).ToString(@"mm\:ss");
            }
        }

        private void BtnLoadAudio_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog()
            {
                Filter = "mp3 files (*.mp3)|*.mp3|m4a files (*.m4a)|*.m4a|wav files (*.wav)|*.wav"
            })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    TrackPathBox.Text = OFD.FileName;
                    source = CSCore.Codecs.CodecFactory.Instance.GetCodec(OFD.FileName);
                    TrackSeekBar.Maximum = (int)Extensions.GetLength(source).TotalSeconds;
                    label3.Text = TimeSpan.FromSeconds(TrackSeekBar.ValueLeft).ToString(@"mm\:ss");
                    label4.Text = TimeSpan.FromSeconds(TrackSeekBar.ValueRight).ToString(@"mm\:ss");
                }
            }
        }

        private void AudioInfo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TrackPathBox.Text))
            {
                MediaInfoDialog MID = new MediaInfoDialog(TrackPathBox.Text);
                MID.ShowDialog();
            }
        }

    }
}
