using CSCore;
using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;


namespace YAMP_alpha
{
    public class LineSpectrum
    {
        private int _barCount;
        private double _barSpacing;
        private double _barWidth;
        public FftSize FftSize;
        private ScalingStrategy _scalingStrategy;
        private int _maxFftIndex;
        private int _maximumFrequency = 20000;
        private int _maximumFrequencyIndex;
        private int _minimumFrequency = 20; //Default spectrum from 20Hz to 20kHz
        private int _minimumFrequencyIndex;
        private int[] _spectrumIndexMax;
        private int[] _spectrumLogScaleIndexMax;
        private int[] _spectrumBucketFrequency;
        public BasicSpectrumProvider _spectrumProvider;

        public int ScaleFactorLinear { get; set; } = 9;

        public const int MinDecibelValue = -90;

        private const int MaxDecibelValue = 0;

        private readonly int DbScale = MaxDecibelValue - MinDecibelValue;

        public LineSpectrum(FftSize fftSize, BasicSpectrumProvider _spectrumProviderp)
        {
            FftSize = fftSize;
            _spectrumProvider = _spectrumProviderp;
            //ScaleStrategy = ScalingStrategy.Sqrt;
        }

        public int MaximumFrequency
        {
            get { return _maximumFrequency; }
            set
            {
                if (value <= MinimumFrequency)
                    throw new ArgumentOutOfRangeException("value",
                        "Value must not be less or equal the MinimumFrequency.");
                _maximumFrequency = value;
                UpdateFrequencyMapping();

                //RaisePropertyChanged("MaximumFrequency");
            }
        }

        public int MinimumFrequency
        {
            get { return _minimumFrequency; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _minimumFrequency = value;
                UpdateFrequencyMapping();

                //RaisePropertyChanged("MinimumFrequency");
            }
        }

        public double BarSpacing
        {
            get { return _barSpacing; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _barSpacing = value;
                UpdateFrequencyMapping();
            }
        }

        public int BarCount
        {
            get { return _barCount; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _barCount = value;
                UpdateFrequencyMapping();
            }
        }

        void UpdateFrequencyMapping()
        {
            _maxFftIndex = (int)FftSize / 2 - 1;
            _maximumFrequencyIndex = Math.Min(_spectrumProvider.GetFftBandIndex(_maximumFrequency) + 1, _maxFftIndex);
            _minimumFrequencyIndex = Math.Min(_spectrumProvider.GetFftBandIndex(_minimumFrequency), _maxFftIndex);

            int actualResolution = BarCount;

            int indexCount = _maximumFrequencyIndex - _minimumFrequencyIndex;
            double linearIndexBucketSize = Math.Round(indexCount / (double)actualResolution, 3);

            _spectrumIndexMax = _spectrumIndexMax.CheckBuffer(actualResolution, true);
            _spectrumLogScaleIndexMax = _spectrumLogScaleIndexMax.CheckBuffer(actualResolution, true);
            _spectrumBucketFrequency = new int[_spectrumLogScaleIndexMax.GetUpperBound(0) + 1];

            double minFrequencyLog = Math.Log(_minimumFrequency); // min freq
            double maxFrequencyLog = Math.Log(_maximumFrequency); // max freq
            double bucketFactorLog = (maxFrequencyLog - minFrequencyLog) / actualResolution;
            for (int i = 1; i < actualResolution; i++)
            {
                _spectrumIndexMax[i - 1] = _minimumFrequencyIndex + (int)(i * linearIndexBucketSize);
                _spectrumLogScaleIndexMax[i - 1] = (int)(Math.Pow(Math.E, minFrequencyLog + (i - 1) * bucketFactorLog) / _maximumFrequency * indexCount);
                if (i > 1)
                {
                    if (_spectrumLogScaleIndexMax[i - 1] <= _spectrumLogScaleIndexMax[i - 2])
                    {
                        _spectrumLogScaleIndexMax[i - 1] = _spectrumLogScaleIndexMax[i - 2] + 1;
                    }
                }
                _spectrumBucketFrequency[i - 1] = (int)((double)_spectrumLogScaleIndexMax[i - 1] / indexCount * _maximumFrequency) + _minimumFrequency;
            }
            if (actualResolution > 0)
            {
                _spectrumIndexMax[_spectrumIndexMax.Length - 1] =
                _spectrumLogScaleIndexMax[_spectrumLogScaleIndexMax.Length - 1] = _maximumFrequencyIndex;
            }
        }

