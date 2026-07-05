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
            var peakMeter = YAMPVars.CORE?.AudioPeakMeter;
            if (peakMeter != null)
            {
                peakMeter.PeakCalculated += AudioPeakMeter_PeakCalculated;
            }
        }

        private void PeakMeterDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Unsubscribe from event to prevent memory leaks
            var peakMeter = YAMPVars.CORE?.AudioPeakMeter;
            if (peakMeter != null)
            {
                peakMeter.PeakCalculated -= AudioPeakMeter_PeakCalculated;
            }
        }

        private void AudioPeakMeter_PeakCalculated(object sender, CSCore.Streams.PeakEventArgs e)
        {
            var peakMeter = YAMPVars.CORE?.AudioPeakMeter;
            if (peakMeter != null && !YAMPVars.CORE.PlayerStopped)
            {
                // Clone or snapshot peak values to avoid cross-thread issues
                float[] channelPeaks;
                lock (peakMeter)
                {
                    channelPeaks = peakMeter.ChannelPeakValues.ToArray();
                }

                int[] PeakVals = channelPeaks.Select(x => (int)(x * 100F)).ToArray();

                int Left, Right, avgpeak;

                if (PeakVals.Length > 1)
                {
                    Left = PeakVals[0];
                    Right = PeakVals[1];
                    avgpeak = (int)((Left + Right) / 2F);
                }
                else if (PeakVals.Length == 1)
                {
                    Left = Right = avgpeak = PeakVals[0];
                }
                else
                {
                    return; // no peaks to display
                }

                // Marshal back to UI thread
                if (meterControl1.InvokeRequired)
                {
                    meterControl1.Invoke((MethodInvoker)(() =>
                    {
                        meterControl1.Level = Left;// Clamp(Left, 0, meterControl1.Maximum);
                        meterControl2.Level = Right;// Clamp(Right, 0, meterControl2.Maximum);
                        meterControl3.Level = avgpeak;// Clamp(avgpeak, 0, meterControl3.Maximum);
                    }));
                }
                else
                {
                    meterControl1.Level = Left;// Clamp(Left, 0, meterControl1.Maximum);
                    meterControl2.Level = Right;// Clamp(Right, 0, meterControl2.Maximum);
                    meterControl3.Level = avgpeak;// Clamp(avgpeak, 0, meterControl3.Maximum);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
