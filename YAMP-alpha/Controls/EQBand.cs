using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace YAMP_alpha.Controls
{
    public partial class EQBand : UserControl
    {
        public EQBand()
        {
            InitializeComponent();
        }

        public EQBand(string Header, int MinVal, int MaxVal, int Value, string Footer)
        {
            InitializeComponent();
            ValueChanged += EQBand_ValueChanged;
            EQBox.Text = Header;
            BandMin = MinVal;
            BandMax = MaxVal;
            bool flag = Value >= MinVal && Value < MaxVal;
            if (flag)
            {
                BandValue = Value;
            }
            else
            {
                BandValue = MinVal;
            }
            FooterText = Footer;
        }

        private void EQBand_ValueChanged(object sender, EventArgs e)
        {
            if (ShowBandValueInFooter)
            {
                FooterText = EQBandBar.Value.ToString();
            }
        }

        [Category("EQBand Settings")]
        [Browsable(true)]
        public int BandMin
        {
            get
            {
                return EQBandBar.Minimum;
            }
            set
            {
                EQBandBar.Minimum = value;
            }
        }

        [Category("EQBand Settings")]
        [Browsable(true)]
        public bool ShowBandValueInFooter { get; set; }

        [Category("EQBand Settings")]
        [Browsable(true)]
        public int BandMax
        {
            get
            {
                return EQBandBar.Maximum;
            }
            set
            {
                EQBandBar.Maximum = value;
            }
        }

        [Category("EQBand Settings")]
        [Browsable(true)]
        public int BandValue
        {
            get
            {
                return EQBandBar.Value;
            }
            set
            {
                EQBandBar.Value = value;
                OnValueChanged();
            }
        }

        [Category("EQBand Settings")]
        [Browsable(true)]
        public string FooterText
        {
            get
            {
                return EQFooter.Text;
            }
            set
            {
                EQFooter.Text = value;
            }
        }

        [Category("EQBand Settings")]
        [Browsable(true)]
        public TrackBar Tracker
        {
            get
            {
                return EQBandBar;
            }
        }

        [Category("EQBand Settings")]
        [Browsable(true)]
        public Panel MarginPanel
        {
            get
            {
                return EQSideMargin;
            }
        }

        [Category("EQBand Settings")]
        [Browsable(true)]
        public TickStyle TickStyle
        {
            get
            {
                return EQBandBar.TickStyle;
            }
            set
            {
                EQBandBar.TickStyle = value;
            }
        }

        [Category("EQBand Settings")]
        [Browsable(true)]
        public int TickFrequency
        {
            get
            {
                return EQBandBar.TickFrequency;
            }
            set
            {
                EQBandBar.TickFrequency = value;
            }
        }

        [Category("EQBand Settings")]
        [Browsable(true)]
        public bool FooterVisible
        {
            get
            {
                return EQFooter.Visible;
            }
            set
            {
                EQFooter.Visible = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return EQBox.Text;
            }
            set
            {
                EQBox.Text = value;
            }
        }

        [Browsable(true)]
        public event EventHandler ValueChanged;
        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        [Browsable(true)]
        public new event EventHandler TextChanged;
        private void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        private void EQBandBar_ValueChanged(object sender, EventArgs e)
        {
            BandValue = EQBandBar.Value;
        }

        private void EQBand_SizeChanged(object sender, EventArgs e)
        {
            EQSideMargin.Width = ClientRectangle.Width / 2 - 22;
        }

        private void EQBand_FocusChanged(object sender, EventArgs e)
        {
            Color _color = EQFooter.BackColor;
            EQFooter.BackColor = EQFooter.ForeColor;
            EQFooter.ForeColor = _color;
        }

        private void EQFooter_Click(object sender, EventArgs e)
        {
            EQBox.Focus();
        }

        private void EQBandBar_Enter(object sender, EventArgs e)
        {
            EQBox.Focus();
        }
    }
}
