using CSCore.DSP;
using CSCore.Streams;
using CSCore.Streams.Effects;
using FftSharp;
using ScottPlot.Plottable;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using YAMP_alpha.Controls;

namespace YAMP_alpha
{
    public partial class EqualizerDialog : Form
    {
        private float[] buffer;
        private SignalPlot signalPlot;
        private BasicSpectrumProvider SpectrumProvider;
        private VoicePrint3DSpectrum Spectrum;
        private int SampleRate;
        private int ChannelCount;
        private FftSize FFTSIZE;
        private Bitmap SpectroBitmap;
        private int MaxDB = 12;
        private int _xpos;

        private EQBand VolBand = new EQBand("Volume", 0, 100, 0, string.Empty)
        {
            FooterVisible = true,
            Dock = DockStyle.Left,
            ShowBandValueInFooter = true
        };

        private EQBand GainBand = new EQBand("Gain", 100, 400, 0, string.Empty)
        {
            FooterVisible = true,
            Dock = DockStyle.Left,
            ShowBandValueInFooter = true
        };

        public EqualizerDialog()
        {
            InitializeComponent();
            formsPlot1.Plot.Frameless(true);
            formsPlot1.Plot.Margins(0);
            SpectroBitmap = new Bitmap(2000, 600);
        }

        private void EqualizerDialog_Load(object sender, EventArgs e)
        {
            bool flag = YAMPVars.CORE != null && YAMPVars.CORE.PlayerSource != null;
            if (flag)
            {
                SampleRate = YAMPVars.CORE.PlayerSource.WaveFormat.SampleRate;
                ChannelCount = YAMPVars.CORE.PlayerSource.WaveFormat.Channels;
                YAMPVars.SingleBlockNotificationStream.SingleBlockRead += SingleBlockNotificationStream_SingleBlockRead;
                YAMPVars.FftProvider = new FftProvider(ChannelCount, FftSize.Fft2048)
                {
                    WindowFunction = WindowFunctions.Hanning
                };
                FFTSIZE = YAMPVars.FftProvider.FftSize;
                SpectrumProvider = new BasicSpectrumProvider(ChannelCount, SampleRate, FFTSIZE);
                Spectrum = new VoicePrint3DSpectrum(FFTSIZE)
                {
                    SpectrumProvider = SpectrumProvider,
                    UseAverage = true,
                    PointCount = 200,
                    IsXLogScale = true,
                    ScalingStrategy = ScalingStrategy.Sqrt
                };
                splitContainer1.Panel2.Controls.Add(VolBand);
                splitContainer1.Panel2.Controls.Add(GainBand);
                VolBand.BandValue = (int)(YAMPVars.VolumeSource.Volume * 100f);
                GainBand.BandValue = (int)(YAMPVars.VolumeSource.Volume * 100f);
                VolBand.ValueChanged += VolBand_ValueChanged;
                GainBand.ValueChanged += GainBand_ValueChanged;
                Scope.Start();
                Spectrogram.Start();
                splitContainer1.Panel2.Controls.Add(VolBand);
                splitContainer1.Panel2.Controls.Add(GainBand);
                for (int i = 0; i < YAMPVars.EqualizerEffect.SampleFilters.Count; i++)
                {
                    EqualizerFilter item = YAMPVars.EqualizerEffect.SampleFilters[i];
                    int FilterFreq = (int)item.Filters[0].Frequency;
                    string FreqText = (FilterFreq < 1000) ? (FilterFreq.ToString() + " Hz") : ((FilterFreq / 1000).ToString() + " KHz");
                    EQBand EQBAND = new EQBand("EQ", -50, 50, 0, FreqText)
                    {
                        Dock = DockStyle.Left,
                        FooterText = FreqText,
                        Tag = i,
                    };
                    EQBAND.BandValue = (int)(item.AverageGainDB / MaxDB * EQBAND.BandMax);
                    EQBAND.ValueChanged += EQBAND_ValueChanged;
                    splitContainer1.Panel2.Controls.Add(EQBAND);
                }
                foreach (EQBand item in splitContainer1.Panel2.Controls.OfType<EQBand>())
                {
                    item.BringToFront();
                }

            }
        }

        private void SingleBlockNotificationStream_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {
            YAMPVars.FftProvider.Add(e.Left, e.Right);
            SpectrumProvider.Add(e.Left, e.Right);
        }

        private void GainBand_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.GainSource.Volume = (sender as EQBand).BandValue / 100f;
        }

        private void VolBand_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.VolumeSource.Volume = VolBand.BandValue / 100f;
        }

        private void EQBAND_ValueChanged(object sender, EventArgs e)
        {
            EQBand EQBAND;
            bool flag = (EQBAND = (sender as EQBand)) != null && YAMPVars.EqualizerEffect != null;
            if (flag)
            {
                double perc = EQBAND.BandValue / (double)EQBAND.BandMax;
                float value = (float)(perc * MaxDB);
                int filterIndex = (int)EQBAND.Tag;
                YAMPVars.EqualizerEffect.SampleFilters[filterIndex].AverageGainDB = value;
            }
        }

        private void GenerateVoice3DPrintSpectrum()
        {
            using (Graphics g = Graphics.FromImage(SpectroBitmap))
            {
                //CachedSoundSource
                pictureBox1.Image = null;
                bool flag = Spectrum.CreateVoicePrint3D(g, new RectangleF(0f, 0f, SpectroBitmap.Width, SpectroBitmap.Height), _xpos, Color.Black, 3f);
                if (flag)
                {
                    _xpos += 3;
                    bool flag2 = _xpos >= SpectroBitmap.Width;
                    if (flag2)
                    {
                        _xpos = 0;
                    }
                }
                pictureBox1.Image = SpectroBitmap;
            }
        }

        private void Scope_Tick(object sender, EventArgs e)
        {
            bool isNewDataAvailable = YAMPVars.FftProvider.IsNewDataAvailable;
            if (isNewDataAvailable)
            {
                buffer = new float[(int)YAMPVars.FftProvider.FftSize];
                YAMPVars.FftProvider.GetFftData(buffer);
                double[] DoubleBuffer = Array.ConvertAll(buffer, (float x) => (double)x);
                double[] zeropadded = Pad.ZeroPad(DoubleBuffer);
                double[] fftpower = Transform.FFTmagnitude(zeropadded);
                bool flag = formsPlot1.Plot.GetPlottables().Count() == 0;
                if (flag)
                {
                    signalPlot = formsPlot1.Plot.AddSignal(fftpower, 2.0 * fftpower.Length / YAMPVars.CORE.PlayerSource.WaveFormat.SampleRate, null, null);
                }
                else
                {
                    signalPlot.Ys = fftpower;
                }
                formsPlot1.Render(false, false);
            }
        }
        private void Spectrogram_Tick(object sender, EventArgs e)
        {
            GenerateVoice3DPrintSpectrum();
        }

        private void recenterEQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EQBand[] EQ2Bands = (from x in splitContainer1.Panel2.Controls.OfType<EQBand>()
                                 where x.Tag != null
                                 select x).ToArray();
            foreach (EQBand item in EQ2Bands)
            {
                item.BandValue = 0;
            }
        }
    }
}
