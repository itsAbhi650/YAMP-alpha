using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace YAMP_alpha
{
    /// <summary>
    /// Modern waveform visualization with multiple display styles, frequency filtering,
    /// smooth decay, and efficient CPU usage through sample pooling and bitmap caching.
    /// </summary>
    public class ModernWaveformSpectrum : IDisposable
    {
        #region Constants

        private const int DefaultBufferSize = 4096;
        private const int MaxRenderResolution = 512;

        #endregion

        #region Private Fields

        // Sample buffers - circular buffer
        private readonly float[] _leftBuffer;
        private readonly float[] _rightBuffer;
        private int _bufferWriteIndex;
        private int _sampleCount;
        private readonly object _lockObj = new object();

        // Decay state - persists between frames for smooth animation
        private float[] _decayLeftSamples;
        private float[] _decayRightSamples;

        // Simple low-pass filter state
        private float _lpfLeftState;
        private float _lpfRightState;

        // Cached brushes and pens
        private Brush _leftChannelBrush;
        private Brush _rightChannelBrush;
        private Brush _backgroundBrush;
        private Pen _leftChannelPen;
        private Pen _rightChannelPen;
        private Pen _centerLinePen;
        private Pen _gridPen;
        private bool _brushesDirty = true;

        // Cached bitmap for reuse
        private Bitmap _cachedBitmap;
        private Graphics _cachedGraphics;

        // Cached sample arrays
        private float[] _cachedLeftSamples;
        private float[] _cachedRightSamples;

        // Cached point arrays
        private PointF[] _cachedPoints1;
        private PointF[] _cachedPoints2;

        // Last rendered size
        private Size _lastSize;

        // Sample rate for frequency calculations (default 44100 Hz)
        private int _sampleRate = 44100;

        #endregion

        #region Public Properties

        /// <summary>
        /// Waveform display style
        /// </summary>
        public WaveformStyle Style { get; set; } = WaveformStyle.FilledMirror;

        /// <summary>
        /// Left channel color
        /// </summary>
        public Color LeftChannelColor
        {
            get => _leftChannelColor;
            set { _leftChannelColor = value; _brushesDirty = true; }
        }
        private Color _leftChannelColor = Color.FromArgb(0, 200, 255);

        /// <summary>
        /// Right channel color
        /// </summary>
        public Color RightChannelColor
        {
            get => _rightChannelColor;
            set { _rightChannelColor = value; _brushesDirty = true; }
        }
        private Color _rightChannelColor = Color.FromArgb(255, 100, 150);

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; _brushesDirty = true; }
        }
        private Color _backgroundColor = Color.Black;

        /// <summary>
        /// Which channels to render
        /// </summary>
        public WaveformChannel RenderChannel { get; set; } = WaveformChannel.Both;

        /// <summary>
        /// Enable glow/neon effect
        /// </summary>
        public bool EnableGlow { get; set; } = false;

        /// <summary>
        /// Enable gradient fill for filled styles
        /// </summary>
        public bool EnableGradientFill { get; set; } = true;

        /// <summary>
        /// Show center line
        /// </summary>
        public bool ShowCenterLine { get; set; } = true;

        /// <summary>
        /// Center line color
        /// </summary>
        public Color CenterLineColor
        {
            get => _centerLineColor;
            set { _centerLineColor = value; _brushesDirty = true; }
        }
        private Color _centerLineColor = Color.FromArgb(80, 255, 255, 255);

        /// <summary>
        /// Show grid lines
        /// </summary>
        public bool ShowGrid { get; set; } = false;

        /// <summary>
        /// Grid color
        /// </summary>
        public Color GridColor
        {
            get => _gridColor;
            set { _gridColor = value; _brushesDirty = true; }
        }
        private Color _gridColor = Color.FromArgb(30, 255, 255, 255);

        /// <summary>
        /// Line thickness for line-based styles
        /// </summary>
        public float LineThickness { get; set; } = 2.0f;

        /// <summary>
        /// Smoothing factor for waveform (0 = sharp, 1 = very smooth)
        /// </summary>
        public float SmoothingFactor { get; set; } = 0.3f;

        /// <summary>
        /// Amplitude scale multiplier
        /// </summary>
        public float AmplitudeScale { get; set; } = 1.0f;

        /// <summary>
        /// Enable anti-aliasing
        /// </summary>
        public bool EnableAntiAliasing { get; set; } = true;

        /// <summary>
        /// Number of points to render (capped at 512)
        /// </summary>
        public int RenderResolution
        {
            get => _renderResolution;
            set => _renderResolution = Math.Min(value, MaxRenderResolution);
        }
        private int _renderResolution = 256;

        /// <summary>
        /// Show amplitude labels
        /// </summary>
        public bool ShowAmplitudeLabels { get; set; } = false;

        /// <summary>
        /// Audio sample rate in Hz (default 44100). Used for frequency filtering.
        /// </summary>
        public int SampleRate
        {
            get => _sampleRate;
            set => _sampleRate = Math.Max(8000, Math.Min(192000, value));
        }

        /// <summary>
        /// Minimum frequency to display in Hz (high-pass filter cutoff).
        /// Set to 0 to disable high-pass filtering.
        /// </summary>
        public float MinimumFrequency { get; set; } = 20f;

        /// <summary>
        /// Maximum frequency to display in Hz (low-pass filter cutoff).
        /// Set to 0 or Nyquist to disable low-pass filtering.
        /// </summary>
        public float MaximumFrequency { get; set; } = 16000f;

        /// <summary>
        /// Enable frequency filtering based on MinimumFrequency and MaximumFrequency
        /// </summary>
        public bool EnableFrequencyFilter { get; set; } = false;

        /// <summary>
        /// Decay rate for smooth amplitude falloff (0 = instant, 0.99 = very slow).
        /// Applied per-frame to create smooth falling animation.
        /// </summary>
        public float DecayRate { get; set; } = 0.85f;

        /// <summary>
        /// Enable smooth decay animation (amplitudes fall gradually instead of instantly)
        /// </summary>
        public bool EnableDecay { get; set; } = true;

        /// <summary>
        /// Attack rate - how fast amplitude rises (0 = instant, 1 = slowest)
        /// </summary>
        public float AttackRate { get; set; } = 0.7f;

        /// <summary>
        /// Use smooth curved lines (bezier/spline) instead of straight lines.
        /// Creates more organic, flowing waveforms with rounded peaks.
        /// </summary>
        public bool UseCurvedLines { get; set; } = true;

        /// <summary>
        /// Curve tension for bezier interpolation (0 = angular, 1 = very smooth curves).
        /// Only applies when UseCurvedLines is true.
        /// </summary>
        public float CurveTension { get; set; } = 0.5f;

        #endregion

        #region Constructor

        public ModernWaveformSpectrum(int bufferSize = DefaultBufferSize)
        {
            _leftBuffer = new float[bufferSize];
            _rightBuffer = new float[bufferSize];

            // Pre-allocate arrays
            _cachedLeftSamples = new float[MaxRenderResolution];
            _cachedRightSamples = new float[MaxRenderResolution];
            _decayLeftSamples = new float[MaxRenderResolution];
            _decayRightSamples = new float[MaxRenderResolution];
            _cachedPoints1 = new PointF[MaxRenderResolution];
            _cachedPoints2 = new PointF[MaxRenderResolution];
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a stereo sample pair to the buffer with optional frequency filtering
        /// </summary>
        public void AddSamples(float left, float right)
        {
            // Apply frequency filtering if enabled
            if (EnableFrequencyFilter)
            {
                ApplyFrequencyFilter(ref left, ref right);
            }

            lock (_lockObj)
            {
                _leftBuffer[_bufferWriteIndex] = left;
                _rightBuffer[_bufferWriteIndex] = right;
                _bufferWriteIndex = (_bufferWriteIndex + 1) % _leftBuffer.Length;
                if (_sampleCount < _leftBuffer.Length)
                    _sampleCount++;
            }
        }

        /// <summary>
        /// Renders the waveform to a bitmap
        /// </summary>
        public Bitmap Draw(int width, int height)
        {
            if (_cachedBitmap == null || _cachedBitmap.Width != width || _cachedBitmap.Height != height)
            {
                _cachedGraphics?.Dispose();
                _cachedBitmap?.Dispose();

                _cachedBitmap = new Bitmap(width, height);
                _cachedGraphics = Graphics.FromImage(_cachedBitmap);
            }

            Draw(_cachedGraphics, width, height);

            return (Bitmap)_cachedBitmap.Clone();
        }

        /// <summary>
        /// Renders the waveform to a Graphics object
        /// </summary>
        public void Draw(Graphics graphics, int width, int height)
        {
            if (_brushesDirty || _lastSize.Width != width || _lastSize.Height != height)
            {
                UpdateCachedResources(width, height);
                _lastSize = new Size(width, height);
                _brushesDirty = false;
            }

            // Configure graphics
            bool isLargeScreen = width > 1200 || height > 800;
            if (EnableAntiAliasing && !isLargeScreen)
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            }
            else
            {
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.Low;
            }
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            // Clear background
            graphics.FillRectangle(_backgroundBrush, 0, 0, width, height);

            // Draw grid
            if (ShowGrid)
                DrawGrid(graphics, width, height);

            // Draw center line
            if (ShowCenterLine)
            {
                int centerY = height / 2;
                graphics.DrawLine(_centerLinePen, 0, centerY, width, centerY);
            }

            // Get and process samples
            int sampleCount = GetProcessedSamples(_renderResolution);

            if (sampleCount < 2)
                return;

            // Draw based on style
            switch (Style)
            {
                case WaveformStyle.Line:
                    DrawLineWaveform(graphics, sampleCount, width, height);
                    break;
                case WaveformStyle.FilledMirror:
                    DrawFilledMirrorWaveform(graphics, sampleCount, width, height);
                    break;
                case WaveformStyle.Bars:
                    DrawBarsWaveform(graphics, sampleCount, width, height);
                    break;
                case WaveformStyle.Points:
                    DrawPointsWaveform(graphics, sampleCount, width, height);
                    break;
                case WaveformStyle.AreaFill:
                    DrawAreaFillWaveform(graphics, sampleCount, width, height);
                    break;
                case WaveformStyle.MirroredBars:
                    DrawMirroredBarsWaveform(graphics, sampleCount, width, height);
                    break;
            }

            // Draw labels
            if (ShowAmplitudeLabels && !isLargeScreen)
                DrawAmplitudeLabels(graphics, width, height);
        }

        /// <summary>
        /// Resets all buffers and decay state
        /// </summary>
        public void Reset()
        {
            lock (_lockObj)
            {
                Array.Clear(_leftBuffer, 0, _leftBuffer.Length);
                Array.Clear(_rightBuffer, 0, _rightBuffer.Length);
                Array.Clear(_decayLeftSamples, 0, _decayLeftSamples.Length);
                Array.Clear(_decayRightSamples, 0, _decayRightSamples.Length);
                _bufferWriteIndex = 0;
                _sampleCount = 0;
                _lpfLeftState = 0;
                _lpfRightState = 0;
            }
        }

        public void Dispose()
        {
            _leftChannelBrush?.Dispose();
            _rightChannelBrush?.Dispose();
            _backgroundBrush?.Dispose();
            _leftChannelPen?.Dispose();
            _rightChannelPen?.Dispose();
            _centerLinePen?.Dispose();
            _gridPen?.Dispose();
            _cachedGraphics?.Dispose();
            _cachedBitmap?.Dispose();
        }

        #endregion

        #region Frequency Filtering

        /// <summary>
        /// Applies simple frequency filtering to the samples
        /// </summary>
        private void ApplyFrequencyFilter(ref float left, ref float right)
        {
            float nyquist = _sampleRate / 2f;

            // Low-pass filter (removes high frequencies above MaximumFrequency)
            if (MaximumFrequency > 0 && MaximumFrequency < nyquist)
            {
                // Simple one-pole low-pass filter
                float lpfAlpha = CalculateFilterAlpha(MaximumFrequency);
                _lpfLeftState = _lpfLeftState + lpfAlpha * (left - _lpfLeftState);
                _lpfRightState = _lpfRightState + lpfAlpha * (right - _lpfRightState);
                left = _lpfLeftState;
                right = _lpfRightState;
            }

            // High-pass filter (removes low frequencies below MinimumFrequency)
            // Implemented as: output = input - lowpass(input)
            if (MinimumFrequency > 0)
            {
                float hpfAlpha = CalculateFilterAlpha(MinimumFrequency);
                // This is a simplified approach - subtract the low-frequency component
                float lpfLeft = left * hpfAlpha;
                float lpfRight = right * hpfAlpha;
                left = left - lpfLeft * (1 - hpfAlpha);
                right = right - lpfRight * (1 - hpfAlpha);
            }
        }

        /// <summary>
        /// Calculates filter coefficient alpha for a given cutoff frequency
        /// </summary>
        private float CalculateFilterAlpha(float cutoffFrequency)
        {
            // Simple RC filter approximation
            float dt = 1.0f / _sampleRate;
            float rc = 1.0f / (2.0f * (float)Math.PI * cutoffFrequency);
            return dt / (rc + dt);
        }

        #endregion

        #region Private Drawing Methods

        private void DrawLineWaveform(Graphics g, int sampleCount, int width, int height)
        {
            int centerY = height / 2;
            float xStep = (float)width / (sampleCount - 1);
            float yScale = (height / 2.0f) * AmplitudeScale;

            // Fill point arrays with decay-applied samples
            for (int i = 0; i < sampleCount; i++)
            {
                float x = i * xStep;
                _cachedPoints1[i] = new PointF(x, centerY - _cachedLeftSamples[i] * yScale);
                _cachedPoints2[i] = new PointF(x, centerY - _cachedRightSamples[i] * yScale);
            }

            // Draw glow
            if (EnableGlow)
            {
                if (RenderChannel == WaveformChannel.Both || RenderChannel == WaveformChannel.Left)
                {
                    using (Pen glowPen = new Pen(Color.FromArgb(50, _leftChannelColor), LineThickness + 4))
                    {
                        DrawLinesOptimized(g, glowPen, _cachedPoints1, sampleCount);
                    }
                }
                if (RenderChannel == WaveformChannel.Both || RenderChannel == WaveformChannel.Right)
                {
                    using (Pen glowPen = new Pen(Color.FromArgb(50, _rightChannelColor), LineThickness + 4))
                    {
                        DrawLinesOptimized(g, glowPen, _cachedPoints2, sampleCount);
                    }
                }
            }

            // Draw channels
            if (RenderChannel == WaveformChannel.Both || RenderChannel == WaveformChannel.Left)
                DrawLinesOptimized(g, _leftChannelPen, _cachedPoints1, sampleCount);
            if (RenderChannel == WaveformChannel.Both || RenderChannel == WaveformChannel.Right)
                DrawLinesOptimized(g, _rightChannelPen, _cachedPoints2, sampleCount);
        }

        private void DrawFilledMirrorWaveform(Graphics g, int sampleCount, int width, int height)
        {
            int centerY = height / 2;
            float xStep = (float)width / (sampleCount - 1);
            float yScale = (height / 2.0f) * AmplitudeScale * 0.9f;

            for (int i = 0; i < sampleCount; i++)
            {
                float combined = (Math.Abs(_cachedLeftSamples[i]) + Math.Abs(_cachedRightSamples[i])) / 2.0f;
                float x = i * xStep;
                _cachedPoints1[i] = new PointF(x, centerY - combined * yScale);
                _cachedPoints2[i] = new PointF(x, centerY + combined * yScale);
            }

            // Top area
            using (GraphicsPath topPath = new GraphicsPath())
            {
                AddLinesToPath(topPath, _cachedPoints1, sampleCount);
                topPath.AddLine(_cachedPoints1[sampleCount - 1].X, _cachedPoints1[sampleCount - 1].Y, width, centerY);
                topPath.AddLine(width, centerY, 0, centerY);
                topPath.CloseFigure();

                if (EnableGradientFill)
                {
                    using (LinearGradientBrush gradBrush = new LinearGradientBrush(
                        new Point(0, 0), new Point(0, centerY),
                        Color.FromArgb(180, _leftChannelColor),
                        Color.FromArgb(50, _leftChannelColor)))
                    {
                        g.FillPath(gradBrush, topPath);
                    }
                }
                else
                {
                    g.FillPath(_leftChannelBrush, topPath);
                }
            }

            // Bottom area
            using (GraphicsPath bottomPath = new GraphicsPath())
            {
                AddLinesToPath(bottomPath, _cachedPoints2, sampleCount);
                bottomPath.AddLine(_cachedPoints2[sampleCount - 1].X, _cachedPoints2[sampleCount - 1].Y, width, centerY);
                bottomPath.AddLine(width, centerY, 0, centerY);
                bottomPath.CloseFigure();

                if (EnableGradientFill)
                {
                    using (LinearGradientBrush gradBrush = new LinearGradientBrush(
                        new Point(0, centerY), new Point(0, height),
                        Color.FromArgb(50, _rightChannelColor),
                        Color.FromArgb(180, _rightChannelColor)))
                    {
                        g.FillPath(gradBrush, bottomPath);
                    }
                }
                else
                {
                    g.FillPath(_rightChannelBrush, bottomPath);
                }
            }

            // Outlines
            DrawLinesOptimized(g, _leftChannelPen, _cachedPoints1, sampleCount);
            DrawLinesOptimized(g, _rightChannelPen, _cachedPoints2, sampleCount);
        }

        private void DrawBarsWaveform(Graphics g, int sampleCount, int width, int height)
        {
            int centerY = height / 2;
            float barWidth = Math.Max(1, (float)width / sampleCount - 1);
            float yScale = (height / 2.0f) * AmplitudeScale * 0.9f;

            for (int i = 0; i < sampleCount; i++)
            {
                float x = i * ((float)width / sampleCount);
                float leftHeight = Math.Abs(_cachedLeftSamples[i]) * yScale;
                float rightHeight = Math.Abs(_cachedRightSamples[i]) * yScale;

                if (RenderChannel == WaveformChannel.Both || RenderChannel == WaveformChannel.Left)
                {
                    if (leftHeight > 0.5f)
                        g.FillRectangle(_leftChannelBrush, x, centerY - leftHeight, barWidth, leftHeight);
                }
                if (RenderChannel == WaveformChannel.Both || RenderChannel == WaveformChannel.Right)
                {
                    if (rightHeight > 0.5f)
                        g.FillRectangle(_rightChannelBrush, x, centerY, barWidth, rightHeight);
                }
            }
        }

        private void DrawMirroredBarsWaveform(Graphics g, int sampleCount, int width, int height)
        {
            int centerY = height / 2;
            float barWidth = Math.Max(2, (float)width / sampleCount - 2);
            float yScale = (height / 2.0f) * AmplitudeScale * 0.85f;

            for (int i = 0; i < sampleCount; i++)
            {
                float x = i * ((float)width / sampleCount) + 1;
                float combined = (Math.Abs(_cachedLeftSamples[i]) + Math.Abs(_cachedRightSamples[i])) / 2.0f;
                float barHeight = combined * yScale;

                if (barHeight < 1) continue;

                g.FillRectangle(_leftChannelBrush, x, centerY - barHeight, barWidth, barHeight);
                g.FillRectangle(_rightChannelBrush, x, centerY, barWidth, barHeight);
            }
        }

        private void DrawPointsWaveform(Graphics g, int sampleCount, int width, int height)
        {
            int centerY = height / 2;
            float xStep = (float)width / (sampleCount - 1);
            float yScale = (height / 2.0f) * AmplitudeScale;
            float pointSize = Math.Max(2, LineThickness);
            float halfPoint = pointSize / 2;

            for (int i = 0; i < sampleCount; i++)
            {
                float x = i * xStep;

                if (RenderChannel == WaveformChannel.Both || RenderChannel == WaveformChannel.Left)
                {
                    float y = centerY - _cachedLeftSamples[i] * yScale;
                    g.FillEllipse(_leftChannelBrush, x - halfPoint, y - halfPoint, pointSize, pointSize);
                }
                if (RenderChannel == WaveformChannel.Both || RenderChannel == WaveformChannel.Right)
                {
                    float y = centerY - _cachedRightSamples[i] * yScale;
                    g.FillEllipse(_rightChannelBrush, x - halfPoint, y - halfPoint, pointSize, pointSize);
                }
            }
        }

        private void DrawAreaFillWaveform(Graphics g, int sampleCount, int width, int height)
        {
            int centerY = height / 2;
            float xStep = (float)width / (sampleCount - 1);
            float yScale = (height / 2.0f) * AmplitudeScale;

            for (int i = 0; i < sampleCount; i++)
            {
                _cachedPoints1[i] = new PointF(i * xStep, centerY - _cachedLeftSamples[i] * yScale);
            }

            if ((RenderChannel == WaveformChannel.Both || RenderChannel == WaveformChannel.Left) && sampleCount >= 2)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddLine(0, centerY, _cachedPoints1[0].X, _cachedPoints1[0].Y);
                    AddLinesToPath(path, _cachedPoints1, sampleCount);
                    path.AddLine(_cachedPoints1[sampleCount - 1].X, _cachedPoints1[sampleCount - 1].Y, width, centerY);
                    path.CloseFigure();

                    using (LinearGradientBrush gradBrush = new LinearGradientBrush(
                        new Point(0, 0), new Point(0, height),
                        Color.FromArgb(150, _leftChannelColor),
                        Color.FromArgb(30, _leftChannelColor)))
                    {
                        g.FillPath(gradBrush, path);
                    }
                }
            }
        }

        private void DrawGrid(Graphics g, int width, int height)
        {
            for (int i = 1; i < 4; i++)
            {
                int y = height * i / 4;
                g.DrawLine(_gridPen, 0, y, width, y);
            }
            for (int i = 1; i < 8; i++)
            {
                int x = width * i / 8;
                g.DrawLine(_gridPen, x, 0, x, height);
            }
        }

        private void DrawAmplitudeLabels(Graphics g, int width, int height)
        {
            using (Font font = new Font("Segoe UI", 8))
            using (Brush textBrush = new SolidBrush(Color.FromArgb(150, 255, 255, 255)))
            {
                g.DrawString("100%", font, textBrush, 5, 5);
                g.DrawString("0%", font, textBrush, 5, height / 2 - 8);
                g.DrawString("-100%", font, textBrush, 5, height - 20);
            }
        }

        private void DrawLinesOptimized(Graphics g, Pen pen, PointF[] points, int count)
        {
            if (count < 2) return;

            if (UseCurvedLines && count >= 4)
            {
                // Use smooth curve (cardinal spline) for organic look
                DrawCurveOptimized(g, pen, points, count);
            }
            else
            {
                // Fallback to straight lines
                if (count == points.Length)
                {
                    g.DrawLines(pen, points);
                }
                else
                {
                    PointF[] subset = new PointF[count];
                    Array.Copy(points, subset, count);
                    g.DrawLines(pen, subset);
                }
            }
        }

        /// <summary>
        /// Draws a smooth cardinal spline curve through points
        /// </summary>
        private void DrawCurveOptimized(Graphics g, Pen pen, PointF[] points, int count)
        {
            if (count < 2) return;

            // Cardinal spline tension (0 = straight lines, 1 = very curvy)
            float tension = CurveTension;

            if (count == points.Length)
            {
                g.DrawCurve(pen, points, tension);
            }
            else
            {
                PointF[] subset = new PointF[count];
                Array.Copy(points, subset, count);
                g.DrawCurve(pen, subset, tension);
            }
        }

        private void AddLinesToPath(GraphicsPath path, PointF[] points, int count)
        {
            if (count < 2) return;

            if (UseCurvedLines && count >= 4)
            {
                // Use smooth curve for organic look
                AddCurveToPath(path, points, count);
            }
            else
            {
                // Fallback to straight lines
                if (count == points.Length)
                {
                    path.AddLines(points);
                }
                else
                {
                    PointF[] subset = new PointF[count];
                    Array.Copy(points, subset, count);
                    path.AddLines(subset);
                }
            }
        }

        /// <summary>
        /// Adds a smooth cardinal spline curve to a GraphicsPath
        /// </summary>
        private void AddCurveToPath(GraphicsPath path, PointF[] points, int count)
        {
            if (count < 2) return;

            float tension = CurveTension;

            if (count == points.Length)
            {
                path.AddCurve(points, tension);
            }
            else
            {
                PointF[] subset = new PointF[count];
                Array.Copy(points, subset, count);
                path.AddCurve(subset, tension);
            }
        }

        #endregion

        #region Private Helper Methods

        private void UpdateCachedResources(int width, int height)
        {
            _leftChannelBrush?.Dispose();
            _rightChannelBrush?.Dispose();
            _backgroundBrush?.Dispose();
            _leftChannelPen?.Dispose();
            _rightChannelPen?.Dispose();
            _centerLinePen?.Dispose();
            _gridPen?.Dispose();

            _leftChannelBrush = new SolidBrush(Color.FromArgb(180, _leftChannelColor));
            _rightChannelBrush = new SolidBrush(Color.FromArgb(150, _rightChannelColor));
            _backgroundBrush = new SolidBrush(_backgroundColor);

            _leftChannelPen = new Pen(_leftChannelColor, LineThickness);
            _leftChannelPen.StartCap = LineCap.Round;
            _leftChannelPen.EndCap = LineCap.Round;
            _leftChannelPen.LineJoin = LineJoin.Round;

            _rightChannelPen = new Pen(_rightChannelColor, LineThickness);
            _rightChannelPen.StartCap = LineCap.Round;
            _rightChannelPen.EndCap = LineCap.Round;
            _rightChannelPen.LineJoin = LineJoin.Round;

            _centerLinePen = new Pen(_centerLineColor, 1);
            _gridPen = new Pen(_gridColor, 1);
        }

        /// <summary>
        /// Gets processed samples with smoothing and decay applied
        /// </summary>
        private int GetProcessedSamples(int targetCount)
        {
            lock (_lockObj)
            {
                if (_sampleCount < 2)
                {
                    // Apply decay even when no new samples (for smooth falloff)
                    if (EnableDecay)
                    {
                        for (int i = 0; i < targetCount && i < MaxRenderResolution; i++)
                        {
                            _decayLeftSamples[i] *= DecayRate;
                            _decayRightSamples[i] *= DecayRate;
                            _cachedLeftSamples[i] = _decayLeftSamples[i];
                            _cachedRightSamples[i] = _decayRightSamples[i];
                        }
                        return targetCount;
                    }
                    return 0;
                }

                int step = Math.Max(1, _sampleCount / targetCount);
                int actualCount = Math.Min(_sampleCount / step, MaxRenderResolution);

                int readIndex = (_bufferWriteIndex - _sampleCount + _leftBuffer.Length) % _leftBuffer.Length;
                float prevLeft = 0, prevRight = 0;

                for (int i = 0; i < actualCount; i++)
                {
                    // Find peak in this chunk
                    float maxLeft = 0, maxRight = 0;
                    for (int j = 0; j < step; j++)
                    {
                        int idx = (readIndex + i * step + j) % _leftBuffer.Length;
                        if (Math.Abs(_leftBuffer[idx]) > Math.Abs(maxLeft))
                            maxLeft = _leftBuffer[idx];
                        if (Math.Abs(_rightBuffer[idx]) > Math.Abs(maxRight))
                            maxRight = _rightBuffer[idx];
                    }

                    // Apply smoothing between samples
                    if (SmoothingFactor > 0 && i > 0)
                    {
                        float smoothFactor = 1 - SmoothingFactor;
                        maxLeft = prevLeft + (maxLeft - prevLeft) * smoothFactor;
                        maxRight = prevRight + (maxRight - prevRight) * smoothFactor;
                    }

                    // Apply decay with attack/release envelope
                    if (EnableDecay)
                    {
                        float absLeft = Math.Abs(maxLeft);
                        float absRight = Math.Abs(maxRight);
                        float decayedLeft = Math.Abs(_decayLeftSamples[i]);
                        float decayedRight = Math.Abs(_decayRightSamples[i]);

                        // Attack: new value is higher - use attack rate
                        // Release: new value is lower - use decay rate
                        if (absLeft > decayedLeft)
                        {
                            // Attack - rise quickly
                            _decayLeftSamples[i] = maxLeft * AttackRate + _decayLeftSamples[i] * (1 - AttackRate);
                        }
                        else
                        {
                            // Decay - fall gradually, preserve sign
                            _decayLeftSamples[i] *= DecayRate;
                        }

                        if (absRight > decayedRight)
                        {
                            _decayRightSamples[i] = maxRight * AttackRate + _decayRightSamples[i] * (1 - AttackRate);
                        }
                        else
                        {
                            _decayRightSamples[i] *= DecayRate;
                        }

                        _cachedLeftSamples[i] = _decayLeftSamples[i];
                        _cachedRightSamples[i] = _decayRightSamples[i];
                    }
                    else
                    {
                        _cachedLeftSamples[i] = maxLeft;
                        _cachedRightSamples[i] = maxRight;
                    }

                    prevLeft = maxLeft;
                    prevRight = maxRight;
                }

                _sampleCount = 0;
                _bufferWriteIndex = 0;

                return actualCount;
            }
        }

        #endregion
    }

    #region Enums

    /// <summary>
    /// Waveform visualization styles
    /// </summary>
    public enum WaveformStyle
    {
        /// <summary>Simple line waveform with optional glow and decay</summary>
        Line,
        /// <summary>Filled area with mirrored top/bottom</summary>
        FilledMirror,
        /// <summary>Vertical bars</summary>
        Bars,
        /// <summary>Point/dot visualization</summary>
        Points,
        /// <summary>Filled area under the waveform</summary>
        AreaFill,
        /// <summary>Mirrored vertical bars with gradient</summary>
        MirroredBars
    }

    /// <summary>
    /// Which audio channels to render
    /// </summary>
    public enum WaveformChannel
    {
        Left,
        Right,
        Both
    }

    #endregion
}
