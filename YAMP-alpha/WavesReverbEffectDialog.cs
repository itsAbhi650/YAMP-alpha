using CSCore.Streams.Effects;
using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class WavesReverbEffectDialog : Form
    {
        private static DmoWavesReverbEffect WavesReverbEffect;
        public WavesReverbEffectDialog()
        {
            InitializeComponent();
        }

        public WavesReverbEffectDialog(ref YAMP_Core YampCore)
        {
            InitializeComponent();
            WavesReverbEffect = YampCore.WavesReverbEffect ?? throw new ArgumentNullException("YampCore");
        }

        private void Tb_WaveRevHFRTR_ValueChanged(object sender, EventArgs e)
        {
            WavesReverbEffect.HighFrequencyRTRatio = Tb_WaveRevHFRTR.Value / 1000F;
        }

        private void Tb_WaveRevInGain_ValueChanged(object sender, EventArgs e)
        {
            WavesReverbEffect.InGain = Tb_WaveRevInGain.Value;
        }

        private void Tb_WaveRev2Mix_ValueChanged(object sender, EventArgs e)
        {
            WavesReverbEffect.ReverbMix = Tb_WaveRev2Mix.Value;
        }

        private void Tb_WaveRevRevTime_ValueChanged(object sender, EventArgs e)
        {
            float RevTime, fRevTime, finalRevTime;
            RevTime = Tb_WaveRev2Time.Value;
            fRevTime = Tb_WaveRev2TimeFine.Value / 1000F;//((WavesReverbEffect.ReverbTime % 1) * 1000F);
            finalRevTime = RevTime + fRevTime;
            WavesReverbEffect.ReverbTime = finalRevTime;
        }

        private void CB_EffectEnableToggle_CheckedChanged(object sender, EventArgs e)
        {
            WavesReverbEffect.IsEnabled = CB_EffectEnableToggle.Checked;
        }

        private void WavesReverbEffectDialog_Load(object sender, EventArgs e)
        {
            Tb_WaveRev2Time.Value = (int)Math.Truncate(WavesReverbEffect.ReverbTime);
            Tb_WaveRev2TimeFine.Value = (int)((WavesReverbEffect.ReverbTime % 1) * 1000F);
            Tb_WaveRevHFRTR.Value = (int)(WavesReverbEffect.HighFrequencyRTRatio * 1000F);
            Tb_WaveRevInGain.Value = (int)WavesReverbEffect.InGain;
            Tb_WaveRev2Mix.Value = (int)WavesReverbEffect.ReverbMix;
            CB_EffectEnableToggle.Checked = WavesReverbEffect.IsEnabled;
        }
    }
}
