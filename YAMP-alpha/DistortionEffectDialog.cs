using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class DistortionEffectDialog : Form
    {
        public DistortionEffectDialog()
        {
            InitializeComponent();
        }

        private void DistortionEffectDialog_Load(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.DistortionEffect != null)
            {
                Tb_DistortEdge.Value = (int)YAMPVars.CORE.DistortionEffect.Edge;
                Tb_DistortGain.Value = (int)YAMPVars.CORE.DistortionEffect.Gain;
                Tb_DistortPostEQBW.Value = (int)YAMPVars.CORE.DistortionEffect.PostEQBandwidth;
                Tb_DistortPostEQCF.Value = (int)YAMPVars.CORE.DistortionEffect.PostEQCenterFrequency;
                Tb_DistortPLC.Value = (int)YAMPVars.CORE.DistortionEffect.PreLowpassCutoff;
                Cb_DistortEffectEnable.Checked = YAMPVars.CORE.DistortionEffect.IsEnabled;
            }
        }
        private void Tb_DistortEdge_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.DistortionEffect.Edge = Tb_DistortEdge.Value;
        }

        private void Tb_DistortGain_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.DistortionEffect.Gain = Tb_DistortGain.Value;
        }

        private void Tb_DistortPostEQBW_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.DistortionEffect.PostEQBandwidth = Tb_DistortPostEQBW.Value;
        }

        private void Tb_DistortPostEQCF_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.DistortionEffect.PostEQCenterFrequency = Tb_DistortPostEQCF.Value;
        }

        private void Tb_DistortPLC_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.DistortionEffect.PreLowpassCutoff = Tb_DistortPLC.Value;
        }

        private void Cb_DistortEffectEnable_CheckedChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.DistortionEffect.IsEnabled= Cb_DistortEffectEnable.Checked ;
        }
    }
}
