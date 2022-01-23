using System;
using System.Linq;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class PeakMeterDialog : Form
    {
        //private CSCore.Streams.PeakMeter PeakMeter;
        //public PeakMeter PeakMeter { get; set; }
        // private ISampleSource PeakMeterSampleSource;
        // bool AlmostEndOfStream;
        // bool EndOfStream;

        //  public event PropertyChangedEventHandler PropertyChanged;

        //private float[] _peaks;
        //public float[] Peaks { get { return _peaks; }set { _peaks = Core.AudioPeakMeter.ChannelPeakValues; OnPropertyChanged(); } }

        //public PeakMeterDialog(ISampleSource SampleSource)
        //{
        //    InitializeComponent();
        //    PeakMeterSampleSource = SampleSource;
        //    //var notificationSource = new SingleBlockNotificationStream(SampleSource, 15000);
        //    //notificationSource.SingleBlockStreamAlmostFinished += NotificationSource_SingleBlockStreamAlmostFinished;
        //    //notificationSource.SingleBlockRead += NotificationSource_SingleBlockRead;
        //    //SampleSource = notificationSource.ToWaveSource().ToSampleSource();
        //}

        //private void NotificationSource_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        //private void NotificationSource_SingleBlockStreamAlmostFinished(object sender, SingleBlockStreamAlmostFinishedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        public PeakMeterDialog()
        {
            InitializeComponent();
            //Core = core;
        }

        private void PeakMeterDialog_Load(object sender, EventArgs e)
        {
            if (YAMPVars.CORE != null && YAMPVars.AudioPeakMeter != null)
            {
                PeakFetch.Start();
            }
            //PeakMeter = Core.AudioPeakMeter;
            //PeakMeter.Interval = 25;
            //float[] PeakVals = YAMP_Vars.PeakVals;
            //trackBar1.Value = (int)(PeakVals[0] * 100F);
            //trackBar2.Value = (int)(PeakVals[1] * 100F);
            //var PlayerSampleSource = Core.GetSampleSource();
            //var notificationsource = new SingleBlockNotificationStream(PlayerSampleSource, 15000);
            //PlayerSampleSource = notificationsource.ToWaveSource(8).ToSampleSource();

            //PeakMeter = new PeakMeter(PlayerSampleSource)
            //{
            //    Interval = 25
            //};
            //PeakMeter.PeakCalculated += PeakMeter_PeakCalculated;
            //PeakMeter.PeakCalculated += PeakMeter_PeakCalculated;
            //PeakMeter = new PeakMeter(PeakMeterSampleSource)
            //{
            //    Interval = 25
            //};

            //PeakMeter.PeakCalculated += PeakMeter_PeakCalculated;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PeakFetch_Tick(object sender, EventArgs e)
        {
            int[] PeakVals = YAMPVars.AudioPeakMeter.ChannelPeakValues.Select(x => (int)(x * 100F)).ToArray();
            int Left = PeakVals[0];
            int Right = PeakVals[1];
            int avgpeak = (int)((Left + Right) / 2F);
            trackBar1.Value = Left;
            trackBar2.Value = Right;
            trackBar3.Value = avgpeak;
        }
    }
}
