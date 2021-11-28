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
    public partial class FlangerEffectDialog : Form
    {
        public FlangerEffectDialog()
        {
            InitializeComponent();
        }

        internal DmoFlangerEffect flangerEffect;

        public FlangerEffectDialog(ref YAMP_Core YampCore)
        {
            InitializeComponent();
            if (YampCore!=null)
            {
                flangerEffect = YampCore.Flanger;
            }
        }

        private void FlangerEffectDialog_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = flangerEffect.IsEnabled;
            Tb_FlangWDMixBar.Value = Convert.ToInt32(flangerEffect.WetDryMix);
            trackBar1.Value = Convert.ToInt32(flangerEffect.Feedback);
            trackBar2.Value = Convert.ToInt32(flangerEffect.Depth);
            numericUpDown1.Value = Convert.ToDecimal(flangerEffect.Frequency);
            comboBox1.SelectedIndex = (int)flangerEffect.Phase;
            comboBox2.SelectedIndex = (int)flangerEffect.Waveform;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            flangerEffect.Frequency = Convert.ToSingle(numericUpDown1.Value);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            flangerEffect.Phase = (FlangerPhase)comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            flangerEffect.Waveform = (FlangerWaveform)comboBox2.SelectedIndex;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            flangerEffect.IsEnabled = checkBox1.Checked;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            flangerEffect.Depth = Convert.ToSingle(trackBar2.Value);
        }

        private void Tb_FlangWDMixBar_ValueChanged(object sender, EventArgs e)
        {
            flangerEffect.WetDryMix = Convert.ToSingle(Tb_FlangWDMixBar.Value);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            flangerEffect.Feedback = Convert.ToSingle(trackBar1.Value);
        }
    }
}