        public Bitmap CreateSpectrumLine(Size size, Color BarColor, Color background, bool highQuality)
        {
            var fftBuffer = new float[(int)FftSize];
            _barWidth = Math.Max((size.Width - (BarSpacing * (BarCount - 1))) / BarCount, 0.00001);

            if (_spectrumProvider.GetFftData(fftBuffer))
            {
                using (var pen = new Pen(new SolidBrush(BarColor), (float)_barWidth))
                {
                    var bitmap = new Bitmap(size.Width, size.Height);

                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        // PrepareGraphics(graphics, highQuality);
                        graphics.SmoothingMode = SmoothingMode.HighSpeed;
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.PixelOffsetMode = PixelOffsetMode.None;
                        graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                        //graphics.Clear(background);

                        // CreateSpectrumLineInternal(graphics, pen, fftBuffer, size);
                        int height = size.Height;
                        SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(height, fftBuffer);
                        //int freeHeadingColumn = 0;

                        for (int i = 0; i < spectrumPoints.Length; i++)
                        {
                            SpectrumPointData p = spectrumPoints[i];
                            int barIndex = p.SpectrumPointIndex;
                            double xCoord = (_barWidth * barIndex) + (BarSpacing * barIndex) + 1 + _barWidth / 2;

                            var p1 = new PointF((float)xCoord, height);
                            var p2 = new PointF((float)xCoord, height - (float)p.Value - 1);

                            graphics.DrawLine(pen, p1, p2);
                        }
                    }
                    return bitmap;
                }
            }
            //}
            return null;
        }

        SpectrumPointData[] CalculateSpectrumPoints(double maxValue, float[] fftBuffer)
        {
            var dataPoints = new List<SpectrumPointData>();

            double value0 = 0, value = 0;
            double actualMaxValue = maxValue;
            double sumBucketValues = 0;
            double numberBucketValues = 0;
            int spectrumPointIndex = 0;
            int ScaleFactorSqr = 3;
            for (int i = _minimumFrequencyIndex; i <= _maximumFrequencyIndex; i++)
            {
                switch (ScaleStrategy)
                {
                    case ScalingStrategy.Decibel:
                        value0 = (20 * Math.Log10(fftBuffer[i]) - MinDecibelValue) / DbScale * actualMaxValue;
                        break;
                    case ScalingStrategy.Linear:
                        value0 = fftBuffer[i] * ScaleFactorLinear * actualMaxValue;
                        break;
                    case ScalingStrategy.Sqrt:
                        value0 = Math.Sqrt(fftBuffer[i]) * ScaleFactorSqr * actualMaxValue * Math.Log10(i + 1);
                        break;
                }
                value = Math.Max(0, Math.Min(value0, maxValue));
                sumBucketValues += value;
                numberBucketValues += 1;
                if (i == _spectrumLogScaleIndexMax[spectrumPointIndex])
                {
                    dataPoints.Add(new SpectrumPointData { SpectrumPointIndex = spectrumPointIndex, Value = sumBucketValues / numberBucketValues, Frequency = _spectrumBucketFrequency[spectrumPointIndex] });
                    spectrumPointIndex++;
                    sumBucketValues = 0;
                    numberBucketValues = 0;
                }
            }

            return dataPoints.ToArray();
        }

        struct SpectrumPointData
        {
            public int SpectrumPointIndex;
            public double Value;
            public int Frequency;
        }

        public enum ScalingStrategy
        {
            Decibel,
            Linear,
            Sqrt
        }

        public ScalingStrategy ScaleStrategy
        {
            get { return _scalingStrategy; }
            set { _scalingStrategy = value; }
        }


    }
}
