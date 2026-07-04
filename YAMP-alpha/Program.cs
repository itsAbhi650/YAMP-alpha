using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YAMP_alpha
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var Source = CSCore.Codecs.CodecFactory.Instance.GetCodec("D:\\RN7 Backup\\Music\\The PropheC\\Solace\\The PropheC - Close.mp3");
            var ScratchSource = new YAMP.Scratch.ScratchPlaybackSource(Source.ToSampleSource());
            CSCore.SoundOut.WasapiOut wasapi = new CSCore.SoundOut.WasapiOut();
            wasapi.Initialize(ScratchSource.ToWaveSource());
            wasapi.Play();
            Application.Run(new YAMP.Scratch.ScratchTest(new YAMP.Scratch.ScratchController(ScratchSource)));
        }
    }
}
