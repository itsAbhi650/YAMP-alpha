using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class WavesReverbEffectDialog : Form
    {
        public WavesReverbEffectDialog()
        {
            InitializeComponent();
        }

        private void WavesReverbEffectDialog_Load(object sender, EventArgs e)
        {
            if (YAMPVars.CORE != null && YAMPVars.WavesReverbEffect != null)
            {
                //Enabled = true;
                Tb_WaveRev2Time.Value = (int)Math.Truncate(YAMPVars.WavesReverbEffect.ReverbTime);
                Tb_WaveRev2TimeFine.Value = (int)((YAMPVars.WavesReverbEffect.ReverbTime % 1) * 1000F);
                Tb_WaveRevHFRTR.Value = (int)(YAMPVars.WavesReverbEffect.HighFrequencyRTRatio * 1000F);
                Tb_WaveRevInGain.Value = (int)YAMPVars.WavesReverbEffect.InGain;
                Tb_WaveRev2Mix.Value = (int)YAMPVars.WavesReverbEffect.ReverbMix;
                CB_EffectEnableToggle.Checked = YAMPVars.WavesReverbEffect.IsEnabled;
            }
            else
            {
                MessageBox.Show("Effect not Initialized.. Load a file");
                Close();
            }
        }

        private void Tb_WaveRevHFRTR_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.WavesReverbEffect.HighFrequencyRTRatio = Tb_WaveRevHFRTR.Value / 1000F;
        }

        private void Tb_WaveRevInGain_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.WavesReverbEffect.InGain = Tb_WaveRevInGain.Value;
        }

        private void Tb_WaveRev2Mix_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.WavesReverbEffect.ReverbMix = Tb_WaveRev2Mix.Value;
        }

        private void Tb_WaveRevRevTime_ValueChanged(object sender, EventArgs e)
        {
            float RevTime, fRevTime, finalRevTime;
            RevTime = Tb_WaveRev2Time.Value;
            fRevTime = Tb_WaveRev2TimeFine.Value / 1000F;
            finalRevTime = RevTime + fRevTime;
            YAMPVars.WavesReverbEffect.ReverbTime = finalRevTime;
        }

        private void CB_EffectEnableToggle_CheckedChanged(object sender, EventArgs e)
        {
            YAMPVars.WavesReverbEffect.IsEnabled = CB_EffectEnableToggle.Checked;
        }

    }
}
