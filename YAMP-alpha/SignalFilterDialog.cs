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
    public partial class SignalFilterDialog : Form
    {
        AudioFilters.Filter filterType;

        public SignalFilterDialog()
        {
            InitializeComponent();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (YAMPVars.biQuadFilterSrc != null)
            {
                //YAMPVars.biQuadFilterSrc.Filters = AudioFilters.GetFilters(YAMPVars.CORE.PlayerSource.WaveFormat.SampleRate, 600, 0, 1);
                foreach (CSCore.DSP.BiQuad filter in YAMPVars.biQuadFilterSrc.Filters)
                {
                    filter.Frequency = (double)nudInitFreq.Value;
                    filter.GainDB = Convert.ToInt32(nudInitGain.Value);
                    //filter.Q = (double)nudBndWdt.Value;
                }
                nudFreq.Value = nudInitFreq.Value;
                nudGain.Value = nudInitGain.Value;
                nudBndWdt.Value = nudInitBndWdt.Value;
            }
        }

        private void radioFilter_CheckedChanged(object sender, EventArgs e)
        {

            filterType = (AudioFilters.Filter)Convert.ToInt32((sender as RadioButton).Tag);
            if (YAMPVars.biQuadFilterSrc != null)
            {
                nudBndWdt.Enabled = false;
                BndWdtTracker.Enabled = false;
                nudGain.Enabled = false;
                gainTracker.Enabled = false;
                switch (filterType)
                {
                    case AudioFilters.Filter.HighShelf:
                        nudGain.Enabled = true;
                        gainTracker.Enabled = true;
                        break;
                    case AudioFilters.Filter.LowShelf:
                        nudGain.Enabled = true;
                        gainTracker.Enabled = true;
                        break;
                    case AudioFilters.Filter.Peak:
                        nudBndWdt.Value = Convert.ToDecimal((YAMPVars.biQuadFilterSrc.Filters[(int)filterType] as CSCore.DSP.PeakFilter).BandWidth);
                        nudGain.Enabled = true;
                        gainTracker.Enabled = true;
                        nudBndWdt.Enabled = true;
                        BndWdtTracker.Enabled = true;
                        break;
                }
                nudFreq.Value = Convert.ToDecimal(YAMPVars.biQuadFilterSrc.Filters[(int)filterType].Frequency);
                nudGain.Value = Convert.ToDecimal(YAMPVars.biQuadFilterSrc.Filters[(int)filterType].GainDB);
            }

        }

        private void Freq_ValueChanged(object sender, EventArgs e)
        {
            double FreqVal;
            if (sender.GetType() == typeof(NumericUpDown))
            {
                FreqVal = Convert.ToDouble(((NumericUpDown)sender).Value);
                freqTracker.Value = (int)FreqVal;
            }
            else
            {
                FreqVal = Convert.ToDouble(((TrackBar)sender).Value);
                nudFreq.Value = (decimal)FreqVal;
            }
            if (YAMPVars.biQuadFilterSrc.Filters != null)
            {
                YAMPVars.biQuadFilterSrc.Filters[(int)filterType].Frequency = FreqVal;
            }
        }

        private void Gain_ValueChanged(object sender, EventArgs e)
        {
            int GainVal;
            if (sender.GetType() == typeof(NumericUpDown))
            {
                GainVal = Convert.ToInt32(((NumericUpDown)sender).Value);
                gainTracker.Value = GainVal;
            }
            else
            {
                GainVal = Convert.ToInt32(((TrackBar)sender).Value);
                nudGain.Value = GainVal;
            }
            if (YAMPVars.biQuadFilterSrc.Filters != null)
            {
                YAMPVars.biQuadFilterSrc.Filters[(int)filterType].GainDB = GainVal;
            }
        }

        private void BndWdt_ValueChanged(object sender, EventArgs e)
        {
            int Bandwidth;
            if (sender.GetType() == typeof(NumericUpDown))
            {
                Bandwidth = Convert.ToInt32(((NumericUpDown)sender).Value);
                BndWdtTracker.Value = Bandwidth;
            }
            else
            {
                Bandwidth = Convert.ToInt32(((TrackBar)sender).Value);
                nudBndWdt.Value = Bandwidth;
            }
            if (YAMPVars.biQuadFilterSrc.Filters != null)
            {
                if (filterType == AudioFilters.Filter.Peak)
                {
                    var PQ = YAMPVars.biQuadFilterSrc.Filters[(int)filterType] as CSCore.DSP.PeakFilter;
                    PQ.BandWidth = Convert.ToDouble(nudBndWdt.Value);
                    BndWdtTracker.Value = Convert.ToInt32(nudBndWdt.Value);

                }
            }
        }

        private void SignalFilterDialog_Load(object sender, EventArgs e)
        {
            
        }

        private void toggleFilterCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (YAMPVars.biQuadFilterSrc != null)
            {
                filterGroupBox.Enabled =
                InitParamGroupBox.Enabled =
                FltCtrGroupBox.Enabled =
                YAMPVars.biQuadFilterSrc.FilteringEnabled = toggleFilterCheck.Checked;
            }
        }
    }
}
