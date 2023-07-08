using CSCore;
using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMP_alpha
{
    public static class AudioFilters
    {
        /// <summary>
        /// Audio filter types
        /// </summary>
        public enum Filter
        {
            BandPass = 0,
            HighPass = 1,
            HighShelf = 2,
            LowPass = 3,
            LowShelf = 4,
            Notch = 5,
            Peak = 6,
            Bell = 7
        }

        private static PeakFilter BQP = null;
        private static NotchFilter BQN = null;
        private static BandpassFilter BQBP = null;
        private static HighpassFilter BQHP = null;
        private static HighShelfFilter BQHS = null;
        private static LowpassFilter BQLP = null;
        private static LowShelfFilter BQLS = null;
        private static BellFilter BQB = null;

        /// <summary>
        /// Get a single audio filter specified by its type.
        /// </summary>
        /// <param name="filter">type of filter.</param>
        /// <returns></returns>
        public static BiQuad GetFilter(Filter filter)
        {
            BiQuad flt;
            switch (filter)
            {
                case Filter.BandPass:
                    flt = BQBP;
                    break;
                case Filter.HighPass:
                    flt = BQHP;
                    break;
                case Filter.HighShelf:
                    flt = BQHS;
                    break;
                case Filter.LowPass:
                    flt = BQLP;
                    break;
                case Filter.LowShelf:
                    flt = BQLS;
                    break;
                case Filter.Notch:
                    flt = BQN;
                    break;
                case Filter.Peak:
                    flt = BQP;
                    break;
                default:
                    flt = null;
                    break;
            }
            return flt;
        }

        /// <summary>
        /// Initialize a filter
        /// </summary>
        /// <param name="filter">type of filter.</param>
        /// <param name="SampleRate">Sampling rate.</param>
        /// <param name="Frequency">Frequency (Hz).</param>
        /// <param name="Gain">Gain (dB)</param>
        /// <param name="BandWidth">Band Width</param>
        public static void SetFilter(Filter filter, int SampleRate, double Frequency, int Gain = 5, int BandWidth = 100)
        {
            switch (filter)
            {
                case Filter.BandPass:
                    BQBP = new BandpassFilter(SampleRate, Frequency);
                    break;
                case Filter.HighPass:
                    BQHP = new HighpassFilter(SampleRate, Frequency);
                    break;
                case Filter.HighShelf:
                    BQHS = new HighShelfFilter(SampleRate, Frequency, Gain);
                    break;
                case Filter.LowPass:
                    BQLP = new LowpassFilter(SampleRate, Frequency);
                    break;
                case Filter.LowShelf:
                    BQLS = new LowShelfFilter(SampleRate, Frequency, Gain);
                    break;
                case Filter.Notch:
                    BQN = new NotchFilter(SampleRate, Frequency);
                    break;
                case Filter.Peak:
                    BQP = new PeakFilter(SampleRate, Frequency, BandWidth, Gain);
                    break;
                case Filter.Bell:
                    BQB = new BellFilter(SampleRate, Frequency, BandWidth, Gain);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get an array including all the filters with supplied configurations.
        /// </summary>
        /// <param name="SampleRate">Sampling rate</param>
        /// <param name="Frequency">Frequency (Hz)</param>
        /// <param name="Gain">Gain (dB)</param>
        /// <param name="BandWidth">The Band Width. (if remain 0, peak filter will not be added in the array.)</param>
        /// <returns>Array of filters configured as per supplied configurations.</returns>
        public static BiQuad[] SetAllFilters(int SampleRate, double Frequency, int Gain = 1, int BandWidth = 0)
        {
            List<BiQuad> Filters = new List<BiQuad>();
            BQBP = new BandpassFilter(SampleRate, Frequency)
            {
                GainDB = Gain
            };
            Filters.Add(BQBP);
            BQHP = new HighpassFilter(SampleRate, Frequency)
            {
                GainDB = Gain
            };
            Filters.Add(BQHP);
            BQHS = new HighShelfFilter(SampleRate, Frequency, Gain);
            Filters.Add(BQHS);
            BQLP = new LowpassFilter(SampleRate, Frequency)
            {
                GainDB = Gain
            };
            Filters.Add(BQLP);
            BQLS = new LowShelfFilter(SampleRate, Frequency, Gain)
            {
                GainDB = Gain
            };
            Filters.Add(BQLS);
            BQN = new NotchFilter(SampleRate, Frequency)
            {
                GainDB = Gain
            };
            Filters.Add(BQN);
            if (BandWidth > 0)
            {
                BQP = new PeakFilter(SampleRate, Frequency, BandWidth, Gain);
                BQB = new BellFilter(SampleRate, Frequency, BandWidth, Gain);
                Filters.Add(BQP);
                Filters.Add(BQB);
            }
            return Filters.ToArray();
        }

        /// <summary>
        /// Append all filters to a sample source with supplied configuration.
        /// </summary>
        /// <param name="SampleSource">Sample source to which the filters will be appended.</param>
        /// <param name="FilterSource">Filter object to update configurations.</param>
        /// <param name="SampleRate">Sampling rate</param>
        /// <param name="Frequency">Frequency (Hz)</param>
        /// <param name="Gain">Gain (dB)</param>
        /// <param name="BandWidth">The Band Width. (if remain 0, peak filter will not be appended in the source.)</param>
        /// <returns>Sample source with filters appended.</returns>
        public static ISampleSource GetFilterSource(ISampleSource SampleSource, out BiQuadFiltersSource FilterSource, int SampleRate, double Frequency, int Gain = 1, int BandWidth = 0)
        {
            return SampleSource.AppendSource(x => new BiQuadFiltersSource(x) { Filters = SetAllFilters(SampleRate, Frequency, Gain, BandWidth) }, out FilterSource);
        }
    }
}
