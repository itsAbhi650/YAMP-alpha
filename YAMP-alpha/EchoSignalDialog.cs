using CSCore.Streams.Effects;
using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class EchoSignalDialog : Form
    {
        public DmoEchoEffect EchoEffect;
        int EchoJump;
        public EchoSignalDialog()
        {
            InitializeComponent();
        }

        public EchoSignalDialog(ref DmoEchoEffect dmoEchoEffect)
        {
            InitializeComponent();
            if (dmoEchoEffect != null)
            {
                EchoEffect = dmoEchoEffect;
            }
            else
            {
                throw new NullReferenceException("dmoEchoEffect");
            }
        }

        public EchoSignalDialog(ref YAMP_Core YampCore)
        {
            InitializeComponent();
            if (YampCore != null)
            {
                EchoEffect = YampCore.DCE;
            }
            else
            {
                throw new NullReferenceException("YampCore");
            }
        }

        private void EchoSignalDialog_Load(object sender, EventArgs e)
        {
            Tb_EchoShiftingBar.Value = (int)EchoEffect.WetDryMix;
            Tb_EchoShiftingBar.ValueChanged += Tb_EchoShiftingBar_ValueChanged;
            textBox1.TextChanged += TextBox1_TextChanged;
            textBox1.Text = "1";
            EchoEffect.IsEnabled = checkBox1.Checked;
            checkBox2.Checked = EchoEffect.PanDelay;
            numericUpDown1.Value = Convert.ToDecimal(EchoEffect.Feedback);
            numericUpDown2.Value = Convert.ToDecimal(EchoEffect.LeftDelay);
            numericUpDown3.Value = Convert.ToDecimal(EchoEffect.RightDelay);
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox1.Text, out EchoJump);
        }

        private void Tb_EchoShiftingBar_ValueChanged(object sender, EventArgs e)
        {
            if (EchoEffect != null)
            {
                EchoEffect.WetDryMix = Tb_EchoShiftingBar.Value;
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
            EchoEffect.IsEnabled = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            EchoEffect.PanDelay = checkBox2.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            EchoEffect.Feedback = Convert.ToSingle(numericUpDown1.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            EchoEffect.LeftDelay = Convert.ToSingle(numericUpDown2.Value);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            EchoEffect.RightDelay = Convert.ToSingle(numericUpDown3.Value);
        }
    }
}
