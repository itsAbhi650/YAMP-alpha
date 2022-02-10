using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class PitchShiftDialog : Form
    { 
        int ShiftJump;
        public PitchShiftDialog()
        {
            InitializeComponent();
        }

        private void PitchShiftDialog_Load(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.Player != null && YAMPVars.PitchShiftEffect != null)
            {
                var value = Math.Log10(YAMPVars.PitchShiftEffect.PitchShiftFactor) / Math.Log10(2) * 120;
                Tb_PitchShiftingBar.Value = (int)value;
                textBox1.Text = "1";
            }
            else
            {
                MessageBox.Show("Effect not Initialized.. Load a file");
                Close();
            }
        }

        private void Tb_PitchShiftingBar_ValueChanged(object sender, EventArgs e)
        {
            if (YAMPVars.PitchShiftEffect != null)
            {
                YAMPVars.PitchShiftEffect.PitchShiftFactor = (float)Math.Pow(2, Tb_PitchShiftingBar.Value / 120.0);
            }
        }

        private void Btn_UpShift_Click(object sender, EventArgs e)
        {
            Tb_PitchShiftingBar.Value = Tb_PitchShiftingBar.Value + ShiftJump < Tb_PitchShiftingBar.Maximum ? Tb_PitchShiftingBar.Value + ShiftJump : Tb_PitchShiftingBar.Maximum;
        }

        private void Btn_DownShift_Click(object sender, EventArgs e)
        {
            Tb_PitchShiftingBar.Value = Tb_PitchShiftingBar.Value - ShiftJump > Tb_PitchShiftingBar.Minimum ? Tb_PitchShiftingBar.Value - ShiftJump : Tb_PitchShiftingBar.Minimum;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox1.Text, out ShiftJump);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Tb_PitchShiftingBar.Enabled = true;
            }
            else
            {
                Tb_PitchShiftingBar.Value = 0;
                Tb_PitchShiftingBar.Enabled = false;
            }
        }
    }
}
