using CSCore.DSP;
using CSCore.Streams;
using CSCore.Streams.Effects;
using FftSharp;
using ScottPlot.Plottable;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using Spectrogram;
using System.Windows.Forms;
using YAMP_alpha.Controls;

namespace YAMP_alpha
{
    public partial class EqualizerDialog : Form
    {
        private float[] buffer;
        private double[] SpectroBuffer;
        private SignalPlot signalPlot;
        private BasicSpectrumProvider SpectrumProvider;
        private VoicePrint3DSpectrum Spectrum;
        private SpectrogramGenerator SpectroScott;
        private int SampleRate;
        private int ChannelCount;
        private FftSize FFTSIZE;
        private Bitmap SpectroBitmap;
        private int MaxDB = 12;
        private int _xpos;
        private Colormap[] Colormaps = Colormap.GetColormaps();
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
        private bool inBand;

        public EqualizerDialog()
        {
            InitializeComponent();
            formsPlot1.Plot.Frameless(true);
            formsPlot1.Plot.Margins(0);
            SpectroBitmap = new Bitmap(2000, 600);
        }

        private void EqualizerDialog_Load(object sender, EventArgs e)
        {
            CmbBx_ColMap.DataSource = Colormap.GetColormapNames();
            CmbBx_RotateGraph.DataSource = Enum.GetNames(typeof(RotateFlipType)).Select(x => x.Remove(0, "Rotate".Length)).ToArray();
            CmbBx_ImgMode.DataSource = Enum.GetNames(typeof(PictureBoxSizeMode));
            if (YAMPVars.CORE != null && YAMPVars.CORE.PlayerSource != null)
            {
                SampleRate = YAMPVars.CORE.PlayerSource.WaveFormat.SampleRate;
                ChannelCount = YAMPVars.CORE.PlayerSource.WaveFormat.Channels;
                YAMPVars.NotificationSource.BlockRead += NotificationSource_BlockRead;
                YAMPVars.SingleBlockNotificationStream.SingleBlockRead += SingleBlockNotificationStream_SingleBlockRead;
                YAMPVars.FftProvider = new FftProvider(ChannelCount, FftSize.Fft1024)
                {
                    WindowFunction = WindowFunctions.Hanning
                };
                FFTSIZE = YAMPVars.FftProvider.FftSize;
                SpectroScott = new SpectrogramGenerator(SampleRate, 1024, 512) { OffsetHz = 10000 };

                pictureBox1.Height = SpectroScott.Height;
                SpectroScott.SetFixedWidth(Pb_SpectrogramAdv.Width);
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
                GainBand.BandValue = (int)(YAMPVars.GainSource.Volume * 100f);
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
                    EQBAND.DoubleClick += EQBAND_DoubleClick;
                    splitContainer1.Panel2.Controls.Add(EQBAND);
                }
                foreach (EQBand item in splitContainer1.Panel2.Controls.OfType<EQBand>())
                {
                    item.BringToFront();
                }
            }
        }

        private void EQBAND_DoubleClick(object sender, EventArgs e)
        {
            inBand = true;
            var Band = (EQBand)sender;
            int filterIndex = (int)Band.Tag;
            var EQFilter = YAMPVars.EqualizerEffect.SampleFilters[filterIndex];
            using (var BandConfigurator = new EQBandConfigDialog(EQFilter))
            {
                BandConfigurator.ShowDialog();
            }
            Band.BandValue = GainToBand(EQFilter.AverageGainDB, Band.BandMax, MaxDB);
            inBand = false;
        }

        private void NotificationSource_BlockRead(object sender, BlockReadEventArgs<float> e)
        {
            SpectroBuffer = e.Data.Select(x => (double)(x * (int)NUD_Multiplier.Value)).ToArray();
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
            if (!inBand)
            {
                EQBand EQBAND;
                bool flag = (EQBAND = sender as EQBand) != null && YAMPVars.EqualizerEffect != null;
                if (flag)
                {
                    int filterIndex = (int)EQBAND.Tag;
                    YAMPVars.EqualizerEffect.SampleFilters[filterIndex].AverageGainDB = BandToGain(EQBAND.BandValue, EQBAND.BandMax, MaxDB);
                }
            }
        }

        private int GainToBand(double dB, int BandMax, int DBMax)
        {
            double perc = dB / DBMax;
            int BandValue = (int)(perc * BandMax);
            return BandValue;
        }

        private double BandToGain(int BandValue, int BandMax, int DBMax)
        {
            double perc = BandValue / (double)BandMax;
            double GainValue = perc * MaxDB;
            return GainValue;
        }

        private void GenerateVoice3DPrintSpectrum()
        {
            using (Graphics g = Graphics.FromImage(SpectroBitmap))
            {
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
                if (SpectroBuffer != null)
                {
                    SpectroScott.Add(SpectroBuffer);
                    if (SpectroScott.Width > 0)
                    {
                        Pb_SpectrogramAdv.Image?.Dispose();
                        var Bitmp = SpectroScott.GetBitmap((float)NUD_Brightness.Value, dB: ChkBx_Dcbl.Checked, roll: ChkBx_RollGraph.Checked);
                        Bitmp.RotateFlip((RotateFlipType)Enum.Parse(typeof(RotateFlipType), "Rotate" + CmbBx_RotateGraph.SelectedItem.ToString()));
                        Pb_SpectrogramAdv.Image = Bitmp;
                    }
                    formsPlot1.Render(false, false);
                    formsPlot1.Plot.AxisAutoY();
                }
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

        private void CmbBx_ColMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            SpectroScott.Colormap = Colormaps[CmbBx_ColMap.SelectedIndex];
        }

        private void CmbBx_ImgMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pb_SpectrogramAdv.SizeMode = (PictureBoxSizeMode)Enum.Parse(typeof(PictureBoxSizeMode), CmbBx_ImgMode.SelectedItem.ToString());
        }

        private void EqualizerDialog_SizeChanged(object sender, EventArgs e)
        {
            if (ChkBx_ResizeSpectro.Checked)
            {
                SpectroScott.SetFixedWidth(Pb_SpectrogramAdv.Width);
            }
        }

        private void NUD_OffHz_ValueChanged(object sender, EventArgs e)
        {
            SpectroScott.OffsetHz = (int)NUD_OffHz.Value;
        }
    }
}
