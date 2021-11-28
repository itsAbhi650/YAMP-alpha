using CSCore.Streams.Effects;
using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class PitchShiftDialog : Form
    {
        PitchShifter pitchShift;
        int ShiftJump;
        public PitchShiftDialog()
        {
            InitializeComponent();
        }

        public PitchShiftDialog(ref PitchShifter pitchShifter)
        {
            InitializeComponent();
            if (pitchShifter != null)
            {
                pitchShift = pitchShifter;
            }
            else
            {
                throw new NullReferenceException("pitchShifter");
            }
        }

        public PitchShiftDialog(ref YAMP_Core YampCore)
        {
            InitializeComponent();
            if (YampCore != null)
            {
                pitchShift = YampCore.PitchShift;
            }
            else
            {
                throw new NullReferenceException("YampCore");
            }
        }

        private void PitchShiftDialog_Load(object sender, EventArgs e)
        {
            var value = Math.Log10(pitchShift.PitchShiftFactor) / Math.Log10(2) * 120;
            Tb_PitchShiftingBar.Value = (int)value;
            checkBox1.Checked = true;
            textBox1.Text = "1";
        }

        private void Tb_PitchShiftingBar_ValueChanged(object sender, EventArgs e)
        {
            if (pitchShift != null)
            {
                pitchShift.PitchShiftFactor = (float)Math.Pow(2, Tb_PitchShiftingBar.Value / 120.0);
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
