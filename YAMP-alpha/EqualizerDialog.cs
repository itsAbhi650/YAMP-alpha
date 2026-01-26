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
        //private SignalPlot signalPlot;
        private BasicSpectrumProvider SpectrumProvider;
        private VoicePrint3DSpectrum Spectrum;
        private SpectrogramGenerator SpectroScott;
        private int SampleRate;
        private int ChannelCount;
        private FftSize FFTSIZE;
        private Bitmap SpectroBitmap;
        private static int MaxDB = 6;
        private int _xpos;
        private Colormap[] Colormaps = Colormap.GetColormaps();
        private EQBand[] FrequencyBands = null;
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
        }

        private void EqualizerDialog_Load(object sender, EventArgs e)
        {
            SpectroBitmap = new Bitmap(2140, 702);
            CmbBx_ColMap.DataSource = Colormap.GetColormapNames();
            CmbBx_RotateGraph.DataSource = Enum.GetNames(typeof(RotateFlipType)).Select(x => x.Remove(0, "Rotate".Length)).ToArray();
            CmbBx_ImgMode.DataSource = Enum.GetNames(typeof(PictureBoxSizeMode));
            GainMeter.Maximum = GainBand.BandMax;
            VolMeter.Maximum = VolBand.BandMax;
            //CmbBx_FftSize.DataSource = Enum.GetNames(typeof(FftSize));
            //CmbBx_FftSize.SelectedItem = "Fft4096";
            //CmbBx_FftSize.SelectedIndexChanged += CmbBx_FftSize_SelectedIndexChanged;

            if (YAMPVars.CORE != null && YAMPVars.CORE.PlayerSource != null)
            {
                ChannelCount = YAMPVars.CORE.PlayerSource.WaveFormat.Channels;
                YAMPVars.FftProvider = new FftProvider(ChannelCount, FftSize.Fft4096);
                FFTSIZE = YAMPVars.FftProvider.FftSize;
                SampleRate = YAMPVars.CORE.PlayerSource.WaveFormat.SampleRate;
                SpectrumProvider = new BasicSpectrumProvider(ChannelCount, SampleRate, FFTSIZE);
                
                YAMPVars.NotificationSource.BlockRead += NotificationSource_BlockRead;
                YAMPVars.SingleBlockNotificationStream.SingleBlockRead += SingleBlockNotificationStream_SingleBlockRead;
                
                CmbBx_ColMap.SelectedIndexChanged += new System.EventHandler(this.CmbBx_ColMap_SelectedIndexChanged);
                SpectroScott = new SpectrogramGenerator(SampleRate, (int)FFTSIZE, 512) { OffsetHz = 20 };
                Pb_Spectrogram.Height = SpectroScott.Height;
                SpectroScott.SetFixedWidth(Pb_SpectrogramAdv.Width);

                Spectrum = new VoicePrint3DSpectrum(FFTSIZE)
                {
                    SpectrumProvider = SpectrumProvider,
                    UseAverage = true,
                    PointCount = 200,
                    IsXLogScale = true,
                    ScalingStrategy = ScalingStrategy.Sqrt
                };

                GainBand.ValueChanged += GainBand_ValueChanged;
                VolBand.ValueChanged += VolBand_ValueChanged;

                GainBand.BandValue = (int)(YAMPVars.GainSource.Volume * 100f);
                VolBand.BandValue = (int)(YAMPVars.VolumeSource.Volume * 100f);

                splitContainer1.Panel2.Controls.Add(VolBand);
                splitContainer1.Panel2.Controls.Add(GainBand);
                
                //Scope.Start();
                //Spectrogram.Start();
                FrequencyBands = new EQBand[YAMPVars.EqualizerEffect.SampleFilters.Count];
                
                for (int i = 0; i < YAMPVars.EqualizerEffect.SampleFilters.Count; i++)
                {
                    EqualizerFilter item = YAMPVars.EqualizerEffect.SampleFilters[i];
                    int FilterFreq = (int)item.Filters[0].Frequency;
                    string FreqText = (FilterFreq < 1000) ? (FilterFreq.ToString() + " Hz") : ((FilterFreq / 1000).ToString() + " KHz");
                    FrequencyBands[i] = new EQBand("EQ", -6, 6, 0, FreqText)
                    {
                        Dock = DockStyle.Left,
                        FooterText = FreqText,
                        Tag = i,
                    };

                    FrequencyBands[i].BandValue = (int)(item.AverageGainDB / MaxDB * FrequencyBands[i].BandMax);
                    FrequencyBands[i].ValueChanged += EQBAND_ValueChanged;
                    FrequencyBands[i].DoubleClick += EQBAND_DoubleClick;
                    
                    splitContainer1.Panel2.Controls.Add(FrequencyBands[i]);
                    FrequencyBands[i].BringToFront();
                }

                EqCurve.Image = RenderEQCurve(FrequencyBands, EqCurve.Width, EqCurve.Height);
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
            SpectrumProvider.Add(e.Left, e.Right);
            YAMPVars.FftProvider.Add(e.Left, e.Right);
        }

        private void GainBand_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.GainSource.Volume = (sender as EQBand).BandValue / 100f;
            GainMeter.Level = GainBand.BandValue;
        }

        private void VolBand_ValueChanged(object sender, EventArgs e)
        {
            YAMPVars.VolumeSource.Volume = VolBand.BandValue / 100f;
            VolMeter.Level = VolBand.BandValue;
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
                    YAMPVars.EqualizerEffect.SampleFilters[filterIndex].AverageGainDB = BandToGain(EQBAND.BandValue, MaxDB, MaxDB);
                    EqCurve.Image = RenderEQCurve(FrequencyBands, EqCurve.Width, EqCurve.Height);
                }
            }
        }

        private int GainToBand(double dB, int BandMax, int DBMax)
        {
            double perc = dB / DBMax;
            int BandValue = (int)(perc * BandMax);
            return BandValue;
        }

        private static double BandToGain(int BandValue, int BandMax, int DBMax)
        {
            double perc = BandValue / (double)BandMax;
            double GainValue = perc * MaxDB;
            return GainValue;
        }

        private void GenerateVoice3DPrintSpectrum()
        {
            using (Graphics g = Graphics.FromImage(SpectroBitmap))
            {
                Pb_Spectrogram.Image = null;
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
                Pb_Spectrogram.Image = SpectroBitmap;
            }
        }

        public static Bitmap RenderEQCurve(EQBand[] bands, int width, int height)
        {
            height -= 5;
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Enable high-quality rendering
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                
                // Dark gradient background
                using (System.Drawing.Drawing2D.LinearGradientBrush bgBrush = 
                    new System.Drawing.Drawing2D.LinearGradientBrush(
                        new Rectangle(0, 0, width, height),
                        Color.FromArgb(15, 15, 20),
                        Color.FromArgb(5, 5, 10),
                        90f))
                {
                    g.FillRectangle(bgBrush, 0, 0, width, height);
                }

                if (bands == null || bands.Length < 2)
                    return bmp;

                int count = bands.Length;
                float bandSpacing = width / (float)(count - 1);
                float midY = height / 2f;

                // Draw horizontal grid lines
                using (Pen gridPen = new Pen(Color.FromArgb(40, 50, 50, 60), 1f))
                {
                    gridPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    
                    // +6dB, +3dB, 0dB, -3dB, -6dB lines
                    for (int db = -6; db <= 6; db += 3)
                    {
                        float y = midY - (db / 6.0f) * (height / 2f);
                        g.DrawLine(gridPen, 0, y, width, y);
                    }
                }

                // Draw 0dB reference line (brighter)
                using (Pen centerPen = new Pen(Color.FromArgb(80, 100, 100, 120), 1f))
                {
                    centerPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    g.DrawLine(centerPen, 0, midY, width, midY);
                }

                // Draw vertical grid lines at band positions
                using (Pen vertGridPen = new Pen(Color.FromArgb(30, 60, 60, 70), 1f))
                {
                    vertGridPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    for (int i = 0; i < count; i++)
                    {
                        float x = i * bandSpacing;
                        g.DrawLine(vertGridPen, x, 0, x, height);
                    }
                }

                // Create smooth interpolated curve using Catmull-Rom spline
                PointF[] controlPoints = new PointF[count];
                for (int i = 0; i < count; i++)
                {
                    float x = i * bandSpacing;
                    float y = midY - Convert.ToSingle((bands[i].BandValue / 6.0f) * (height / 2f));
                    controlPoints[i] = new PointF(x, y);
                }

                // Generate smooth curve with more points
                PointF[] smoothPoints = GenerateSmoothCurve(controlPoints, 20); // 20 points between each control point

                // Create gradient fill under the curve
                using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    // Add the curve to the path
                    path.AddCurve(smoothPoints, 0.5f); // 0.5f tension for smoother curves
                    
                    // Close the path to create a filled area
                    path.AddLine(smoothPoints[smoothPoints.Length - 1], new PointF(width, height));
                    path.AddLine(new PointF(width, height), new PointF(0, height));
                    path.AddLine(new PointF(0, height), smoothPoints[0]);

                    // Create gradient fill (green to transparent)
                    using (System.Drawing.Drawing2D.LinearGradientBrush fillBrush = 
                        new System.Drawing.Drawing2D.LinearGradientBrush(
                            new Rectangle(0, 0, width, height),
                            Color.FromArgb(60, 0, 255, 100),
                            Color.FromArgb(10, 0, 100, 50),
                            90f))
                    {
                        g.FillPath(fillBrush, path);
                    }
                }

                // Draw glow effect (outer glow)
                for (int i = 3; i > 0; i--)
                {
                    using (Pen glowPen = new Pen(Color.FromArgb(30 / i, 0, 255, 150), 2f + i * 2))
                    {
                        glowPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                        g.DrawCurve(glowPen, smoothPoints, 0.5f);
                    }
                }

                // Draw main curve (bright green)
                using (Pen curvePen = new Pen(Color.FromArgb(255, 0, 255, 100), 2f))
                {
                    curvePen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    curvePen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    curvePen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    g.DrawCurve(curvePen, smoothPoints, 0.5f);
                }

                // Draw control points as dots
                using (Brush dotBrush = new SolidBrush(Color.FromArgb(200, 0, 255, 150)))
                {
                    foreach (PointF point in controlPoints)
                    {
                        g.FillEllipse(dotBrush, point.X - 3, point.Y - 3, 6, 6);
                    }
                }

                // Draw inner highlight on control points
                using (Brush highlightBrush = new SolidBrush(Color.FromArgb(255, 150, 255, 200)))
                {
                    foreach (PointF point in controlPoints)
                    {
                        g.FillEllipse(highlightBrush, point.X - 1.5f, point.Y - 1.5f, 3, 3);
                    }
                }
            }

            return bmp;
        }

        /// <summary>
        /// Generates a smooth curve through control points using Catmull-Rom spline interpolation
        /// </summary>
        private static PointF[] GenerateSmoothCurve(PointF[] controlPoints, int pointsPerSegment)
        {
            if (controlPoints.Length < 2)
                return controlPoints;

            var smoothPoints = new System.Collections.Generic.List<PointF>();

            for (int i = 0; i < controlPoints.Length - 1; i++)
            {
                PointF p0 = i > 0 ? controlPoints[i - 1] : controlPoints[i];
                PointF p1 = controlPoints[i];
                PointF p2 = controlPoints[i + 1];
                PointF p3 = i < controlPoints.Length - 2 ? controlPoints[i + 2] : controlPoints[i + 1];

                for (int j = 0; j < pointsPerSegment; j++)
                {
                    float t = j / (float)pointsPerSegment;
                    PointF point = CatmullRomInterpolate(p0, p1, p2, p3, t);
                    smoothPoints.Add(point);
                }
            }

            // Add the last point
            smoothPoints.Add(controlPoints[controlPoints.Length - 1]);

            return smoothPoints.ToArray();
        }

        /// <summary>
        /// Catmull-Rom spline interpolation for smooth curves
        /// </summary>
        private static PointF CatmullRomInterpolate(PointF p0, PointF p1, PointF p2, PointF p3, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;

            float x = 0.5f * (
                (2f * p1.X) +
                (-p0.X + p2.X) * t +
                (2f * p0.X - 5f * p1.X + 4f * p2.X - p3.X) * t2 +
                (-p0.X + 3f * p1.X - 3f * p2.X + p3.X) * t3
            );

            float y = 0.5f * (
                (2f * p1.Y) +
                (-p0.Y + p2.Y) * t +
                (2f * p0.Y - 5f * p1.Y + 4f * p2.Y - p3.Y) * t2 +
                (-p0.Y + 3f * p1.Y - 3f * p2.Y + p3.Y) * t3
            );

            return new PointF(x, y);
        }

        private void Scope_Tick(object sender, EventArgs e)
        {
            bool isNewDataAvailable = YAMPVars.FftProvider.IsNewDataAvailable;
            if (isNewDataAvailable)
            {
                buffer = new float[4096];
                YAMPVars.FftProvider.GetFftData(buffer);

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


        private void spectrogramAdvONToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Scope.Enabled = spectrogramAdvONToolStripMenuItem.Checked;
            if (!Scope.Enabled)
            {
                Pb_SpectrogramAdv.Image = null;
                Pb_SpectrogramAdv.Invalidate();
                spectrogramAdvONToolStripMenuItem.Text = "Spectrogram Adv: OFF";
            }
            else
            {
                spectrogramAdvONToolStripMenuItem.Text = "Spectrogram Adv: ON";
            }
        }

        private void spectrogramONToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Spectrogram.Enabled = spectrogramONToolStripMenuItem.Checked;
            if (!Spectrogram.Enabled)
            {
                Pb_Spectrogram.Image = null;
                Pb_Spectrogram.Invalidate();
                spectrogramONToolStripMenuItem.Text = "Spectrogram: OFF";
            }
            else
            {
                spectrogramONToolStripMenuItem.Text = "Spectrogram: ON";
            }
        }
    }
}
