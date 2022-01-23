using CSCore.Streams.Effects;
using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class FlangerEffectDialog : Form
    {
        public FlangerEffectDialog()
        {
            InitializeComponent();
        }

        private void FlangerEffectDialog_Load(object sender, EventArgs e)
        {
            if (YAMPVars.CORE != null && YAMPVars.FlangerEffect != null)
            {
                Enabled = true;
                checkBox1.Checked = YAMPVars.FlangerEffect.IsEnabled;
                Tb_FlangWDMixBar.Value = Convert.ToInt32(YAMPVars.FlangerEffect.WetDryMix);
                trackBar1.Value = Convert.ToInt32(YAMPVars.FlangerEffect.Feedback);
                trackBar2.Value = Convert.ToInt32(YAMPVars.FlangerEffect.Depth);
                numericUpDown1.Value = Convert.ToDecimal(YAMPVars.FlangerEffect.Frequency);
                comboBox1.SelectedIndex = (int)YAMPVars.FlangerEffect.Phase;
                comboBox2.SelectedIndex = (int)YAMPVars.FlangerEffect.Waveform;
            }
            else
            {
                MessageBox.Show("Effect not Initialized.. Load a file");
                Close();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.FlangerEffect.Frequency = Convert.ToSingle(numericUpDown1.Value);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            YAMPVars.FlangerEffect.Phase = (FlangerPhase)comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            YAMPVars.FlangerEffect.Waveform = (FlangerWaveform)comboBox2.SelectedIndex;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            YAMPVars.FlangerEffect.IsEnabled = checkBox1.Checked;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.FlangerEffect.Depth = Convert.ToSingle(trackBar2.Value);
        }

        private void Tb_FlangWDMixBar_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.FlangerEffect.WetDryMix = Convert.ToSingle(Tb_FlangWDMixBar.Value);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.FlangerEffect.Feedback = Convert.ToSingle(trackBar1.Value);
        }
    }
}
