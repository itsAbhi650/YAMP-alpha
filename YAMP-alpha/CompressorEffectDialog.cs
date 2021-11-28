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
    public partial class CompressorEffectDialog : Form
    {

        private static DmoCompressorEffect CompressorEffect;
        private static YAMP_Core Core;

        public CompressorEffectDialog()
        {
            InitializeComponent();
        }

        public CompressorEffectDialog(ref YAMP_Core YampCore)
        {
            InitializeComponent();
            Core = YampCore ?? throw new ArgumentException("YampCore");
            CompressorEffect = YampCore.CompressorEffect ?? throw new ArgumentException("YampCore");
        }

        private void CompressorEffectDialog_Load(object sender, EventArgs e)
        {
            Tb_CompGain.Value = (int)CompressorEffect.Gain;
            Tb_CompRatio.Value = (int)CompressorEffect.Ratio;
            Tb_CompRelease.Value = (int)CompressorEffect.Release;
            Tb_CompThresh.Value = (int)CompressorEffect.Threshold;
            CB_EffectEnableToggle.Checked = CompressorEffect.IsEnabled;
            NumUD_PreDelUpDown.Value = (decimal)CompressorEffect.Predelay;
            Tb_CompAtckStrn.Value = (int)Math.Truncate(CompressorEffect.Attack);
            Tb_CompFineAtckStrn.Value = (int)((CompressorEffect.Attack % 1) * 100);
            groupBox1.Text = string.Format("Attack: {0}", CompressorEffect.Attack);
            groupBox7.Text = string.Format("Threshold: {0}", CompressorEffect.Threshold);
            groupBox2.Text = string.Format("Gain: {0}", CompressorEffect.Gain);
            groupBox3.Text = string.Format("Ratio: {0}", CompressorEffect.Ratio);
            groupBox6.Text = string.Format("Release: {0}", CompressorEffect.Release);

        }

        private void Tb_CompFineAtckStrn_ValueChanged(object sender, EventArgs e)
        {
            float Attack, fAttack, FinalAttack;
            Attack = Tb_CompAtckStrn.Value;
            fAttack = Tb_CompFineAtckStrn.Value / 100F;
            FinalAttack = Attack + fAttack;
            CompressorEffect.Attack = FinalAttack;
            groupBox1.Text = string.Format("Attack: {0}", FinalAttack);
        }

        private void CB_EffectEnableToggle_CheckedChanged(object sender, EventArgs e)
        {
            CompressorEffect.IsEnabled = CB_EffectEnableToggle.Checked;
        }

        private void Tb_CompThresh_ValueChanged(object sender, EventArgs e)
        {
            CompressorEffect.Threshold = Tb_CompThresh.Value;
            groupBox7.Text = string.Format("Threshold: {0}", Tb_CompThresh.Value);
        }

        private void NumUD_PreDelUpDown_ValueChanged(object sender, EventArgs e)
        {
            CompressorEffect.Predelay = (float)NumUD_PreDelUpDown.Value;
        }

        private void Tb_CompGain_ValueChanged(object sender, EventArgs e)
        {
            CompressorEffect.Gain = Tb_CompGain.Value;
            groupBox2.Text = string.Format("Gain: {0}", Tb_CompGain.Value);
        }

        private void Tb_CompRatio_ValueChanged(object sender, EventArgs e)
        {
            CompressorEffect.Ratio = Tb_CompRatio.Value;
            groupBox3.Text = string.Format("Ratio: {0}", Tb_CompRatio.Value);
        }

        private void Tb_CompRelease_ValueChanged(object sender, EventArgs e)
        {
            CompressorEffect.Release = Tb_CompRelease.Value;
            groupBox6.Text = string.Format("Release: {0}", Tb_CompRelease.Value);
        }
    }
}
