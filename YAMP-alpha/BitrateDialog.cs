using CSCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class BitrateDialog : Form
    {
        public BitrateDialog()
        {
            InitializeComponent();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog() { Filter = CSCore.Codecs.CodecFactory.SupportedFilesFilterEn })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    fileBox.Text = OFD.FileName;
                    currentBitrateBox.Text = Encoder.GetBitRate(fileBox.Text).ToString();
                }
            }
        }

        private void BitrateDialog_Load(object sender, EventArgs e)
        {
            validBitRateBox.DataSource = Encoder.GetSupportedBitRates(AudioSubTypes.MpegLayer3).OrderBy(x => x).ToArray();

        }

        private void BtnChange_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog SFD = new SaveFileDialog())
            {
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    IWaveSource source = null; 
                    var enc = Encoder.GetEncoder(fileBox.Text, SFD.FileName, out source, Convert.ToInt32(validBitRateBox.SelectedItem));
                    Encoder.PerformOperation(enc, source, new Progress<int>(per => { bitrateProgressBar.Value = per; }));
                    enc.Dispose();
                    source.Dispose();
                    if (Cb_RetainTags.Checked)
                    {
                        Encoder.TagCopy(fileBox.Text, SFD.FileName);
                    }
                    MessageBox.Show("Done!");
                }
            }
        }
    }
}
