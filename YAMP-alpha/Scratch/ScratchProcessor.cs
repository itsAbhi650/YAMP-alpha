// YAMP/Scratch/ScratchProcessor.cs  
using System;
using CSCore;

namespace YAMP.Scratch
{
    /// <summary>  
    /// Factory and coordinator.  
    /// Creates ScratchPlaybackSource + ScratchController and wires them together.  
    ///  
    /// Usage:  
    ///   var processor = new ScratchProcessor(myISampleSource);  
    ///   // Insert processor.ScratchSource into your pipeline  
    ///   var controller = processor.Controller;  
    /// </summary>  
    public sealed class ScratchProcessor : IDisposable
    {
        public ScratchPlaybackSource ScratchSource { get; }
        public ScratchController Controller { get; }

        public ScratchProcessor(ISampleSource source)
        {
            ScratchSource = new ScratchPlaybackSource(source);
            Controller = new ScratchController(ScratchSource);
        }

        public void Dispose() => ScratchSource.Dispose();
    }
}