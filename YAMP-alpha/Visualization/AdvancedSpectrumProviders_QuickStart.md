# Quick Start Guide - Advanced Spectrum Providers

## ?? Quick Setup Examples

### 1. Peak Hold Spectrum (Professional Look)
```csharp
// Create peak hold provider
var peakProvider = new PeakHoldSpectrumProvider(
    channels: 2,
    sampleRate: 44100,
    fftSize: FftSize.Fft4096,
    peakHoldFrames: 15,    // Hold peaks for 250ms @ 60fps
    peakDecayRate: 0.95f   // Medium decay
);

// Create horizontal bar spectrum
var spectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    SpectrumProvider = peakProvider,
    BarCount = 30,
    BarSpacing = 2,
    ShowPeakIndicators = true,          // ? Show peak lines
    PeakIndicatorColor = Color.Red,
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Decibel
};

// Feed audio
peakProvider.Add(leftSample, rightSample);

// Render
Bitmap image = spectrum.CreateHorizontalBarSpectrum(
    new Size(800, 600),
    Color.Lime,     // Left side color
    Color.Red,      // Right side color
    Color.Black,    // Background
    true            // High quality
);
```

---

### 2. Smooth Flowing Spectrum (Aesthetic)
```csharp
// Create smoothing provider
var smoothProvider = new SmoothingSpectrumProvider(
    channels: 2,
    sampleRate: 44100,
    fftSize: FftSize.Fft4096,
    attackTime: 0.03f,    // 30ms rise
    releaseTime: 0.12f,   // 120ms fall
    frameRate: 60f
);

// Or use preset
smoothProvider.SetSmoothingPreset(SmoothingPreset.Medium);

// Create spectrum
var spectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    SpectrumProvider = smoothProvider,
    BarCount = 30,
    BarSpacing = 2,
    ShowPeakIndicators = false,         // ? No peaks with smoothing
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Decibel
};

// Feed audio
smoothProvider.Add(leftSample, rightSample);

// Render
Bitmap image = spectrum.CreateHorizontalBarSpectrum(
    new Size(800, 600),
    Color.Cyan,
    Color.Purple,
    Color.Black,
    true
);
```

---

## ?? Visual Styles

### Peak Hold Effect
```
Bass   ????????????????????????|  ? Peak indicator
       ???????????????????|
Mid    ??????????????|
       ????????????????????|
Treble ????????|
```
? Professional audio analyzer look
? Shows both current and maximum values
? Peaks decay gradually

### Smoothing Effect
```
Bass   ????????????????????????
       ????????????????????
Mid    ???????????????
       ????????????????????
Treble ?????????
```
? Fluid, flowing motion
? No flickering or jitter
? Natural analog feel

---

## ?? Configuration Presets

### Smoothing Presets
```csharp
// Very Fast - Gaming, reactive visuals
smoothProvider.SetSmoothingPreset(SmoothingPreset.VeryFast);

// Fast - Electronic music
smoothProvider.SetSmoothingPreset(SmoothingPreset.Fast);

// Medium - General purpose (DEFAULT)
smoothProvider.SetSmoothingPreset(SmoothingPreset.Medium);

// Slow - Ambient, classical
smoothProvider.SetSmoothingPreset(SmoothingPreset.Slow);

// Very Slow - Decorative, smooth motion
smoothProvider.SetSmoothingPreset(SmoothingPreset.VerySlow);
```

### Peak Decay Rates
```csharp
// Fast decay (0.90) - Fast-paced music
peakHoldFrames: 10, peakDecayRate: 0.90f

// Medium decay (0.95) - General purpose (DEFAULT)
peakHoldFrames: 15, peakDecayRate: 0.95f

// Slow decay (0.97) - Classical, ambient
peakHoldFrames: 20, peakDecayRate: 0.97f

// Very slow (0.99) - Visual effect
peakHoldFrames: 30, peakDecayRate: 0.99f
```

---

## ?? Use Cases

| Use Case | Provider | Settings |
|----------|----------|----------|
| **Studio Monitor** | PeakHoldSpectrumProvider | 20 frames, 0.96 decay, peaks ON |
| **Music Player** | SmoothingSpectrumProvider | Medium preset, peaks OFF |
| **DJ Software** | PeakHoldSpectrumProvider | 15 frames, 0.93 decay, peaks ON |
| **Visualizer** | SmoothingSpectrumProvider | Slow preset, peaks OFF |
| **Gaming** | SmoothingSpectrumProvider | VeryFast preset, peaks OFF |
| **Analysis** | PeakHoldSpectrumProvider | 25 frames, 0.98 decay, peaks ON |

