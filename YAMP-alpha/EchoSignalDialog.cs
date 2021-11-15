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
    public partial class EchoSignalDialog : Form
    {
        public YAMP_Core Core { get; set; }
        int EchoJump;
        public EchoSignalDialog()
        {
            InitializeComponent();
        }

        private void EchoSignalDialog_Load(object sender, EventArgs e)
        {
            if (Core!=null)
            {
                Tb_EchoShiftingBar.Value = (int)Core.DCE.WetDryMix;
                Tb_EchoShiftingBar.ValueChanged += Tb_EchoShiftingBar_ValueChanged;
                textBox1.TextChanged += TextBox1_TextChanged;
                textBox1.Text = "1";
            }
            else
            {
                MessageBox.Show("Core not initialized");
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox1.Text, out EchoJump);
        }

        private void Tb_EchoShiftingBar_ValueChanged(object sender, EventArgs e)
        {
            if (Core!=null)
            {
                Core.DCE.WetDryMix = Tb_EchoShiftingBar.Value;
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
    }
}
