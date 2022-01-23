using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class Waveform : Form
    {
        private bool BYPASS_TMR = false;

        public Waveform()
        {
            InitializeComponent();
        }

        private void Waveform_Load(object sender, EventArgs e)
        {
            if (YAMPVars.CORE != null)
            {
                tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;
                tableLayoutPanel1.ColumnStyles[0].Width = 50;
                timer1.Start();
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            BYPASS_TMR = YAMPVars.CORE.PlayerStopped || YAMPVars.CORE.PlayerPaused ? true : false;
            if (!BYPASS_TMR)
            {
                waveformPainter1.AddMax(YAMPVars.CORE.WaveFormLEFT);
                waveformPainter2.AddMax(YAMPVars.CORE.WaveFormRIGHT);
            }
        }
    }
}
