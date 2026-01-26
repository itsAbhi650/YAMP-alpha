using System;
using System.Collections.Generic;
using CSCore.DSP;

namespace YAMP_alpha
{
    /// <summary>
    /// Spectrum provider that maintains peak values with decay over time.
    /// Shows both current spectrum and falling peak indicators.
    /// </summary>
    public class PeakHoldSpectrumProvider : FftProvider, ISpectrumProvider
    {
        private readonly int _sampleRate;
        private readonly List<object> _contexts = new List<object>();
        private float[] _peakValues;
        private int _framesSinceLastUpdate;  // Track frames since last GetFftData call
        private readonly int _peakHoldFrames;
        private readonly float _peakDecayRate;
        private PeakHoldMode _peakHoldMode;

        /// <summary>
        /// Creates a new PeakHoldSpectrumProvider
        /// </summary>
        /// <param name="channels">Number of audio channels</param>
        /// <param name="sampleRate">Sample rate in Hz</param>
        /// <param name="fftSize">FFT size</param>
        /// <param name="peakHoldFrames">Number of frames to hold peak before decay (default: 15)</param>
        /// <param name="peakDecayRate">Decay rate per frame (default: 0.95, slower = 0.98, faster = 0.90)</param>
        /// <param name="peakHoldMode">Peak hold behavior mode (default: FallingPeak)</param>
        public PeakHoldSpectrumProvider(int channels, int sampleRate, FftSize fftSize, 
            int peakHoldFrames = 15, float peakDecayRate = 0.95f, PeakHoldMode peakHoldMode = PeakHoldMode.FallingPeak) 
            : base(channels, fftSize)
        {
            if (sampleRate <= 0)
                throw new ArgumentOutOfRangeException("sampleRate");
            if (peakHoldFrames < 0)
                throw new ArgumentOutOfRangeException("peakHoldFrames");
            if (peakDecayRate < 0.5f || peakDecayRate > 1.0f)
                throw new ArgumentOutOfRangeException("peakDecayRate", "Must be between 0.5 and 1.0");

            _sampleRate = sampleRate;
            _peakHoldFrames = peakHoldFrames;
            _peakDecayRate = peakDecayRate;
            _peakValues = new float[(int)fftSize / 2];
            _framesSinceLastUpdate = 0;
            _peakHoldMode = peakHoldMode;
        }

        /// <summary>
        /// Gets or sets the number of frames to hold peak values before decay
        /// </summary>
        public int PeakHoldFrames { get; set; }

        /// <summary>
        /// Gets or sets the decay rate (0.5 to 1.0, where 1.0 = no decay, 0.9 = fast decay)
        /// </summary>
        public float PeakDecayRate 
        { 
            get { return _peakDecayRate; }
        }

        /// <summary>
        /// Gets or sets the peak hold behavior mode
        /// </summary>
        public PeakHoldMode PeakHoldMode
        {
            get { return _peakHoldMode; }
            set { _peakHoldMode = value; }
        }

        public int GetFftBandIndex(float frequency)
        {
            int fftSize = (int)FftSize;
            double f = _sampleRate / 2.0;
            return (int)((frequency / f) * (fftSize / 2));
        }

        /// <summary>
        /// Gets FFT data with peak hold applied
        /// </summary>
        public bool GetFftData(float[] fftResultBuffer, object context)
        {
            if (_contexts.Contains(context))
                return false;

            _contexts.Add(context);
            
            // Get current FFT data
            GetFftData(fftResultBuffer);

            // Increment frame counter (for decay timing)
            _framesSinceLastUpdate++;

            // Update peak values with decay
            UpdatePeakValues(fftResultBuffer);

            return true;
        }

        /// <summary>
        /// Gets the peak values array (for rendering peak indicators separately)
        /// </summary>
        public float[] GetPeakValues()
        {
            float[] peaks = new float[_peakValues.Length];
            Array.Copy(_peakValues, peaks, _peakValues.Length);
            return peaks;
        }

        private void UpdatePeakValues(float[] currentFftData)
        {
            for (int i = 0; i < currentFftData.Length && i < _peakValues.Length; i++)
            {
                // If current value exceeds peak, update peak
                if (currentFftData[i] > _peakValues[i])
                {
                    _peakValues[i] = currentFftData[i];
                }
                else
                {
                    // Apply decay based on mode
                    switch (_peakHoldMode)
                    {
                        case PeakHoldMode.NeverFall:
                            // Peaks never decay - stay at maximum forever
                            break;

                        case PeakHoldMode.FallingPeak:
                            // Standard falling peak with hold time
                            if (_framesSinceLastUpdate > _peakHoldFrames)
                            {
                                _peakValues[i] *= _peakDecayRate;
                                
                                if (_peakValues[i] < 0.001f)
                                    _peakValues[i] = 0;
                            }
                            break;

                        case PeakHoldMode.InstantFall:
                            // Peaks immediately follow bar height (no hold)
                            _peakValues[i] = currentFftData[i];
                            break;
                    }
                }
            }
        }

        public override void Add(float[] samples, int count)
        {
            base.Add(samples, count);
            if (count > 0)
            {
                _contexts.Clear();
                // Don't reset frame counter here - let it accumulate for decay
            }
        }

        public override void Add(float left, float right)
        {
            base.Add(left, right);
            _contexts.Clear();
            // Don't reset frame counter here - let it accumulate for decay
        }

        /// <summary>
        /// Resets all peak values to zero
        /// </summary>
        public void ResetPeaks()
        {
            Array.Clear(_peakValues, 0, _peakValues.Length);
            _framesSinceLastUpdate = 0;
        }
    }
}
