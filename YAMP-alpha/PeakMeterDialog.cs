using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.CoreAudioAPI;
using CSCore.DSP;
using System.Windows.Forms;
using CSCore.Streams;
using CSCore;

namespace YAMP_alpha
{
    public partial class PeakMeterDialog : Form
    {
        public YAMP_Core Core { get; set; }
        public PeakMeter PeakMeter { get; set; }
        bool AlmostEndOfStream;
        bool EndOfStream;
        
        public PeakMeterDialog()
        {
            InitializeComponent();
        }

        public PeakMeterDialog(YAMP_Core Core)
        {
            InitializeComponent();
            this.Core = Core;

            var PlayerSampleSource = Core.PlayerSource.ToSampleSource();
            var notificationSource = new SingleBlockNotificationStream(PlayerSampleSource, 15000);
            notificationSource.SingleBlockStreamAlmostFinished += NotificationSource_SingleBlockStreamAlmostFinished;
            notificationSource.SingleBlockRead += NotificationSource_SingleBlockRead;
            PlayerSampleSource = notificationSource.ToWaveSource().ToSampleSource();
            PeakMeter = new PeakMeter(Core.PlayerSource.ToSampleSource())
            {
                Interval = 25
            };
            PeakMeter.PeakCalculated += PeakMeter_PeakCalculated;
        }

        private void NotificationSource_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NotificationSource_SingleBlockStreamAlmostFinished(object sender, SingleBlockStreamAlmostFinishedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public PeakMeterDialog(IWaveSource PlayerSource)
        {
            InitializeComponent();
            var PlayerSampleSource = PlayerSource.ToSampleSource();
            var notificationSource = new SingleBlockNotificationStream(PlayerSampleSource, 15000);
            PlayerSampleSource = notificationSource.ToWaveSource(8).ToSampleSource();
            PeakMeter = new PeakMeter(PlayerSource.ToSampleSource())
            {
                Interval = 25
            };
            PeakMeter.PeakCalculated += PeakMeter_PeakCalculated;
        }

        private void PeakMeter_PeakCalculated(object sender, PeakEventArgs e)
        {
            float[] PeakVals = PeakMeter.ChannelPeakValues;
            trackBar1.Value = (int)(PeakVals[0]* 100F);
            trackBar2.Value = (int)(PeakVals[1] * 100F);
        }

        private void PeakMeterDialog_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
