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
            if (YAMPVars.CORE != null && YAMPVars.ChorusEffect != null)
            {
                CB_EffectEnableToggle.Checked = YAMPVars.ChorusEffect.IsEnabled;
                CmbBx_PhaseBox.SelectedIndex = (int)YAMPVars.ChorusEffect.Phase;
                CmbBx_WaveFormBox.SelectedIndex = (int)YAMPVars.ChorusEffect.Waveform;
                NumUD_FreqUpDown.Value = (decimal)YAMPVars.ChorusEffect.Frequency;
                Tb_ChorusWDMixBar.Value = (int)YAMPVars.ChorusEffect.WetDryMix;
                Tb_ChorusFeedBar.Value = (int)YAMPVars.ChorusEffect.Feedback;
                Tb_ChorusDepthBar.Value = (int)YAMPVars.ChorusEffect.Depth;
                Tb_ChorusDelayBar.Value = (int)YAMPVars.ChorusEffect.Delay;
            }
            else
            {
                MessageBox.Show("Effect not Initialized.. Load a file");
                Close();
            }
        }

        private void Tb_ChorusWDMixBar_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.ChorusEffect.WetDryMix = Tb_ChorusWDMixBar.Value;
        }

        private void Tb_ChorusFeedBar_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.ChorusEffect.Feedback = Tb_ChorusFeedBar.Value;
        }

        private void Tb_ChorusDepthBar_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.ChorusEffect.Depth = Tb_ChorusDepthBar.Value;
        }

        private void NumUD_FreqUpDown_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.ChorusEffect.Frequency = (float)NumUD_FreqUpDown.Value;
        }

        private void Tb_ChorusDelayBar_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.ChorusEffect.Delay = Tb_ChorusDelayBar.Value;
        }

        private void CmbBx_PhaseBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            YAMPVars.ChorusEffect.Phase = (ChorusPhase)CmbBx_PhaseBox.SelectedIndex;
        }

        private void CmbBx_WaveFormBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            YAMPVars.ChorusEffect.Waveform = (ChorusWaveform)CmbBx_WaveFormBox.SelectedIndex;
        }

        private void CB_EffectEnableToggle_CheckedChanged(object sender, EventArgs e)
        {
            YAMPVars.ChorusEffect.IsEnabled = CB_EffectEnableToggle.Checked;
        }
    }
}
