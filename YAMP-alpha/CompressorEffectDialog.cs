using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class CompressorEffectDialog : Form
    {
        public CompressorEffectDialog()
        {
            InitializeComponent();
        }

        private void CompressorEffectDialog_Load(object sender, EventArgs e)
        {
            if (YAMPVars.CORE != null && YAMPVars.CORE.PlayerSource != null)
            {
                Tb_CompGain.Value = (int)YAMPVars.CompressorEffect.Gain;
                Tb_CompRatio.Value = (int)YAMPVars.CompressorEffect.Ratio;
                Tb_CompRelease.Value = (int)YAMPVars.CompressorEffect.Release;
                Tb_CompThresh.Value = (int)YAMPVars.CompressorEffect.Threshold;
                CB_EffectEnableToggle.Checked = YAMPVars.CompressorEffect.IsEnabled;
                NumUD_PreDelUpDown.Value = (decimal)YAMPVars.CompressorEffect.Predelay;
                Tb_CompAtckStrn.Value = (int)Math.Truncate(YAMPVars.CompressorEffect.Attack);
                Tb_CompFineAtckStrn.Value = (int)((YAMPVars.CompressorEffect.Attack % 1) * 100);
                groupBox1.Text = string.Format("Attack: {0}", YAMPVars.CompressorEffect.Attack);
                groupBox7.Text = string.Format("Threshold: {0}", YAMPVars.CompressorEffect.Threshold);
                groupBox2.Text = string.Format("Gain: {0}", YAMPVars.CompressorEffect.Gain);
                groupBox3.Text = string.Format("Ratio: {0}", YAMPVars.CompressorEffect.Ratio);
                groupBox6.Text = string.Format("Release: {0}", YAMPVars.CompressorEffect.Release);
            }
            else
            {
                MessageBox.Show("Effect not Initialized.. Load a file");
                Close();
            }

        }

        private void Tb_CompFineAtckStrn_ValueChanged(object sender, EventArgs e)
        {
            float Attack, fAttack, FinalAttack;
            Attack = Tb_CompAtckStrn.Value;
            fAttack = Tb_CompFineAtckStrn.Value / 100F;
            FinalAttack = Attack + fAttack;
            YAMPVars.CompressorEffect.Attack = FinalAttack;
            groupBox1.Text = string.Format("Attack: {0}", FinalAttack);
        }

        private void CB_EffectEnableToggle_CheckedChanged(object sender, EventArgs e)
        {
            YAMPVars.CompressorEffect.IsEnabled = CB_EffectEnableToggle.Checked;
        }

        private void Tb_CompThresh_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CompressorEffect.Threshold = Tb_CompThresh.Value;
            groupBox7.Text = string.Format("Threshold: {0}", Tb_CompThresh.Value);
        }

        private void NumUD_PreDelUpDown_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CompressorEffect.Predelay = (float)NumUD_PreDelUpDown.Value;
        }

        private void Tb_CompGain_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CompressorEffect.Gain = Tb_CompGain.Value;
            groupBox2.Text = string.Format("Gain: {0}", Tb_CompGain.Value);
        }

        private void Tb_CompRatio_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CompressorEffect.Ratio = Tb_CompRatio.Value;
            groupBox3.Text = string.Format("Ratio: {0}", Tb_CompRatio.Value);
        }

        private void Tb_CompRelease_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CompressorEffect.Release = Tb_CompRelease.Value;
            groupBox6.Text = string.Format("Release: {0}", Tb_CompRelease.Value);
        }
    }
}
