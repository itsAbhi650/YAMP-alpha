using CSCore;
using CSCore.DSP;

namespace YAMP_alpha
{
    public class BiQuadFiltersSource : SampleAggregatorBase
    {
        private readonly object _lockObject = new object();
        private BiQuad[] _biquad;

        public BiQuad[] Filters
        {
            get { return _biquad; }
            set
            {
                lock (_lockObject)
                {
                    _biquad = value;
                }
            }
        }

        public bool FilteringEnabled { get; set; } = true;

        public BiQuadFiltersSource(ISampleSource source) : base(source)
        {

        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            lock (_lockObject)
            {
                if (Filters != null && FilteringEnabled)
                {
                    for (int i = 0; i < read; i++)
                    {
                        foreach (BiQuad filter in Filters)
                        {
                            buffer[i + offset] = filter.Process(buffer[i + offset]);
                        }
                    }
                }
            }
            return read;
        }
    }
}
