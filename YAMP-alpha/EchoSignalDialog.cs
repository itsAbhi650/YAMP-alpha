using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class EchoSignalDialog : Form
    {
        int EchoJump;
        public EchoSignalDialog()
        {
            InitializeComponent();
        }

        private void EchoSignalDialog_Load(object sender, EventArgs e)
        {
            if (YAMPVars.CORE != null && YAMPVars.EchoEffect != null)
            {
                //Enabled = true;
                Tb_EchoShiftingBar.Value = (int)YAMPVars.EchoEffect.WetDryMix;
                Tb_EchoShiftingBar.ValueChanged += Tb_EchoShiftingBar_ValueChanged;
                textBox1.TextChanged += TextBox1_TextChanged;
                textBox1.Text = "1";
                YAMPVars.EchoEffect.IsEnabled = checkBox1.Checked;
                checkBox2.Checked = YAMPVars.EchoEffect.PanDelay;
                numericUpDown1.Value = Convert.ToDecimal(YAMPVars.EchoEffect.Feedback);
                numericUpDown2.Value = Convert.ToDecimal(YAMPVars.EchoEffect.LeftDelay);
                numericUpDown3.Value = Convert.ToDecimal(YAMPVars.EchoEffect.RightDelay);
            }
            else
            {
                MessageBox.Show("Effect not Initialized.. Load a file");
                Close();
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox1.Text, out EchoJump);
        }

        private void Tb_EchoShiftingBar_ValueChanged(object sender, EventArgs e)
        {
            if (YAMPVars.EchoEffect != null)
            {
                YAMPVars.EchoEffect.WetDryMix = Tb_EchoShiftingBar.Value;
            }
        }

        private void Btn_UpShift_Click(object sender, EventArgs e)
        {
            int JUMP = Tb_EchoShiftingBar.Value + EchoJump;
            Tb_EchoShiftingBar.Value = JUMP < Tb_EchoShiftingBar.Maximum ? JUMP : Tb_EchoShiftingBar.Maximum;
        }

        private void Btn_DownShift_Click(object sender, EventArgs e)
        {
            int JUMP = Tb_EchoShiftingBar.Value - EchoJump;
            Tb_EchoShiftingBar.Value = JUMP > Tb_EchoShiftingBar.Minimum ? JUMP : Tb_EchoShiftingBar.Minimum;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            YAMPVars.EchoEffect.IsEnabled = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            YAMPVars.EchoEffect.PanDelay = checkBox2.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.EchoEffect.Feedback = Convert.ToSingle(numericUpDown1.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.EchoEffect.LeftDelay = Convert.ToSingle(numericUpDown2.Value);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.EchoEffect.RightDelay = Convert.ToSingle(numericUpDown3.Value);
        }
    }
}
