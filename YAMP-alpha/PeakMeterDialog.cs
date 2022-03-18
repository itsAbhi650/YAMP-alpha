using System;
using System.Linq;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class PeakMeterDialog : Form
    {

        public PeakMeterDialog()
        {
            InitializeComponent();
        }

        private void PeakMeterDialog_Load(object sender, EventArgs e)
        {
            if (YAMPVars.CORE != null && YAMPVars.AudioPeakMeter != null)
            {
                YAMPVars.AudioPeakMeter.PeakCalculated += AudioPeakMeter_PeakCalculated;
            }
        }

        private void AudioPeakMeter_PeakCalculated(object sender, CSCore.Streams.PeakEventArgs e)
        {
            if (!YAMPVars.CORE.PlayerStopped)
            {
                int[] PeakVals = YAMPVars.AudioPeakMeter.ChannelPeakValues.Select(x => (int)(x * 100F)).ToArray();
                int Left;
                int Right;
                int avgpeak;
                if (PeakVals.Length > 1)
                {
                    Left = PeakVals[0];
                    Right = PeakVals[1];
                    avgpeak = (int)((Left + Right) / 2F);
                }
                else
                {
                    Left = PeakVals[0];
                    Right = PeakVals[0];
                    avgpeak = PeakVals[0];
                }
                trackBar1.Value = Left;
                trackBar2.Value = Right;
                trackBar3.Value = avgpeak;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
