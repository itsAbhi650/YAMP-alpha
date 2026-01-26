using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using CSCore.DSP;

namespace YAMP_alpha
{
    /// <summary>
    /// Horizontal bar spectrum visualization - displays frequency bars in configurable directions
    /// </summary>
    public class HorizontalBarSpectrum : SpectrumBase
    {
        private int _barCount;
        private double _barSpacing;
        private double _barHeight;  // Note: This represents bar thickness (width or height depending on direction)
        private Size _currentSize;
        private bool _showPeakIndicators;
        private Color _peakIndicatorColor;
        private BarSpectrumRenderDirection _renderDirection;
        private double[] _scaledPeakValues;  // Track peaks of SCALED bar values (not raw FFT)
        private int _framesSinceLastPeak;
        private int _peakHoldFrames = 15;
        private float _peakDecayRate = 0.95f;
        private PeakHoldMode _peakMode = PeakHoldMode.FallingPeak;

        public HorizontalBarSpectrum(FftSize fftSize) : base()
        {
            FftSize = fftSize;
            _showPeakIndicators = false;
            _peakIndicatorColor = Color.Red;
            _renderDirection = BarSpectrumRenderDirection.VerticalBottomToTop; // Default
        }

        /// <summary>
        /// Gets or sets the peak hold mode for peak indicators
        /// </summary>
        public PeakHoldMode PeakMode
        {
            get { return _peakMode; }
            set
            {
                _peakMode = value;
                RaisePropertyChanged("PeakMode");
            }
        }

        /// <summary>
        /// Gets or sets the number of frames to hold peaks before decay
        /// </summary>
        public int PeakHoldFrames
        {
            get { return _peakHoldFrames; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _peakHoldFrames = value;
                RaisePropertyChanged("PeakHoldFrames");
            }
        }

        /// <summary>
        /// Gets or sets the peak decay rate (0.5 to 1.0)
        /// </summary>
        public float PeakDecayRate
        {
            get { return _peakDecayRate; }
            set
            {
                if (value < 0.5f || value > 1.0f)
                    throw new ArgumentOutOfRangeException("value", "Must be between 0.5 and 1.0");
                _peakDecayRate = value;
                RaisePropertyChanged("PeakDecayRate");
            }
        }

        /// <summary>
        /// Gets or sets the rendering direction for the spectrum bars
        /// </summary>
        public BarSpectrumRenderDirection RenderDirection
        {
            get { return _renderDirection; }
            set
            {
                _renderDirection = value;
                // Need to recalculate bar sizing when direction changes
                if (CurrentSize.Width > 0 && CurrentSize.Height > 0)
                {
                    UpdateFrequencyMapping();
                }
                RaisePropertyChanged("RenderDirection");
            }
        }

        /// <summary>
        /// Gets the calculated width of each bar (thickness)
        /// </summary>
        [Browsable(false)]
        public double BarHeight
        {
            get { return _barHeight; }
        }

        /// <summary>
        /// Gets the calculated width of each bar (thickness)
        /// </summary>
        [Browsable(false)]
        public double BarWidth
        {
            get { return _barHeight; }  // In vertical mode, _barHeight is actually width
        }

        /// <summary>
        /// Gets or sets the spacing between bars in pixels
        /// </summary>
        public double BarSpacing
        {
            get { return _barSpacing; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _barSpacing = value;
                UpdateFrequencyMapping();

                RaisePropertyChanged("BarSpacing");
                RaisePropertyChanged("BarHeight");
            }
        }

        /// <summary>
        /// Gets or sets the number of frequency bars
        /// </summary>
        public int BarCount
        {
            get { return _barCount; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _barCount = value;
                SpectrumResolution = value;
                UpdateFrequencyMapping();

                RaisePropertyChanged("BarCount");
                RaisePropertyChanged("BarHeight");
            }
        }

        /// <summary>
        /// Gets or sets whether to show peak indicators
        /// </summary>
        public bool ShowPeakIndicators
        {
            get { return _showPeakIndicators; }
            set
            {
                _showPeakIndicators = value;
                RaisePropertyChanged("ShowPeakIndicators");
            }
        }

        /// <summary>
        /// Gets or sets the color for peak indicators
        /// </summary>
        public Color PeakIndicatorColor
        {
            get { return _peakIndicatorColor; }
            set
            {
                _peakIndicatorColor = value;
                RaisePropertyChanged("PeakIndicatorColor");
            }
        }

        [BrowsableAttribute(false)]
        public Size CurrentSize
        {
            get { return _currentSize; }
            protected set
            {
                _currentSize = value;
                RaisePropertyChanged("CurrentSize");
            }
        }

        /// <summary>
        /// Creates a horizontal bar spectrum with solid brush
        /// </summary>
        public Bitmap CreateHorizontalBarSpectrum(Size size, Brush brush, Color background, bool highQuality)
        {
            if (!UpdateFrequencyMappingIfNecessary(size))
                return null;

            var fftBuffer = new float[(int)FftSize];

            if (SpectrumProvider.GetFftData(fftBuffer, this))
            {
                var bitmap = new Bitmap(size.Width, size.Height);

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    PrepareGraphics(graphics, highQuality);
                    graphics.Clear(background);

                    CreateHorizontalBarSpectrumInternal(graphics, brush, fftBuffer, size);
                }

                return bitmap;
            }
            return null;
        }