---

## ?? Bar Configuration

### Compact (Low Bar Count)
```csharp
BarCount = 20,
BarSpacing = 3
```
Good for: Small displays, minimalist look

### Balanced (Medium)
```csharp
BarCount = 30,   // DEFAULT
BarSpacing = 2
```
Good for: General use, most music

### Detailed (High Bar Count)
```csharp
BarCount = 50,
BarSpacing = 1
```
Good for: Analysis, large displays

---

## ?? Color Schemes

### Classic Green-Red
```csharp
CreateHorizontalBarSpectrum(size, Color.Lime, Color.Red, Color.Black, true)
```

### Neon Cyan-Purple
```csharp
CreateHorizontalBarSpectrum(size, Color.Cyan, Color.Magenta, Color.Black, true)
```

### Blue Spectrum
```csharp
CreateHorizontalBarSpectrum(size, Color.DeepSkyBlue, Color.DodgerBlue, Color.Black, true)
```

### Fire
```csharp
CreateHorizontalBarSpectrum(size, Color.Yellow, Color.OrangeRed, Color.Black, true)
```

### Monochrome
```csharp
CreateHorizontalBarSpectrum(size, new SolidBrush(Color.White), Color.Black, true)
```

---

## ?? Troubleshooting

### Bars too flickery
? Use `SmoothingSpectrumProvider`
? Set `UseAverage = true`
? Try Medium or Slow preset

### Bars too slow/laggy
? Use faster smoothing preset (Fast/VeryFast)
? Lower attack/release times
? Use BasicSpectrumProvider for raw data

### Peak indicators not showing
? Make sure using `PeakHoldSpectrumProvider`
? Set `ShowPeakIndicators = true`
? Check `PeakIndicatorColor` is visible

### Bars too tall/short
? Adjust `ScalingStrategy` (try Sqrt or Linear)
? Change `BarCount` (fewer = taller bars)
? Increase canvas size

---

## ?? Pro Tips

1. **Combine with CircularSpectrum** - Use different providers for different visualizations
2. **Match to Genre** - Use Fast for EDM, Slow for Classical
3. **Test Frame Rate** - Adjust `frameRate` parameter if using different refresh rates
4. **Reset on Track Change** - Call `ResetPeaks()` or `ResetSmoothing()` when changing tracks
5. **Memory Efficient** - All providers reuse buffers internally

---

## ?? Genre-Specific Settings

### Electronic / EDM
```csharp
SmoothingPreset.Fast
BarCount = 25
IsXLogScale = true
ScalingStrategy = ScalingStrategy.Linear
```

### Rock / Metal
```csharp
PeakHold: 12 frames, 0.92f decay
BarCount = 30
IsXLogScale = true
ScalingStrategy = ScalingStrategy.Decibel
```

### Classical / Orchestral
```csharp
SmoothingPreset.Slow
BarCount = 40
IsXLogScale = true
ScalingStrategy = ScalingStrategy.Decibel
```

### Hip-Hop / Rap
```csharp
PeakHold: 18 frames, 0.94f decay
BarCount = 25
IsXLogScale = true
ScalingStrategy = ScalingStrategy.Sqrt
```

### Ambient / Chill
```csharp
SmoothingPreset.VerySlow
BarCount = 20
IsXLogScale = false
ScalingStrategy = ScalingStrategy.Sqrt
```

---

## ?? Usage in YAMP-alpha

Double-click the visualization area to cycle through modes:
```
Cover ? Waveform ? Bars ? Circular ? Lyrics ? HorizontalBars ? (repeat)
```

The HorizontalBars mode uses **PeakHoldSpectrumProvider** by default with peak indicators enabled.

To switch to smoothing, edit `SetupHorizontalBarsMode()` in `NewMain.cs`.

---

## ?? Related Files

- `PeakHoldSpectrumProvider.cs` - Peak hold implementation
- `SmoothingSpectrumProvider.cs` - Smoothing implementation  
- `HorizontalBarSpectrum.cs` - Horizontal rendering
- `AdvancedSpectrumProviders_README.md` - Full documentation
- `NewMain.cs` - Integration code

---

**Build Status**: ? Compiles successfully
**Dependencies**: CSCore, System.Drawing, .NET Framework 4.7.2
