using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace YAMP_alpha.Controls
{

    [DefaultEvent("ValueChanged")]
    [DebuggerDisplay("Level = {Level}, Maximum = {Maximum}")]
    public partial class MeterControl : UserControl
    {
        private int _ledsize = 1;
        private int _maximum = 1;
        private Color _ledcolor = Color.Lime;
        private int _level = 0;
        
        // Smooth animation variables
        private float _smoothLevel = 0f;
        private float _peakLevel = 0f;
        private int _peakHoldCounter = 0;
        private Timer _animationTimer;
        
        // Animation settings
        private const float ATTACK_SPEED = 0.8f;  // How fast meter rises (0-1, higher = faster)
        private const float DECAY_SPEED = 0.15f;  // How fast meter falls (0-1, higher = faster)
        private const int PEAK_HOLD_TIME = 20;    // How long peak stays before falling (in timer ticks)
        private const float PEAK_FALL_SPEED = 0.01f; // How fast peak cap falls

        public MeterControl()
        {
            InitializeComponent();
            MeterBox.Paint += MeterBox_Paint;
            
            // Initialize animation timer
            _animationTimer = new Timer();
            _animationTimer.Interval = 16; // ~60 FPS
            _animationTimer.Tick += AnimationTimer_Tick;
            _animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            bool needsRedraw = false;
            float targetLevel = (float)_level / _maximum;
            
            // Smooth level animation with attack/decay
            if (_smoothLevel < targetLevel)
            {
                // Attack (rising)
                _smoothLevel += (targetLevel - _smoothLevel) * ATTACK_SPEED;
                if (Math.Abs(_smoothLevel - targetLevel) < 0.001f)
                    _smoothLevel = targetLevel;
                needsRedraw = true;
            }
            else if (_smoothLevel > targetLevel)
            {
                // Decay (falling)
                _smoothLevel += (targetLevel - _smoothLevel) * DECAY_SPEED;
                if (Math.Abs(_smoothLevel - targetLevel) < 0.001f)
                    _smoothLevel = targetLevel;
                needsRedraw = true;
            }
            
            // Peak cap logic
            if (_smoothLevel > _peakLevel)
            {
                // New peak reached
                _peakLevel = _smoothLevel;
                _peakHoldCounter = PEAK_HOLD_TIME;
                needsRedraw = true;
            }
            else if (_peakHoldCounter > 0)
            {
                // Hold peak
                _peakHoldCounter--;
                needsRedraw = true;
            }
            else if (_peakLevel > _smoothLevel)
            {
                // Fall peak
                _peakLevel -= PEAK_FALL_SPEED;
                if (_peakLevel < _smoothLevel)
                    _peakLevel = _smoothLevel;
                needsRedraw = true;
            }
            
            if (needsRedraw)
            {
                MeterBox.Invalidate();
            }
        }

        private void MeterBox_Paint(object sender, PaintEventArgs e)
        {
            DrawMeter(e.Graphics);
        }

        [Category("Meter Settings")]
        [Browsable(true)]
        public int Maximum 
        {
            get { return _maximum; }
            set 
            {
                if (_maximum != value)
                {
                    _maximum = value;
                    MeterBox.Invalidate();
                }
            } 
        }

        [Category("Meter Settings")]
        [Browsable(true)]
        public int LEDSize 
        {
            get { return _ledsize; }
            set 
            {
                if (_ledsize != value)
                {
                    _ledsize = value;
                    MeterBox.Invalidate();
                }
            } 
        }

        [Category("Meter Settings")]
        [Browsable(true)]
        public Color LEDColor
        {
            get { return _ledcolor; }
            set 
            {
                if (_ledcolor != value)
                {
                    _ledcolor = value;
                    MeterBox.Invalidate();
                }
            }
        }

        [Category("Meter Settings")]
        [Browsable(true)]
        public Color BackgroundColor
        {
            get { return MeterBox.BackColor; }
            set { MeterBox.BackColor = value; }
        }

        [Category("Meter Settings")]
        [Browsable(true)]
        public int Level
        {
            get { return _level; }
            set
            {
                if (_level != value)
                {
                    _level = value;
                    OnValueChanged();
                }
            }
        }

        [Browsable(true)]
        public event EventHandler ValueChanged;
        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DrawMeter(Graphics g)
        {
            g.Clear(BackgroundColor);
            
            int pbWidth = MeterBox.ClientSize.Width;
            int pbHeight = MeterBox.ClientSize.Height;

            int spacing = 2;
            int ledCount = pbHeight / (_ledsize + spacing);
            ledCount = Math.Max(1, ledCount);

            int activeLEDs = (int)(_smoothLevel * ledCount);
            int peakLED = (int)(_peakLevel * ledCount);
            
            int totalUsedHeight = ledCount * (_ledsize + spacing) - spacing;

            int startY = (pbHeight - totalUsedHeight) / 2;
            int ledWidth = pbWidth / 2;
            int startX = (pbWidth - ledWidth) / 2;

            // Draw meter LEDs
            for (int i = 0; i < ledCount; i++)
            {
                int y = startY + (ledCount - 1 - i) * (_ledsize + spacing);
                
                if (i < activeLEDs)
                {
                    // Active LED with color gradient (green -> yellow -> red)
                    Color ledColor = GetLEDColor(i, ledCount);
                    using (Brush br = new SolidBrush(ledColor))
                    {
                        g.FillRectangle(br, startX, y, ledWidth, _ledsize);
                    }
                }
                else if (i == peakLED && peakLED >= activeLEDs)
                {
                    // Peak cap indicator (brighter color)
                    Color peakColor = GetPeakCapColor(i, ledCount);
                    using (Brush br = new SolidBrush(peakColor))
                    {
                        g.FillRectangle(br, startX, y, ledWidth, _ledsize);
                    }
                }
            }
        }

        private Color GetLEDColor(int ledIndex, int totalLEDs)
        {
            // Create gradient: green (bottom) -> yellow (middle) -> red (top)
            float position = (float)ledIndex / totalLEDs;
            
            if (position < 0.6f)
            {
                // Green zone
                return _ledcolor;
            }
            else if (position < 0.85f)
            {
                // Yellow zone (transition)
                return Color.Yellow;
            }
            else
            {
                // Red zone (peak warning)
                return Color.Red;
            }
        }

        private Color GetPeakCapColor(int ledIndex, int totalLEDs)
        {
            // Peak cap matches the zone color but brighter
            float position = (float)ledIndex / totalLEDs;
            
            if (position < 0.6f)
            {
                // Normal zone - use the configured LED color at full brightness
                // This respects the user's LEDColor property (DeepSkyBlue, Lime, etc.)
                return BrightenColor(_ledcolor);
            }
            else if (position < 0.85f)
            {
                // Yellow zone (transition)
                return Color.FromArgb(255, 255, 255, 0); // Bright yellow
            }
            else
            {
                // Red zone (peak warning)
                return Color.FromArgb(255, 255, 0, 0); // Bright red
            }
        }

        private Color BrightenColor(Color originalColor)
        {
            // Make the color brighter by ensuring full alpha and increasing RGB values
            // while maintaining the color hue
            int r = Math.Min(255, originalColor.R + (255 - originalColor.R) / 2);
            int g = Math.Min(255, originalColor.G + (255 - originalColor.G) / 2);
            int b = Math.Min(255, originalColor.B + (255 - originalColor.B) / 2);
            
            return Color.FromArgb(255, r, g, b);
        }

        private void MeterControl_SizeChanged(object sender, EventArgs e)
        {
            MeterBox.Invalidate();
        }
    }
}
