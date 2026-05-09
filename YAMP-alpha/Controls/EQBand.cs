using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace YAMP_alpha.Controls
{

    [DefaultEvent("ValueChanged")]
    [DebuggerDisplay("Value = {BandValue}, Max = {BandMax}, Min = {BandMin}, Footer = {FooterText}")]
    public partial class EQBand : UserControl
    {
        /// <summary>
        /// A string holding the original or current baseline text for the footer, used to restore the text after temporarily showing the band value or “Locked” status.
        /// </summary>
        private string footervalue;

        /// <summary>
        /// A boolean flag tracking whether the user has locked the band to prevent changes.
        /// </summary>
        private bool BandLocked;

        /// <summary>
        /// Defines the visibility behavior of the footer.
        /// </summary>
        public enum FooterVisibility
        {
            Always,
            OnValueChange,
            Never
        }

        /// <summary>
        /// The backing field for the FooterVisibilityMode property.
        /// </summary>
        private FooterVisibility footerDisplayMode;

        /// <summary>
        /// Sets or retrieves the visibility rules for the footer (Always, OnValueChange, Never) and toggles the EQFooter.Visible status accordingly.
        /// </summary>
        [Category("EQBand Settings")]
        [Browsable(true)]
        public FooterVisibility FooterVisibilityMode
        {
            get { return footerDisplayMode; }
            set
            {
                footerDisplayMode = value;
                if (value == FooterVisibility.Always)
                {
                    EQFooter.Visible = true;
                }
                else if (value == FooterVisibility.Never || value == FooterVisibility.OnValueChange)
                {
                    EQFooter.Visible = false;
                }
            }
        }

        /// <summary>
        /// Defines the content displayed on the footer.
        /// </summary>
        public enum FooterContentSelect
        {
            CustomText,
            Value
        }

        private FooterContentSelect _footerContentMode;

        /// <summary>
        /// Determines whether the footer displays Custom Text or the Value permanently.
        /// </summary>
        [Category("EQBand Settings")]
        [Browsable(true)]
        public FooterContentSelect FooterContentMode
        {
            get { return _footerContentMode; }
            set 
            { 
                _footerContentMode = value; 
                UpdateFooterText();
            }
        }

        private void UpdateFooterText()
        {
            if (BandLocked)
            {
                FooterText = "Locked";
            }
            else if (FooterContentMode == FooterContentSelect.Value)
            {
                FooterText = BandValue.ToString();
            }
            else
            {
                FooterText = footervalue;
            }
        }

        /// <summary>
        /// Creates a new EQBand control.
        /// </summary>
        public EQBand()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the component and sets up the primary properties (header text, min/max values, current value, and footer text).
        /// </summary>
        public EQBand(string Header, int MinVal, int MaxVal, int Value, string Footer)
        {
            InitializeComponent();
            EQBox.Text = Header;
            BandMin = MinVal;
            BandMax = MaxVal;
            footervalue = Footer;
            FooterText = footervalue;
            bool flag = Value >= MinVal && Value < MaxVal;
            if (flag)
            {
                BandValue = Value;
            }
            else
            {
                BandValue = MinVal;
            }
        }

        /// <summary>
        /// The minimum limit for the underlying trackbar.
        /// </summary>
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

        /// <summary>
        /// Toggle indicating whether the footer should temporarily display the slider's numeric value while it is being adjusted.
        /// </summary>
        [Category("EQBand Settings")]
        [Browsable(true)]
        public bool ShowBandValueInFooter { get; set; }

        /// <summary>
        /// The maximum limit for the underlying trackbar.
        /// </summary>
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

        /// <summary>
        /// The current numeric value of the underlying trackbar.
        /// </summary>
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
            }
        }

        /// <summary>
        /// Gets or sets the text directly on the EQFooter label.
        /// </summary>
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
                if (!BandLocked && FooterContentMode == FooterContentSelect.CustomText && !showvaluetimer.Enabled && value != BandValue.ToString())
                {
                    footervalue = value;
                }
            }
        }

        /// <summary>
        /// Exposes the underlying TrackBar control for external access.
        /// </summary>
        [Category("EQBand Settings")]
        [Browsable(true)]
        public TrackBar Tracker
        {
            get
            {
                return EQBandBar;
            }
        }

        /// <summary>
        /// Exposes the side margin panel for external access.
        /// </summary>
        [Category("EQBand Settings")]
        [Browsable(true)]
        public Panel MarginPanel
        {
            get
            {
                return EQSideMargin;
            }
        }

        /// <summary>
        /// Controls the background color of the footer label.
        /// </summary>
        [Category("EQBand Settings")]
        [Browsable(true)]
        public Color FooterBackColor
        {
            get { return EQFooter.BackColor; }
            set { EQFooter.BackColor = value; }
        }

        /// <summary>
        /// Controls the text color of the footer label.
        /// </summary>
        [Category("EQBand Settings")]
        [Browsable(true)]
        public Color FooterForeColor
        {
            get { return EQFooter.ForeColor; }
            set { EQFooter.ForeColor = value; }
        }

        /// <summary>
        /// Directly gets or sets the visual state of the footer, shadowed by the new FooterVisibilityMode.
        /// </summary>
        [Category("EQBand Settings")]
        [Obsolete("Use FooterVisibilityMode instead.")]
        [Browsable(false)]
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

        /// <summary>
        /// Overrides the base UserControl.Text to instead map to the container's header.
        /// </summary>
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

        /// <summary>
        /// Triggered whenever the trackbar's value is modified.
        /// </summary>
        [Browsable(true)]
        public event EventHandler ValueChanged;

        /// <summary>
        /// Invokes the ValueChanged event. Also manages temporary text updates and visibility overrides.
        /// </summary>
        private void OnValueChanged()
        {            
            bool startTimer = false;

            if (FooterContentMode == FooterContentSelect.Value)
            {
                UpdateFooterText();
            }
            else if (ShowBandValueInFooter)
            {
                FooterText = BandValue.ToString();
                startTimer = true;
            }

            if (FooterVisibilityMode == FooterVisibility.OnValueChange || (ShowBandValueInFooter && FooterVisibilityMode == FooterVisibility.Never && FooterContentMode != FooterContentSelect.Value))
            {
                EQFooter.Visible = true;
                startTimer = true;
            }

            if (startTimer)
            {
                showvaluetimer.Stop();
                showvaluetimer.Start();
            }

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Triggered whenever the custom Text string changes.
        /// </summary>
        [Browsable(true)]
        public new event EventHandler TextChanged;

        /// <summary>
        /// Invokes the custom TextChanged event.
        /// </summary>
        private void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires when the trackbar moves and delegates work to OnValueChanged().
        /// </summary>
        private void EQBandBar_ValueChanged(object sender, EventArgs e)
        {
            OnValueChanged();
        }

        /// <summary>
        /// Dynamically calculates and adjusts the width of EQSideMargin whenever the control resizes to maintain centering.
        /// </summary>
        private void EQBand_SizeChanged(object sender, EventArgs e)
        {
            EQSideMargin.Width = ClientRectangle.Width / 2 - 22;
        }

        /// <summary>
        /// Inverts the footer's background and foreground colors when the component receives or loses focus.
        /// </summary>
        private void EQBand_FocusChanged(object sender, EventArgs e)
        {
            Color _color = EQFooter.BackColor;
            EQFooter.BackColor = EQFooter.ForeColor;
            EQFooter.ForeColor = _color;
        }

        /// <summary>
        /// Redirects input focus away from themselves and onto the top-level group box wrapper.
        /// </summary>
        private void EQFooter_Click(object sender, EventArgs e)
        {
            EQBox.Focus();
        }

        /// <summary>
        /// Redirects input focus away from themselves and onto the top-level group box wrapper.
        /// </summary>
        private void EQBandBar_Enter(object sender, EventArgs e)
        {
            EQBox.Focus();
        }

        /// <summary>
        /// Handles unlocking the band if it is currently locked; otherwise passes the double-click to the base component.
        /// </summary>
        private void EQFooter_DoubleClick(object sender, EventArgs e)
        {
            if (EQBandBar.Enabled) base.OnDoubleClick(e);
            else UnlockBand();
        }

        /// <summary>
        /// Disables the trackbar from use, recolors the footer red, and changes the footer text to "Locked".
        /// </summary>
        public void LockBand()
        {
            EQBandBar.Enabled = false;
            FooterBackColor = Color.Red;
            FooterForeColor = Color.White;
            footervalue = FooterText;
            BandLocked = true;
            FooterText = "Locked";
        }

        /// <summary>
        /// Re-enables the disabled trackbar.
        /// </summary>
        public void UnlockBand()
        {
            EQBandBar.Enabled = true;
            BandLocked = false;
            if (Focused)
            {
                FooterBackColor = SystemColors.Control;
                FooterForeColor = SystemColors.ControlText;
            }
            else
            {
                FooterBackColor = SystemColors.ControlText;
                FooterForeColor = SystemColors.Control;
            }
            UpdateFooterText();
        }

        /// <summary>
        /// Fired when the temporary value display timer expires. Restores the footer's original text, hides the footer if visibility is set to OnValueChange, and stops the timer.
        /// </summary>
        private void showvaluetimer_Tick(object sender, EventArgs e)
        {
            UpdateFooterText();
            if (FooterVisibilityMode == FooterVisibility.OnValueChange || (ShowBandValueInFooter && FooterVisibilityMode == FooterVisibility.Never && FooterContentMode != FooterContentSelect.Value))
            {
                EQFooter.Visible = false;
            }
            showvaluetimer.Stop();
        }
    }
}
