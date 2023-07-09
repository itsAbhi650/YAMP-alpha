using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMP_alpha
{
    public class BellFilter : CSCore.DSP.BiQuad
    {
        public double BandWidth
        {
            get { return Q; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                Q = value;
            }
        }

        public BellFilter(int sampleRate, double frequency, double bandwidth, double gainDB) : base(sampleRate, frequency, bandwidth)
        {
            GainDB = gainDB;
        }

        protected override void CalculateBiQuadCoefficients()
        {
            double Pi = 3.141592653589793238;
            double w0 = (2 * Pi * Frequency) / SampleRate;
            if (GainDB > 0)
            {
                double alpha = Math.Sin(w0) / (Frequency / BandWidth);
                double B0 = 1 + alpha + GainDB;
                B1 = A1 = (-2 * Math.Cos(w0)) / B0;
                B2 = (1 - alpha * GainDB) / B0;
                A0 = (1 + alpha / GainDB) / B0;
                A2 = (1 - alpha / GainDB) / B0;
            }
            else if (GainDB == 0.0)
            {
                B1 = A1 = 1;
                B2 = 1;
                A0 = 1;
                A2 = 1;
            }
        }
    }
}
