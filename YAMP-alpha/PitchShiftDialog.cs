using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class PitchShiftDialog : Form
    {
        public YAMP_Core Core { get; set; }
        int ShiftJump;
        public PitchShiftDialog()
        {
            InitializeComponent();
        }

        private void PitchShiftDialog_Load(object sender, EventArgs e)
        {
            if (Core.PlayerSource != null)
            {
                var value = (int)(Core.PitchShift != null
                ? Math.Log10(Core.PitchShift.PitchShiftFactor) / Math.Log10(2) * 120
                : 0);
                Tb_PitchShiftingBar.Value = value;
                textBox1.Text = "1";
            }
            else
            {
                MessageBox.Show("Core not initialized");
            }
        }

        private void Tb_PitchShiftingBar_ValueChanged(object sender, EventArgs e)
        {
            if (Core.PitchShift != null)
            {
                Core.PitchShift.PitchShiftFactor = (float)Math.Pow(2, Tb_PitchShiftingBar.Value / 120.0);
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


    }
}
