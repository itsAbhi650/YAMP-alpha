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

namespace YAMP_alpha
{
    public partial class GargleEffectDialog : Form
    {
        public GargleEffectDialog()
        {
            InitializeComponent();
        }
        internal DmoGargleEffect GargleEffect;

        public GargleEffectDialog(ref YAMP_Core YampCore)
        {
            InitializeComponent();
            if (YampCore!=null)
            {
                GargleEffect = YampCore.GargleEffect;
                updateInfo();
            }
            else
            {
                throw new NullReferenceException("YampCore");
            }
        }

        public GargleEffectDialog(ref DmoGargleEffect GargleEffectRef)
        {
            InitializeComponent();
            if (GargleEffect!=null)
            {
                GargleEffect = GargleEffectRef;
                updateInfo();
            }
            else
            {
                throw new NullReferenceException("GargleEffectRef");
            }
        }

        private void GargleEffectDialog_Load(object sender, EventArgs e)
        {
            trackBar1.Value = GargleEffect.RateHz;
            comboBox1.SelectedIndex = (int)GargleEffect.WaveShape;
            checkBox1.Checked = GargleEffect.IsEnabled;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            GargleEffect.RateHz = trackBar1.Value;
            updateInfo();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GargleEffect.WaveShape = (GargleWaveShape)comboBox1.SelectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            trackBar1.Value = trackBar1.Value - Convert.ToInt32(textBox1.Text);
            updateInfo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            trackBar1.Value = trackBar1.Value + Convert.ToInt32(textBox1.Text);
            updateInfo();
        }

        private void updateInfo()
        {
            label1.Text = string.Format("Change the value ({0} Hz) to observe a change in effect", GargleEffect.RateHz);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            GargleEffect.IsEnabled = checkBox1.Checked;
        }
    }
}
