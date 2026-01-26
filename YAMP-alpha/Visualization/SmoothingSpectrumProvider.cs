using System;
using System.Collections.Generic;
using CSCore.DSP;

namespace YAMP_alpha
{
    /// <summary>
    /// Spectrum provider that applies temporal smoothing using exponential moving average.
    /// Prevents jarring jumps between frames for fluid, flowing visualization.
    /// </summary>
    public class SmoothingSpectrumProvider : FftProvider, ISpectrumProvider
    {
        private readonly int _sampleRate;
        private readonly List<object> _contexts = new List<object>();
        private float[] _smoothedValues;
        private float _attackTime;  // Time to rise (in seconds)
        private float _releaseTime; // Time to fall (in seconds)
        private readonly float _frameRate; // Assumed frame rate for smoothing calculation

        /// <summary>
        /// Creates a new SmoothingSpectrumProvider
        /// </summary>
        /// <param name="channels">Number of audio channels</param>
        /// <param name="sampleRate">Sample rate in Hz</param>
        /// <param name="fftSize">FFT size</param>
        /// <param name="attackTime">Attack time in seconds (default: 0.02 = 20ms)</param>
        /// <param name="releaseTime">Release time in seconds (default: 0.1 = 100ms)</param>
        /// <param name="frameRate">Expected visualization frame rate (default: 60fps)</param>
        public SmoothingSpectrumProvider(int channels, int sampleRate, FftSize fftSize,
            float attackTime = 0.02f, float releaseTime = 0.1f, float frameRate = 60f)
            : base(channels, fftSize)
        {
            if (sampleRate <= 0)
                throw new ArgumentOutOfRangeException("sampleRate");
            if (attackTime <= 0)
                throw new ArgumentOutOfRangeException("attackTime");
            if (releaseTime <= 0)
                throw new ArgumentOutOfRangeException("releaseTime");
            if (frameRate <= 0)
                throw new ArgumentOutOfRangeException("frameRate");

            _sampleRate = sampleRate;
            _attackTime = attackTime;
            _releaseTime = releaseTime;
            _frameRate = frameRate;
            _smoothedValues = new float[(int)fftSize / 2];
        }

        /// <summary>
        /// Gets or sets the attack time (rise time) in seconds.
        /// Lower = faster response, Higher = more smoothing (default: 0.02 = 20ms)
        /// </summary>
        public float AttackTime
        {
            get { return _attackTime; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _attackTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the release time (fall time) in seconds.
        /// Lower = faster decay, Higher = more trailing (default: 0.1 = 100ms)
        /// </summary>
        public float ReleaseTime
        {
            get { return _releaseTime; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _releaseTime = value;
            }
        }

        public int GetFftBandIndex(float frequency)
        {
            int fftSize = (int)FftSize;
            double f = _sampleRate / 2.0;
            return (int)((frequency / f) * (fftSize / 2));
        }

        /// <summary>
        /// Gets smoothed FFT data using exponential moving average
        /// </summary>
        public bool GetFftData(float[] fftResultBuffer, object context)
        {
            if (_contexts.Contains(context))
                return false;

            _contexts.Add(context);

            // Get current raw FFT data
            GetFftData(fftResultBuffer);

            // Apply smoothing
            ApplySmoothing(fftResultBuffer);

            // Copy smoothed values back to buffer
            Array.Copy(_smoothedValues, fftResultBuffer, Math.Min(_smoothedValues.Length, fftResultBuffer.Length));

            return true;
        }

        private void ApplySmoothing(float[] currentFftData)
        {
            // Calculate smoothing coefficients based on attack/release times
            // Formula: coefficient = 1 - exp(-1 / (time * frameRate))
            float attackCoeff = 1.0f - (float)Math.Exp(-1.0 / (_attackTime * _frameRate));
            float releaseCoeff = 1.0f - (float)Math.Exp(-1.0 / (_releaseTime * _frameRate));

            for (int i = 0; i < currentFftData.Length && i < _smoothedValues.Length; i++)
            {
                float currentValue = currentFftData[i];
                float smoothedValue = _smoothedValues[i];

                // Use attack coefficient when rising, release coefficient when falling
                if (currentValue > smoothedValue)
                {
                    // Attack (rising) - faster response
                    _smoothedValues[i] = smoothedValue + (currentValue - smoothedValue) * attackCoeff;
                }
                else
                {
                    // Release (falling) - slower decay
                    _smoothedValues[i] = smoothedValue + (currentValue - smoothedValue) * releaseCoeff;
                }

                // Clamp to prevent negative values
                if (_smoothedValues[i] < 0)
                    _smoothedValues[i] = 0;
            }
        }

        public override void Add(float[] samples, int count)
        {
            base.Add(samples, count);
            if (count > 0)
                _contexts.Clear();
        }

        public override void Add(float left, float right)
        {
            base.Add(left, right);
            _contexts.Clear();
        }

        /// <summary>
        /// Resets all smoothed values to zero
        /// </summary>
        public void ResetSmoothing()
        {
            Array.Clear(_smoothedValues, 0, _smoothedValues.Length);
        }

        /// <summary>
        /// Sets preset smoothing profiles
        /// </summary>
        public void SetSmoothingPreset(SmoothingPreset preset)
        {
            switch (preset)
            {
                case SmoothingPreset.VeryFast:
                    _attackTime = 0.01f;  // 10ms
                    _releaseTime = 0.05f; // 50ms
                    break;

                case SmoothingPreset.Fast:
                    _attackTime = 0.02f;  // 20ms
                    _releaseTime = 0.08f; // 80ms
                    break;

                case SmoothingPreset.Medium:
                    _attackTime = 0.03f;  // 30ms
                    _releaseTime = 0.12f; // 120ms
                    break;

                case SmoothingPreset.Slow:
                    _attackTime = 0.05f;  // 50ms
                    _releaseTime = 0.2f;  // 200ms
                    break;

                case SmoothingPreset.VerySlow:
                    _attackTime = 0.08f;  // 80ms
                    _releaseTime = 0.3f;  // 300ms
                    break;
            }
        }
    }

    /// <summary>
    /// Predefined smoothing presets
    /// </summary>
    public enum SmoothingPreset
    {
        /// <summary>Very fast response, minimal smoothing (10ms attack, 50ms release)</summary>
        VeryFast,
        
        /// <summary>Fast response with light smoothing (20ms attack, 80ms release)</summary>
        Fast,
        
        /// <summary>Balanced smoothing (30ms attack, 120ms release)</summary>
        Medium,
        
        /// <summary>Slow, fluid motion (50ms attack, 200ms release)</summary>
        Slow,
        
        /// <summary>Very slow, heavily smoothed (80ms attack, 300ms release)</summary>
        VerySlow
    }
}
