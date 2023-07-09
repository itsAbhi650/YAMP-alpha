using CSCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class ResamplerDialog : Form
    {
       // private IWaveSource SourceAudio;
        public ResamplerDialog()
        {
            InitializeComponent();
            validSampleBox.DataSource = Encoder.Mpeg3SampleRates;
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog() { Filter = CSCore.Codecs.CodecFactory.SupportedFilesFilterEn })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    fileBox.Text = OFD.FileName;
                    currentSampleBox.Text=Encoder.GetSampleRate(fileBox.Text).ToString();
                }
            }
        }

        private void BtnResample_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog SFD = new SaveFileDialog())
            {
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    Progress<int> progress = new Progress<int>(per =>
                    {
                        resampleProgressBar.Value = per;
                    });
                    Encoder.Resample(fileBox.Text, SFD.FileName, Convert.ToInt32(validSampleBox.Text), p: progress);
                }
            }
        }
    }
}