        /// <summary>
        /// Creates a horizontal bar spectrum with gradient colors
        /// </summary>
        public Bitmap CreateHorizontalBarSpectrum(Size size, Color color1, Color color2, Color background, bool highQuality)
        {
            if (!UpdateFrequencyMappingIfNecessary(size))
                return null;

            // Create horizontal gradient from left to right
            using (Brush brush = new LinearGradientBrush(
                new Rectangle(0, 0, size.Width, size.Height),
                color1,
                color2,
                LinearGradientMode.Horizontal))
            {
                return CreateHorizontalBarSpectrum(size, brush, background, highQuality);
            }
        }

        private void CreateHorizontalBarSpectrumInternal(Graphics graphics, Brush brush, float[] fftBuffer, Size size)
        {
            int width = size.Width;
            int height = size.Height;
            
            // Determine which dimension to use for bar values based on render direction
            bool isVertical = _renderDirection == BarSpectrumRenderDirection.VerticalBottomToTop || 
                             _renderDirection == BarSpectrumRenderDirection.VerticalTopToBottom;
            int maxBarValue = isVertical ? height : width;
            
            // Prepare the FFT result for rendering (this applies scaling, averaging, log scale, etc.)
            SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(maxBarValue, fftBuffer);

            // Initialize or resize peak tracking array
            if (_scaledPeakValues == null || _scaledPeakValues.Length != BarCount)
            {
                _scaledPeakValues = new double[BarCount];
            }

            // Increment frame counter for peak decay
            _framesSinceLastPeak++;

            // Draw the bars and update peaks based on render direction
            for (int i = 0; i < spectrumPoints.Length; i++)
            {
                SpectrumPointData point = spectrumPoints[i];
                int barIndex = point.SpectrumPointIndex;
                double barValue = point.Value;
                
                // Update peak tracking for this bar (uses SCALED bar value, not raw FFT)
                UpdateScaledPeak(barIndex, barValue);
                
                RectangleF barRect;
                
                switch (_renderDirection)
                {
                    case BarSpectrumRenderDirection.HorizontalLeftToRight:
                        barRect = CreateHorizontalLeftToRightBar(barIndex, barValue, width, height);
                        break;
                        
                    case BarSpectrumRenderDirection.HorizontalRightToLeft:
                        barRect = CreateHorizontalRightToLeftBar(barIndex, barValue, width, height);
                        break;
                        
                    case BarSpectrumRenderDirection.VerticalBottomToTop:
                        barRect = CreateVerticalBottomToTopBar(barIndex, barValue, width, height);
                        break;
                        
                    case BarSpectrumRenderDirection.VerticalTopToBottom:
                        barRect = CreateVerticalTopToBottomBar(barIndex, barValue, width, height);
                        break;
                        
                    default:
                        barRect = CreateVerticalBottomToTopBar(barIndex, barValue, width, height);
                        break;
                }

                graphics.FillRectangle(brush, barRect);

                // Draw peak indicator if enabled (uses scaled peak value that matches bar height)
                if (_showPeakIndicators && barIndex < _scaledPeakValues.Length)
                {
                    DrawScaledPeakIndicator(graphics, barIndex, _scaledPeakValues[barIndex], width, height);
                }
            }
        }

        private RectangleF CreateHorizontalLeftToRightBar(int barIndex, double barValue, int width, int height)
        {
            // Bars grow horizontally from left edge
            double yCoord = BarSpacing * (barIndex + 1) + (_barHeight * barIndex);
            
            return new RectangleF(
                0,                          // X: Start at left edge
                (float)yCoord,              // Y: Vertical position
                (float)barValue,            // Width: Bar length (grows right)
                (float)_barHeight           // Height: Bar thickness
            );
        }

        private RectangleF CreateHorizontalRightToLeftBar(int barIndex, double barValue, int width, int height)
        {
            // Bars grow horizontally from right edge
            double yCoord = BarSpacing * (barIndex + 1) + (_barHeight * barIndex);
            
            return new RectangleF(
                width - (float)barValue,    // X: Start from right - bar length
                (float)yCoord,              // Y: Vertical position
                (float)barValue,            // Width: Bar length (grows left)
                (float)_barHeight           // Height: Bar thickness
            );
        }

        private RectangleF CreateVerticalBottomToTopBar(int barIndex, double barValue, int width, int height)
        {
            // Bars grow vertically from bottom edge
            double xCoord = BarSpacing * (barIndex + 1) + (_barHeight * barIndex);
            
            return new RectangleF(
                (float)xCoord,                  // X: Horizontal position
                height - (float)barValue,       // Y: Bottom minus bar height
                (float)_barHeight,              // Width: Bar thickness
                (float)barValue                 // Height: Bar length (grows up)
            );
        }

