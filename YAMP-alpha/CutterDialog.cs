using CSCore;
using CSCore.Codecs.WAV;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class CutterDialog : Form
    {
        CSCore.IWaveSource source;
        long CutLength;
        private long CutEnd;
        private long CutBegin;

        public CutterDialog()
        {
            InitializeComponent();
            //var Something = YAMPVars.CORE.PlayerSource.WaveFormat.Clone();
        }

        /// <summary>
        /// Cut a part from an audio source and write in a stream.
        /// </summary>
        /// <param name="Source">Audio on which cut will run.</param>
        /// <param name="CutFrom">Position in raw bytes from where to begin the cut.</param>
        /// <param name="CutTo">Position in raw bytes till where to cut.</param>
        /// <param name="stream">Stream object to store the cut audio.</param>
        /// <param name="p">Report progess of the cut.</param>
        public Stream GetCutStream(IWaveSource Source, long CutFrom, long CutTo, IProgress<int> p)
        {
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
                float perc = (float)(source.Position / (double)CutEnd);
                p.Report((int)Math.Floor(perc * 100));
                if (CutTo - Source.Position < Source.WaveFormat.BytesPerSecond)
                {
                    buffer = new byte[CutTo - Source.Position];
                }
            }
            CutStream.Position = 0;
            return CutStream;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog() { Filter = "mp3 files (*.mp3)|*.mp3|m4a files (*.m4a)|*.m4a" };
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                Stream CutStream = null;// new MemoryStream();// File.OpenWrite(SFD.FileName);
                progressBar1.Value = 0;
                byte[] buffer = new byte[source.WaveFormat.BytesPerSecond];
                Progress<int> progress = new Progress<int>(per => { progressBar1.Value = per; });
                await Task.Run(() => CutStream = GetCutStream(source, CutBegin, CutEnd, progress));
                EncodeStreamM4A(source.WaveFormat, CutStream, File.OpenWrite(SFD.FileName));
                CutStream.Flush();
                CutStream.Dispose();
                MessageBox.Show("audio cut success!");
            }
        }

        private void doubleTrackBar1_LeftValueChanged(object sender, EventArgs e)
        {
            if (source != null)
            {
                CutBegin = Extensions.GetRawElements(source, new TimeSpan(0, 0, doubleTrackBar1.ValueLeft));
                CutLength = CutEnd - CutBegin;
                label3.Text = doubleTrackBar1.ValueLeft.ToString();
            }
        }

        private void doubleTrackBar1_RightValueChanged(object sender, EventArgs e)
        {
            if (source != null)
            {
                CutEnd = Extensions.GetRawElements(source, new TimeSpan(0, 0, 0, doubleTrackBar1.ValueRight));
                CutLength = CutEnd - CutBegin;
                label4.Text = doubleTrackBar1.ValueRight.ToString();
            }
        }

        private void BtnLoadAudio_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog()
            {
                Filter = "mp3 files (*.mp3)|*.mp3|m4a files (*.m4a)|*.m4a"
            })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    source = CSCore.Codecs.CodecFactory.Instance.GetCodec(OFD.FileName);
                    textBox1.Text = OFD.FileName;
                    doubleTrackBar1.Maximum = (int)Extensions.GetLength(source).TotalSeconds;
                }
            }
        }

        private void EncodeStreamM4A(WaveFormat targetformat, Stream sourcestream, Stream targetstream)
        {
            if (targetformat == null)
            {
                throw new Exception("Source format cannot be null");
            }
            if (!sourcestream.CanRead)
            {
                throw new Exception("Cannot read from source stream.");
            }
            if (!targetstream.CanWrite)
            {
                throw new Exception("Cannot write to target stream.");
            }
            byte[] buffer = new byte[source.WaveFormat.BytesPerSecond];
            CSCore.Codecs.AAC.AacEncoder encoder = new CSCore.Codecs.AAC.AacEncoder(source.WaveFormat, targetstream);
            while (buffer.Length != 0)
            {
                sourcestream.Read(buffer, 0, buffer.Length);
                encoder.Write(buffer, 0, buffer.Length);
                if (sourcestream.Position + buffer.Length > sourcestream.Length)
                {
                    buffer = new byte[sourcestream.Length - sourcestream.Position];
                }
            }
            encoder.Dispose();
        }
    }
}
