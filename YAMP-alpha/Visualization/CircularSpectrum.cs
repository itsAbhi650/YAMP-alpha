using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using CSCore.DSP;

namespace YAMP_alpha
{
    /// <summary>
    /// Circular spectrum visualization with multiple style variations
    /// </summary>
    public class CircularSpectrum : SpectrumBase
    {
        private int _barCount;
        private float _barWidth;
        private Size _currentSize;
        private CircularSpectrumStyle _style;
        private int _innerRadius;
        private int _maxRadius;
        private Point _centerPoint;
        private float _rotation;
        private bool _enableRotation;
        private int _musicalRangeMinFreq = 60;  // Musical range minimum frequency
        private int _musicalRangeMaxFreq = 8000; // Musical range maximum frequency

        public CircularSpectrum(FftSize fftSize) : base()
        {
            FftSize = fftSize;
            _style = CircularSpectrumStyle.FullCircle;
            _innerRadius = 50;
            _barWidth = 3.0f;
            _rotation = 0f;
            _enableRotation = false;
        }

        /// <summary>
        /// Gets or sets the visual style of the circular spectrum
        /// </summary>
        public CircularSpectrumStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                RaisePropertyChanged("Style");
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
            }
        }

        /// <summary>
        /// Gets or sets the width of each bar in pixels
        /// </summary>
        public float BarWidth
        {
            get { return _barWidth; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _barWidth = value;
                RaisePropertyChanged("BarWidth");
            }
        }

        /// <summary>
        /// Gets or sets the inner radius (where bars start)
        /// </summary>
        public int InnerRadius
        {
            get { return _innerRadius; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _innerRadius = value;
                RaisePropertyChanged("InnerRadius");
            }
        }

        /// <summary>
        /// Gets or sets whether the visualization should rotate
        /// </summary>
        public bool EnableRotation
        {
            get { return _enableRotation; }
            set
            {
                _enableRotation = value;
                RaisePropertyChanged("EnableRotation");
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle in degrees (for animated rotation)
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value % 360f;
                RaisePropertyChanged("Rotation");
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
        /// Gets or sets the minimum frequency for musical range mode (default: 60Hz)
        /// </summary>
        public int MusicalRangeMinFrequency
        {
            get { return _musicalRangeMinFreq; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _musicalRangeMinFreq = value;
                RaisePropertyChanged("MusicalRangeMinFrequency");
            }
        }

        /// <summary>
        /// Gets or sets the maximum frequency for musical range mode (default: 8000Hz)
        /// </summary>
        public int MusicalRangeMaxFrequency
        {
            get { return _musicalRangeMaxFreq; }
            set
            {
                if (value <= _musicalRangeMinFreq)
                    throw new ArgumentOutOfRangeException("value");
                _musicalRangeMaxFreq = value;
                RaisePropertyChanged("MusicalRangeMaxFrequency");
            }
        }

        /// <summary>
        /// Creates a circular spectrum visualization
        /// </summary>
        public Bitmap CreateCircularSpectrum(Size size, Brush brush, Color background, bool highQuality)
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

                    // Calculate center point and maximum radius
                    _centerPoint = new Point(size.Width / 2, size.Height / 2);
                    _maxRadius = Math.Min(size.Width, size.Height) / 2 - 10; // Leave 10px margin

                    // Get spectrum points
                    int maxBarHeight = _maxRadius - _innerRadius;
                    SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(maxBarHeight, fftBuffer);

                    // For musical range mode, apply frequency filtering
                    if (_style == CircularSpectrumStyle.MusicalRange)
                    {
                        spectrumPoints = ApplyMusicalRangeFilter(spectrumPoints);
                    }

                    // Draw based on style
                    CreateCircularSpectrumInternal(graphics, brush, spectrumPoints);
                }

                return bitmap;
            }
            return null;
        }

        /// <summary>
        /// Creates a circular spectrum with gradient colors
        /// </summary>
        public Bitmap CreateCircularSpectrum(Size size, Color color1, Color color2, Color background, bool highQuality)
        {
            if (!UpdateFrequencyMappingIfNecessary(size))
                return null;

            // Create a radial-like gradient brush
            using (Brush brush = new LinearGradientBrush(
                new Rectangle(0, 0, size.Width, size.Height),
                color1,
                color2,
                LinearGradientMode.Vertical))
            {
                return CreateCircularSpectrum(size, brush, background, highQuality);
            }
        }

        private void CreateCircularSpectrumInternal(Graphics graphics, Brush brush, SpectrumPointData[] spectrumPoints)
        {
            if (spectrumPoints == null || spectrumPoints.Length == 0)
                return;

            switch (_style)
            {
                case CircularSpectrumStyle.FullCircle:
                    DrawFullCircle(graphics, brush, spectrumPoints, 0, 360);
                    break;

                case CircularSpectrumStyle.SemiCircle:
                    DrawFullCircle(graphics, brush, spectrumPoints, 180, 180);
                    break;

                case CircularSpectrumStyle.ThreeQuarterArc:
                    DrawFullCircle(graphics, brush, spectrumPoints, 135, 270);
                    break;

                case CircularSpectrumStyle.HalfArcBottom:
                    DrawFullCircle(graphics, brush, spectrumPoints, 180, 180);
                    break;

                case CircularSpectrumStyle.HalfArcTop:
                    DrawFullCircle(graphics, brush, spectrumPoints, 0, 180);
                    break;

                case CircularSpectrumStyle.MirrorMode:
                    DrawMirrorMode(graphics, brush, spectrumPoints);
                    break;

                case CircularSpectrumStyle.DualRing:
                    DrawDualRing(graphics, brush, spectrumPoints);
                    break;

                case CircularSpectrumStyle.QuarterArc:
                    DrawFullCircle(graphics, brush, spectrumPoints, 225, 90);
                    break;

                case CircularSpectrumStyle.SymmetricMirror:
                    DrawSymmetricMirror(graphics, brush, spectrumPoints);
                    break;

                case CircularSpectrumStyle.MusicalRange:
                    DrawFullCircle(graphics, brush, spectrumPoints, 0, 360);
                    break;
            }
        }

        private void DrawFullCircle(Graphics graphics, Brush brush, SpectrumPointData[] spectrumPoints, float startAngle, float sweepAngle)
        {
            if (spectrumPoints.Length == 0)
                return;

            float angleStep = sweepAngle / spectrumPoints.Length;
            float currentRotation = _enableRotation ? _rotation : 0f;

            using (Pen pen = new Pen(brush, _barWidth))
            {
                for (int i = 0; i < spectrumPoints.Length; i++)
                {
                    SpectrumPointData point = spectrumPoints[i];
                    float angle = (startAngle + currentRotation + (i * angleStep)) * (float)Math.PI / 180f; // Convert to radians
                    float barHeight = (float)point.Value;

                    // Calculate start point (at inner radius)
                    float x1 = _centerPoint.X + (float)(_innerRadius * Math.Cos(angle));
                    float y1 = _centerPoint.Y + (float)(_innerRadius * Math.Sin(angle));

                    // Calculate end point (at inner radius + bar height)
                    float radius = _innerRadius + barHeight;
                    float x2 = _centerPoint.X + (float)(radius * Math.Cos(angle));
                    float y2 = _centerPoint.Y + (float)(radius * Math.Sin(angle));

                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        private void DrawMirrorMode(Graphics graphics, Brush brush, SpectrumPointData[] spectrumPoints)
        {
            if (spectrumPoints.Length == 0)
                return;

            float angleStep = 360f / spectrumPoints.Length;
            float currentRotation = _enableRotation ? _rotation : 0f;
            int middleRadius = (_innerRadius + _maxRadius) / 2;

            using (Pen pen = new Pen(brush, _barWidth))
            {
                for (int i = 0; i < spectrumPoints.Length; i++)
                {
                    SpectrumPointData point = spectrumPoints[i];
                    float angle = (currentRotation + (i * angleStep)) * (float)Math.PI / 180f;
                    float halfBarHeight = (float)point.Value / 2f;

                    // Draw outward from middle
                    float x1 = _centerPoint.X + (float)(middleRadius * Math.Cos(angle));
                    float y1 = _centerPoint.Y + (float)(middleRadius * Math.Sin(angle));

                    float outerRadius = Math.Min(middleRadius + halfBarHeight, _maxRadius);
                    float x2 = _centerPoint.X + (float)(outerRadius * Math.Cos(angle));
                    float y2 = _centerPoint.Y + (float)(outerRadius * Math.Sin(angle));

                    graphics.DrawLine(pen, x1, y1, x2, y2);

                    // Draw inward from middle
                    float innerRadius = Math.Max(middleRadius - halfBarHeight, _innerRadius);
                    float x3 = _centerPoint.X + (float)(innerRadius * Math.Cos(angle));
                    float y3 = _centerPoint.Y + (float)(innerRadius * Math.Sin(angle));

                    graphics.DrawLine(pen, x1, y1, x3, y3);
                }
            }
        }

        private void DrawDualRing(Graphics graphics, Brush brush, SpectrumPointData[] spectrumPoints)
        {
            if (spectrumPoints.Length == 0)
                return;

            float angleStep = 360f / spectrumPoints.Length;
            float currentRotation = _enableRotation ? _rotation : 0f;
            
            // Split into two rings
            int quarterRadius = (_maxRadius - _innerRadius) / 4;
            int innerRingStart = _innerRadius;
            int innerRingMax = _innerRadius + quarterRadius;
            int outerRingStart = _innerRadius + 2 * quarterRadius;
            int outerRingMax = _maxRadius;

            using (Pen pen = new Pen(brush, _barWidth))
            {
                for (int i = 0; i < spectrumPoints.Length; i++)
                {
                    SpectrumPointData point = spectrumPoints[i];
                    float angle = (currentRotation + (i * angleStep)) * (float)Math.PI / 180f;
                    float barHeight = (float)point.Value;

                    // Inner ring
                    float x1Inner = _centerPoint.X + (float)(innerRingStart * Math.Cos(angle));
                    float y1Inner = _centerPoint.Y + (float)(innerRingStart * Math.Sin(angle));
                    
                    float innerRadius = Math.Min(innerRingStart + barHeight / 2f, innerRingMax);
                    float x2Inner = _centerPoint.X + (float)(innerRadius * Math.Cos(angle));
                    float y2Inner = _centerPoint.Y + (float)(innerRadius * Math.Sin(angle));

                    graphics.DrawLine(pen, x1Inner, y1Inner, x2Inner, y2Inner);

                    // Outer ring
                    float x1Outer = _centerPoint.X + (float)(outerRingStart * Math.Cos(angle));
                    float y1Outer = _centerPoint.Y + (float)(outerRingStart * Math.Sin(angle));
                    
                    float outerRadius = Math.Min(outerRingStart + barHeight / 2f, outerRingMax);
                    float x2Outer = _centerPoint.X + (float)(outerRadius * Math.Cos(angle));
                    float y2Outer = _centerPoint.Y + (float)(outerRadius * Math.Sin(angle));

                    graphics.DrawLine(pen, x1Outer, y1Outer, x2Outer, y2Outer);
                }
            }
        }

        /// <summary>
        /// Draws symmetric mirror mode - mirrors frequency data at 180° for balanced appearance
        /// </summary>
        private void DrawSymmetricMirror(Graphics graphics, Brush brush, SpectrumPointData[] spectrumPoints, float sweepAngle = 360f)
        {
            if (spectrumPoints.Length == 0)
                return;

            // Use only half the spectrum points and mirror them
            int halfCount = spectrumPoints.Length / 2;
            float angleStep = sweepAngle / (halfCount * 2); // Divide 360 by total points (mirrored)
            float currentRotation = _enableRotation ? _rotation : 0f;

            using (Pen pen = new Pen(brush, _barWidth))
            {
                // Draw first half (0° to 180°)
                for (int i = 0; i < halfCount; i++)
                {
                    SpectrumPointData point = spectrumPoints[i];
                    float angle = (currentRotation + (i * angleStep)) * (float)Math.PI / 180f;
                    float barHeight = (float)point.Value;

                    // Calculate and draw the bar
                    float x1 = _centerPoint.X + (float)(_innerRadius * Math.Cos(angle));
                    float y1 = _centerPoint.Y + (float)(_innerRadius * Math.Sin(angle));
                    float radius = _innerRadius + barHeight;
                    float x2 = _centerPoint.X + (float)(radius * Math.Cos(angle));
                    float y2 = _centerPoint.Y + (float)(radius * Math.Sin(angle));

                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }

                // Draw mirrored second half (180° to 360°)
                for (int i = 0; i < halfCount; i++)
                {
                    SpectrumPointData point = spectrumPoints[i]; // Use same data as first half
                    float angle = (currentRotation + 180f + (i * angleStep)) * (float)Math.PI / 180f; // Add 180° offset
                    float barHeight = (float)point.Value;

                    // Calculate and draw the mirrored bar
                    float x1 = _centerPoint.X + (float)(_innerRadius * Math.Cos(angle));
                    float y1 = _centerPoint.Y + (float)(_innerRadius * Math.Sin(angle));
                    float radius = _innerRadius + barHeight;
                    float x2 = _centerPoint.X + (float)(radius * Math.Cos(angle));
                    float y2 = _centerPoint.Y + (float)(radius * Math.Sin(angle));

                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        /// <summary>
        /// Filters spectrum points to only include frequencies within the musical range
        /// </summary>
        private SpectrumPointData[] ApplyMusicalRangeFilter(SpectrumPointData[] spectrumPoints)
        {
            if (spectrumPoints == null || spectrumPoints.Length == 0)
                return spectrumPoints;

            // Calculate frequency bounds as spectrum point indices
            int minIndex = GetSpectrumIndexForFrequency(_musicalRangeMinFreq);
            int maxIndex = GetSpectrumIndexForFrequency(_musicalRangeMaxFreq);

            // Filter points that fall within the musical range
            var filteredPoints = new System.Collections.Generic.List<SpectrumPointData>();
            
            for (int i = 0; i < spectrumPoints.Length; i++)
            {
                var point = spectrumPoints[i];
                // Check if this spectrum point index falls within our frequency range
                if (point.SpectrumPointIndex >= minIndex && point.SpectrumPointIndex <= maxIndex)
                {
                    filteredPoints.Add(point);
                }
            }

            return filteredPoints.ToArray();
        }

        /// <summary>
        /// Converts a frequency (Hz) to approximate spectrum point index
        /// </summary>
        private int GetSpectrumIndexForFrequency(float frequency)
        {
            if (SpectrumProvider == null)
                return 0;

            // Get the FFT band index for this frequency
            int fftBandIndex = SpectrumProvider.GetFftBandIndex(frequency);
            
            // Convert to spectrum point index based on ratio
            double ratio = (double)fftBandIndex / ((int)FftSize / 2);
            return (int)(ratio * SpectrumResolution);
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
