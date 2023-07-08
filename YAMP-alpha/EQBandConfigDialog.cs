using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YAMP_alpha.Controls;

namespace YAMP_alpha
{
    public partial class EQBandConfigDialog : Form
    {
        private EqualizerFilter EQFilter = null;
        public EQBandConfigDialog(EqualizerFilter filter)
        {
            InitializeComponent();
            bandLabel.Text = filter.Filters[0].Frequency.ToString() + "Hz Band Impact";
            LeftBandWidth = new EQBand("BandWidth", 1, 40, 1, "ABC")
            {
                ShowBandValueInFooter = true
            };
            RightBandWidth = new EQBand("BandWidth", 1, 40, 1, "ABC")
            {
                ShowBandValueInFooter = true
            };
            EQFilter = filter;
            avgGain.BandValue = (int)(EQFilter.AverageGainDB / 12 * avgGain.BandMax);
            leftGainDb.BandValue = (int)(EQFilter.Filters[0].GainDB / 12 * leftGainDb.BandMax);
            rightGainDb.BandValue = (int)(EQFilter.Filters[1].GainDB / 12 * rightGainDb.BandMax);
        }

        private void avgGain_ValueChanged(object sender, EventArgs e)
        {
            if (EQFilter!=null)
            {
                double perc = avgGain.BandValue / (double)avgGain.BandMax;
                float value = (float)(perc * 12);
                EQFilter.AverageGainDB = value;
            }
        }

        private void leftGainDb_ValueChanged(object sender, EventArgs e)
        {
            if (EQFilter != null)
            {
                double perc = leftGainDb.BandValue / (double)leftGainDb.BandMax;
                float value = (float)(perc * 12);
                EQFilter.Filters[0].GainDB = value;
            }
        }

        private void rightGainDb_ValueChanged(object sender, EventArgs e)
        {
            if (EQFilter!=null)
            {
                double perc = rightGainDb.BandValue / (double)rightGainDb.BandMax;
                float value = (float)(perc * 12);
                EQFilter.Filters[1].GainDB = value;
            }
        }

        private void LeftBandWidth_ValueChanged(object sender, EventArgs e)
        {
            if (EQFilter != null)
            {
                
                EQFilter.Filters[0].BandWidth = LeftBandWidth.BandValue;
            }
        }

        private void RightBandWidth_ValueChanged(object sender, EventArgs e)
        {
            if (EQFilter != null)
            {
                EQFilter.Filters[1].BandWidth = RightBandWidth.BandValue;
            }
        }
    }
}
