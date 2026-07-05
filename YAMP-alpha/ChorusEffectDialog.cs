using CSCore.Streams.Effects;
using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class ChorusEffectDialog : Form
    {
        public ChorusEffectDialog()
        {
            InitializeComponent();
        }

        private void ChorusEffectDialog_Load(object sender, EventArgs e)
        {
            if (YAMPVars.CORE != null && YAMPVars.CORE.ChorusEffect != null)
            {
                CB_EffectEnableToggle.Checked = YAMPVars.CORE.ChorusEffect.IsEnabled;
                CmbBx_PhaseBox.SelectedIndex = (int)YAMPVars.CORE.ChorusEffect.Phase;
                CmbBx_WaveFormBox.SelectedIndex = (int)YAMPVars.CORE.ChorusEffect.Waveform;
                NumUD_FreqUpDown.Value = (decimal)YAMPVars.CORE.ChorusEffect.Frequency;
                Tb_ChorusWDMixBar.Value = (int)YAMPVars.CORE.ChorusEffect.WetDryMix;
                Tb_ChorusFeedBar.Value = (int)YAMPVars.CORE.ChorusEffect.Feedback;
                Tb_ChorusDepthBar.Value = (int)YAMPVars.CORE.ChorusEffect.Depth;
                Tb_ChorusDelayBar.Value = (int)YAMPVars.CORE.ChorusEffect.Delay;
            }
            else
            {
                MessageBox.Show("Effect not Initialized.. Load a file");
                Close();
            }
        }

        private void Tb_ChorusWDMixBar_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.ChorusEffect.WetDryMix = Tb_ChorusWDMixBar.Value;
        }

        private void Tb_ChorusFeedBar_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.ChorusEffect.Feedback = Tb_ChorusFeedBar.Value;
        }

        private void Tb_ChorusDepthBar_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.ChorusEffect.Depth = Tb_ChorusDepthBar.Value;
        }

        private void NumUD_FreqUpDown_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.ChorusEffect.Frequency = (float)NumUD_FreqUpDown.Value;
        }

        private void Tb_ChorusDelayBar_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.ChorusEffect.Delay = Tb_ChorusDelayBar.Value;
        }

        private void CmbBx_PhaseBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.ChorusEffect.Phase = (ChorusPhase)CmbBx_PhaseBox.SelectedIndex;
        }

        private void CmbBx_WaveFormBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.ChorusEffect.Waveform = (ChorusWaveform)CmbBx_WaveFormBox.SelectedIndex;
        }

        private void CB_EffectEnableToggle_CheckedChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.ChorusEffect.IsEnabled = CB_EffectEnableToggle.Checked;
        }
    }
}