        private RectangleF CreateVerticalTopToBottomBar(int barIndex, double barValue, int width, int height)
        {
            // Bars grow vertically from top edge
            double xCoord = BarSpacing * (barIndex + 1) + (_barHeight * barIndex);
            
            return new RectangleF(
                (float)xCoord,                  // X: Horizontal position
                0,                              // Y: Start at top edge
                (float)_barHeight,              // Width: Bar thickness
                (float)barValue                 // Height: Bar length (grows down)
            );
        }

        /// <summary>
        /// Updates peak value for a specific bar using the SCALED bar value
        /// </summary>
        private void UpdateScaledPeak(int barIndex, double currentScaledValue)
        {
            if (barIndex < 0 || barIndex >= _scaledPeakValues.Length)
                return;

            // If current bar is taller than peak, update peak
            if (currentScaledValue > _scaledPeakValues[barIndex])
            {
                _scaledPeakValues[barIndex] = currentScaledValue;
            }
            else
            {
                // Apply decay based on mode
                switch (_peakMode)
                {
                    case PeakHoldMode.NeverFall:
                        // Peaks never decay - stay at maximum forever
                        break;

                    case PeakHoldMode.FallingPeak:
                        // Standard falling peak with hold time
                        if (_framesSinceLastPeak > _peakHoldFrames)
                        {
                            _scaledPeakValues[barIndex] *= _peakDecayRate;
                            
                            if (_scaledPeakValues[barIndex] < 1.0)
                                _scaledPeakValues[barIndex] = 0;
                        }
                        break;

                    case PeakHoldMode.InstantFall:
                        // Peaks immediately follow bar height (no hold)
                        _scaledPeakValues[barIndex] = currentScaledValue;
                        break;
                }
            }
        }

        /// <summary>
        /// Draws peak indicator using pre-scaled peak value (already in pixel coordinates)
        /// </summary>
        private void DrawScaledPeakIndicator(Graphics graphics, int barIndex, double peakValue, int width, int height)
        {
            if (peakValue <= 2) return; // Only draw if visible

            using (Pen peakPen = new Pen(_peakIndicatorColor, 2.0f))
            {
                switch (_renderDirection)
                {
                    case BarSpectrumRenderDirection.HorizontalLeftToRight:
                    case BarSpectrumRenderDirection.HorizontalRightToLeft:
                        // Draw vertical line for horizontal bars
                        double yCoord = BarSpacing * (barIndex + 1) + (_barHeight * barIndex);
                        float peakX = (_renderDirection == BarSpectrumRenderDirection.HorizontalLeftToRight) 
                            ? (float)peakValue 
                            : width - (float)peakValue;
                        graphics.DrawLine(peakPen, peakX, (float)yCoord, peakX, (float)(yCoord + _barHeight));
                        break;

                    case BarSpectrumRenderDirection.VerticalBottomToTop:
                    case BarSpectrumRenderDirection.VerticalTopToBottom:
                        // Draw horizontal line for vertical bars
                        double xCoord = BarSpacing * (barIndex + 1) + (_barHeight * barIndex);
                        float peakY = (_renderDirection == BarSpectrumRenderDirection.VerticalBottomToTop)
                            ? height - (float)peakValue
                            : (float)peakValue;
                        graphics.DrawLine(peakPen, (float)xCoord, peakY, (float)(xCoord + _barHeight), peakY);
                        break;
                }
            }
        }

        /// <summary>
        /// Resets all peak values to zero
        /// </summary>
        public void ResetPeaks()
        {
            if (_scaledPeakValues != null)
            {
                Array.Clear(_scaledPeakValues, 0, _scaledPeakValues.Length);
            }
            _framesSinceLastPeak = 0;
        }

        protected override void UpdateFrequencyMapping()
        {
            // Calculate bar thickness based on render direction
            bool isVertical = _renderDirection == BarSpectrumRenderDirection.VerticalBottomToTop || 
                             _renderDirection == BarSpectrumRenderDirection.VerticalTopToBottom;
            
            if (isVertical)
            {
                // Vertical bars: distribute across width
                _barHeight = Math.Max(((_currentSize.Width - (BarSpacing * (BarCount + 1))) / BarCount), 0.00001);
            }
            else
            {
                // Horizontal bars: distribute across height
                _barHeight = Math.Max(((_currentSize.Height - (BarSpacing * (BarCount + 1))) / BarCount), 0.00001);
            }
            
            base.UpdateFrequencyMapping();
        }

        private bool UpdateFrequencyMappingIfNecessary(Size newSize)
        {
            if (newSize != CurrentSize)
            {
                CurrentSize = newSize;
                UpdateFrequencyMapping();
            }

            return newSize.Width > 0 && newSize.Height > 0;
        }

        private void PrepareGraphics(Graphics graphics, bool highQuality)
        {
            if (highQuality)
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.AssumeLinear;
                graphics.PixelOffsetMode = PixelOffsetMode.Default;
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            }
            else
            {
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.None;
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            }
        }
    }
}
