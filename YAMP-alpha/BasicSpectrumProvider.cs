using CSCore.DSP;
using System;

namespace YAMP_alpha
{
    public class BasicSpectrumProvider : FftProvider
    {
        private readonly int _sampleRate;
        public BasicSpectrumProvider(int channels, int sampleRate, FftSize fftSize) : base(channels, fftSize)
        {
            if (sampleRate <= 0)
            {
                throw new ArgumentOutOfRangeException("sampleRate");
            }

            _sampleRate = sampleRate;
        }
        public int GetFftBandIndex(float frequency)
        {
            int fftSize = (int)FftSize;
            double f = _sampleRate / 2.0;
            // ReSharper disable once PossibleLossOfFraction
            return (int)(frequency / f * (fftSize / 2));
        }
        //public override void Add(float left, float right)
        //{
        //    base.Add(left, right);
        //}
    }
}
