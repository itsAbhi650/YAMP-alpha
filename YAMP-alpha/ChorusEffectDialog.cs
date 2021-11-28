using CSCore.Streams.Effects;
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
    public partial class ChorusEffectDialog : Form
    {
        //YAMP_Core Core;
        DmoChorusEffect ChorusEffect;
        public ChorusEffectDialog()
        {
            InitializeComponent();
        }

        public ChorusEffectDialog(ref YAMP_Core YampCore)
        {
            InitializeComponent();
            ChorusEffect = YampCore.ChorusEffect ?? throw new ArgumentNullException("YampCore");
        }

        private void ChorusEffectDialog_Load(object sender, EventArgs e)
        {
            CB_EffectEnableToggle.Checked = ChorusEffect.IsEnabled;
            CmbBx_PhaseBox.SelectedIndex = (int)ChorusEffect.Phase;
            CmbBx_WaveFormBox.SelectedIndex = (int)ChorusEffect.Waveform;
            NumUD_FreqUpDown.Value = (decimal)ChorusEffect.Frequency;
            Tb_ChorusWDMixBar.Value = (int)ChorusEffect.WetDryMix;
            Tb_ChorusFeedBar.Value = (int)ChorusEffect.Feedback;
            Tb_ChorusDepthBar.Value = (int)ChorusEffect.Depth;
            Tb_ChorusDelayBar.Value = (int)ChorusEffect.Delay;
        }

        private void Tb_ChorusWDMixBar_ValueChanged(object sender, EventArgs e)
        {
            ChorusEffect.WetDryMix = Tb_ChorusWDMixBar.Value;
        }

        private void Tb_ChorusFeedBar_ValueChanged(object sender, EventArgs e)
        {
            ChorusEffect.Feedback = Tb_ChorusFeedBar.Value;
        }

        private void Tb_ChorusDepthBar_ValueChanged(object sender, EventArgs e)
        {
            ChorusEffect.Depth = Tb_ChorusDepthBar.Value;
        }

        private void NumUD_FreqUpDown_ValueChanged(object sender, EventArgs e)
        {
            ChorusEffect.Frequency = (float)NumUD_FreqUpDown.Value;
        }

        private void Tb_ChorusDelayBar_ValueChanged(object sender, EventArgs e)
        {
            ChorusEffect.Delay = Tb_ChorusDelayBar.Value;
        }

        private void CmbBx_PhaseBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChorusEffect.Phase = (ChorusPhase)CmbBx_PhaseBox.SelectedIndex;
        }

        private void CmbBx_WaveFormBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChorusEffect.Waveform = (ChorusWaveform)CmbBx_WaveFormBox.SelectedIndex;
        }

        private void CB_EffectEnableToggle_CheckedChanged(object sender, EventArgs e)
        {
            ChorusEffect.IsEnabled = CB_EffectEnableToggle.Checked;
        }
    }
}
