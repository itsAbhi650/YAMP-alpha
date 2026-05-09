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
        private int maxdb;
        public EQBandConfigDialog(EqualizerFilter filter, int MaxDB)
        {
            InitializeComponent();
            maxdb = MaxDB;
            bandLabel.Text = filter.Filters[0].Frequency.ToString() + "Hz Band Impact";

            LeftBandWidth.Text = "BandWidth"; // Updates Header (EQBox.Text)
            LeftBandWidth.BandMin = 1;
            LeftBandWidth.BandMax = 40;
            LeftBandWidth.BandValue = 1;
            LeftBandWidth.FooterText = "ABC";
            LeftBandWidth.ShowBandValueInFooter = true;

            RightBandWidth.Text = "BandWidth";
            RightBandWidth.BandMin = 1;
            RightBandWidth.BandMax = 40;
            RightBandWidth.BandValue = 1;
            RightBandWidth.FooterText = "ABC";
            RightBandWidth.ShowBandValueInFooter = true;

            EQFilter = filter;
            leftGainDb.BandMax = MaxDB;
            leftGainDb.BandMin = -MaxDB;
            rightGainDb.BandMax = MaxDB;
            rightGainDb.BandMin = -MaxDB;
            leftGainDb.BandValue = (int)(EQFilter.Filters[0].GainDB / maxdb * leftGainDb.BandMax);
            rightGainDb.BandValue = (int)(EQFilter.Filters[1].GainDB / maxdb * rightGainDb.BandMax);
        }

        private void leftGainDb_ValueChanged(object sender, EventArgs e)
        {
            if (EQFilter != null)
            {
                double perc = leftGainDb.BandValue / (double)leftGainDb.BandMax;
                float value = (float)(perc * maxdb);
                EQFilter.Filters[0].GainDB = value;
            }
        }

        private void rightGainDb_ValueChanged(object sender, EventArgs e)
        {
            if (EQFilter!=null)
            {
                double perc = rightGainDb.BandValue / (double)rightGainDb.BandMax;
                float value = (float)(perc * maxdb);
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
