using CSCore.DSP;
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
        BiQuad[] Filters = null;
        List<BiQuad> FilterList = new List<BiQuad>();
        private bool Processing;

        public SignalFilterDialog()
        {
            InitializeComponent();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (YAMPVars.biQuadFilterSrc != null)
            {
                foreach (BiQuad filter in YAMPVars.biQuadFilterSrc.Filters)
                {
                    filter.Frequency = (double)nudInitFreq.Value;
                    filter.GainDB = Convert.ToInt32(nudInitGain.Value);
                }
                nudFreq.Value = nudInitFreq.Value;
                nudGain.Value = nudInitGain.Value;
                nudBndWdt.Value = nudInitBndWdt.Value;
            }
        }

        private void radioFilter_CheckedChanged(object sender, EventArgs e)
        {
            filterType = (AudioFilters.Filter)Convert.ToInt32((sender as RadioButton).Tag);
            nudFreq.Value = Convert.ToDecimal(Filters[(int)filterType].Frequency);
            nudGain.Value = Convert.ToDecimal(Filters[(int)filterType].GainDB);
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
                Filters[(int)filterType].Frequency = FreqVal;
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
                Filters[(int)filterType].GainDB = GainVal;
            }
        }

        private void BndWdt_ValueChanged(object sender, EventArgs e)
        {
            if (!Processing)
            {
                Processing = true;
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
                if (Filters != null)
                {
                    if (filterType == AudioFilters.Filter.Peak)
                    {
                        var PQ = Filters[(int)filterType] as PeakFilter;
                        PQ.BandWidth = Convert.ToDouble(nudBndWdt.Value);
                    }
                    else if (filterType == AudioFilters.Filter.Bell)
                    {
                        var PQ = Filters[(int)filterType] as BellFilter;
                        PQ.BandWidth = Convert.ToDouble(nudBndWdt.Value);
                    }
                    BndWdtTracker.Value = Convert.ToInt32(nudBndWdt.Value);
                }
                Processing = false;
            }
        }

        private void toggleFilterCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (YAMPVars.biQuadFilterSrc != null)
            {
                filterGroupBox.Enabled =
                InitParamGroupBox.Enabled =
                FltCtrGroupBox.Enabled =
                YAMPVars.biQuadFilterSrc.FilteringEnabled = toggleFilterCheck.Checked;
                if (toggleFilterCheck.Checked)
                {
                    if (Filters == null)
                    {
                        Filters = AudioFilters.SetAllFilters(44100, 600, 1, 1);
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbfilter = sender as CheckBox;
            int cbTag = Convert.ToInt32(cbfilter.Tag);
            RadioButton rbFilter = filterGroupBox.Controls.OfType<RadioButton>().First(x => Convert.ToInt32(x.Tag) == cbTag);
            if (cbfilter.Checked)
            {
                FilterList.Add(Filters[Convert.ToInt32(cbfilter.Tag)]);
                rbFilter.ForeColor = Color.White;
                rbFilter.BackColor = Color.Blue;
            }
            else
            {
                FilterList.Remove(Filters[Convert.ToInt32(cbfilter.Tag)]);
                rbFilter.ForeColor = Color.Black;
                rbFilter.BackColor = SystemColors.Control;
            }
            YAMPVars.biQuadFilterSrc.Filters = FilterList.ToArray();
        }
    }
}
