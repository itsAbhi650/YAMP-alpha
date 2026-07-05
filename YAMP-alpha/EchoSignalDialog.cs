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
            if (YAMPVars.CORE != null && YAMPVars.CORE.EchoEffect != null)
            {
                //Enabled = true;
                Tb_EchoShiftingBar.Value = (int)YAMPVars.CORE.EchoEffect.WetDryMix;
                Tb_EchoShiftingBar.ValueChanged += Tb_EchoShiftingBar_ValueChanged;
                textBox1.TextChanged += TextBox1_TextChanged;
                textBox1.Text = "1";
                YAMPVars.CORE.EchoEffect.IsEnabled = checkBox1.Checked;
                checkBox2.Checked = YAMPVars.CORE.EchoEffect.PanDelay;
                numericUpDown1.Value = Convert.ToDecimal(YAMPVars.CORE.EchoEffect.Feedback);
                numericUpDown2.Value = Convert.ToDecimal(YAMPVars.CORE.EchoEffect.LeftDelay);
                numericUpDown3.Value = Convert.ToDecimal(YAMPVars.CORE.EchoEffect.RightDelay);
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
            if (YAMPVars.CORE.EchoEffect != null)
            {
                YAMPVars.CORE.EchoEffect.WetDryMix = Tb_EchoShiftingBar.Value;
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
            YAMPVars.CORE.EchoEffect.IsEnabled = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.EchoEffect.PanDelay = checkBox2.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.EchoEffect.Feedback = Convert.ToSingle(numericUpDown1.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.EchoEffect.LeftDelay = Convert.ToSingle(numericUpDown2.Value);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.CORE.EchoEffect.RightDelay = Convert.ToSingle(numericUpDown3.Value);
        }
    }
}
