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
            if (YAMPVars.CORE != null && YAMPVars.CORE.FlangerEffect != null)
            {
                Enabled = true;
                checkBox1.Checked = YAMPVars.CORE.FlangerEffect.IsEnabled;
                Tb_FlangWDMixBar.Value = Convert.ToInt32(YAMPVars.CORE.FlangerEffect.WetDryMix);
                trackBar1.Value = Convert.ToInt32(YAMPVars.CORE.FlangerEffect.Feedback);
                trackBar2.Value = Convert.ToInt32(YAMPVars.CORE.FlangerEffect.Depth);
                numericUpDown1.Value = Convert.ToDecimal(YAMPVars.CORE.FlangerEffect.Frequency);
                comboBox1.SelectedIndex = (int)YAMPVars.CORE.FlangerEffect.Phase;
                comboBox2.SelectedIndex = (int)YAMPVars.CORE.FlangerEffect.Waveform;
            }
            else
            {
                MessageBox.Show("Effect not Initialized.. Load a file");
                Close();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.FlangerEffect.Frequency = Convert.ToSingle(numericUpDown1.Value);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.FlangerEffect.Phase = (FlangerPhase)comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.FlangerEffect.Waveform = (FlangerWaveform)comboBox2.SelectedIndex;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.FlangerEffect.IsEnabled = checkBox1.Checked;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.FlangerEffect.Depth = Convert.ToSingle(trackBar2.Value);
        }

        private void Tb_FlangWDMixBar_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.FlangerEffect.WetDryMix = Convert.ToSingle(Tb_FlangWDMixBar.Value);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.FlangerEffect.Feedback = Convert.ToSingle(trackBar1.Value);
        }
    }
}
