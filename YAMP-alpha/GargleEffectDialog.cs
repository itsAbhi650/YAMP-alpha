using CSCore.Streams.Effects;
using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class GargleEffectDialog : Form
    {
        public GargleEffectDialog()
        {
            InitializeComponent();
        }

        private void GargleEffectDialog_Load(object sender, EventArgs e)
        {
            if (YAMPVars.CORE != null && YAMPVars.CORE.PlayerSource != null)
            {
                Enabled = true;
                trackBar1.Value = YAMPVars.GargleEffect.RateHz;
                comboBox1.SelectedIndex = (int)YAMPVars.GargleEffect.WaveShape;
                checkBox1.Checked = YAMPVars.GargleEffect.IsEnabled;
            }
            else
            {
                MessageBox.Show("Effect not Initialized.. Load a file");
                Close();
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.GargleEffect.RateHz = trackBar1.Value;
            updateInfo();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            YAMPVars.GargleEffect.WaveShape = (GargleWaveShape)comboBox1.SelectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            trackBar1.Value = trackBar1.Value - Convert.ToInt32(textBox1.Text);
            updateInfo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            trackBar1.Value = trackBar1.Value + Convert.ToInt32(textBox1.Text);
            updateInfo();
        }

        private void updateInfo()
        {
            label1.Text = string.Format("Change the value ({0} Hz) to observe a change in effect", YAMPVars.GargleEffect.RateHz);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            YAMPVars.GargleEffect.IsEnabled = checkBox1.Checked;
        }
    }
}
