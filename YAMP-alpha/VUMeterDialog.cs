using CSCore.CoreAudioAPI;
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
    public partial class VUMeterDialog : Form
    {
        public VUMeterDialog()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float[] MeterInfoValues = YAMPVars.MeterInformation.GetChannelsPeakValues(2);
            aGauge1.Value = MeterInfoValues[0];
            aGauge2.Value = MeterInfoValues[1];
        }

        private void VUMeterDialog_Load(object sender, EventArgs e)
        {
            if (YAMPVars.CORE != null)
            {
                if (YAMPVars.MeterInformation != null)
                {
                    timer1.Start();
                }
                else
                {
                    YAMPVars.MeterInformation = AudioMeterInformation.FromDevice(YAMPVars.MediaDevice);
                    timer1.Start();
                }
            }
        }
    }
}
