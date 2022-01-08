using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSCore;
using CSCore.Streams;

namespace YAMP_alpha
{
    public partial class Waveform : Form
    {
        ISampleSource Source;
        YAMP_Core CoreSource;
        private bool BYPASS_TMR = false;

        public Waveform()
        {
            InitializeComponent();
        }

        public Waveform(ref YAMP_Core Core)
        {
            InitializeComponent();
            CoreSource = Core;
        }

        private void Waveform_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;
            tableLayoutPanel1.ColumnStyles[0].Width = 50;
            timer1.Start();
        }

       
        private void timer1_Tick(object sender, EventArgs e)
        {
            BYPASS_TMR = CoreSource.PlayerStopped || CoreSource.PlayerPaused ? true : false;
            if (!BYPASS_TMR)
            {
                waveformPainter1.AddMax(CoreSource.WaveFormLEFT);
                waveformPainter2.AddMax(CoreSource.WaveFormRIGHT);
            }
        }
    }
}
