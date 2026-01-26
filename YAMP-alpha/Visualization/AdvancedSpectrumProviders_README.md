# Advanced Spectrum Providers & Horizontal Bar Spectrum - Documentation

## Overview
Three new components have been added to enhance spectrum visualization capabilities:
1. **PeakHoldSpectrumProvider** - Shows peak values with decay
2. **SmoothingSpectrumProvider** - Applies temporal smoothing
3. **HorizontalBarSpectrum** - Displays bars horizontally (left to right)

---

## 1. PeakHoldSpectrumProvider

### Description
Maintains and displays peak frequency values that decay over time. Shows both the current spectrum AND falling peak indicators, similar to professional audio analyzers.

### Key Features
- **Peak Hold**: Captures maximum values and holds them
- **Gradual Decay**: Peaks fall gradually after hold period
- **Configurable**: Adjust hold time and decay rate
- **Professional Look**: Like studio audio equipment

### Constructor
```csharp
public PeakHoldSpectrumProvider(
    int channels, 
    int sampleRate, 
    FftSize fftSize,
    int peakHoldFrames = 15,      // Frames to hold peak before decay
    float peakDecayRate = 0.95f   // Decay rate per frame (0.5-1.0)
)
```

### Properties
- `PeakHoldFrames` - Number of frames to hold peak before decay starts
- `PeakDecayRate` - Decay multiplier (0.9 = fast, 0.98 = slow, 1.0 = no decay)

### Methods
- `GetPeakValues()` - Returns array of current peak values for rendering
- `ResetPeaks()` - Resets all peak values to zero

### Usage Example
```csharp
var peakProvider = new PeakHoldSpectrumProvider(
    channels: 2,
    sampleRate: 44100,
    fftSize: FftSize.Fft4096,
    peakHoldFrames: 20,    // Hold for 20 frames (~333ms at 60fps)
    peakDecayRate: 0.96f   // Slow decay
);

// Feed audio data
peakProvider.Add(leftSample, rightSample);

// Get FFT data with peaks
float[] fftData = new float[2048];
peakProvider.GetFftData(fftData, this);

// Get separate peak array for rendering peak indicators
float[] peaks = peakProvider.GetPeakValues();
```

### Decay Rate Guide
| Rate | Description | Use Case |
|------|-------------|----------|
| 0.90 | Very fast decay | Fast-paced music, quick response |
| 0.93 | Fast decay | Rock, pop music |
| 0.95 | Medium decay | General purpose (default) |
| 0.97 | Slow decay | Classical, ambient |
| 0.99 | Very slow decay | Visual effect, demonstration |

---

## 2. SmoothingSpectrumProvider

### Description
Applies exponential moving average smoothing to prevent jarring jumps between frames. Creates fluid, flowing visualizations with configurable attack (rise) and release (fall) times.

### Key Features
- **Temporal Smoothing**: Eliminates flickering and jitter
- **Attack/Release Control**: Separate rise and fall times
- **Natural Motion**: Like analog VU meters
- **Preset Profiles**: Pre-configured smoothing settings

### Constructor
```csharp
public SmoothingSpectrumProvider(
    int channels,
    int sampleRate,
    FftSize fftSize,
    float attackTime = 0.02f,    // Attack time in seconds (20ms)
    float releaseTime = 0.1f,    // Release time in seconds (100ms)
    float frameRate = 60f         // Expected frame rate
)
```

### Properties
- `AttackTime` - Rise time in seconds (how fast bars grow)
- `ReleaseTime` - Fall time in seconds (how fast bars shrink)

### Methods
- `ResetSmoothing()` - Resets all smoothed values to zero
- `SetSmoothingPreset(SmoothingPreset preset)` - Applies predefined smoothing profile

### Smoothing Presets
```csharp
public enum SmoothingPreset
{
    VeryFast,  // 10ms attack, 50ms release - Minimal smoothing
    Fast,      // 20ms attack, 80ms release - Light smoothing
    Medium,    // 30ms attack, 120ms release - Balanced (default)
    Slow,      // 50ms attack, 200ms release - Fluid motion
    VerySlow   // 80ms attack, 300ms release - Heavy smoothing
}
```

### Usage Example
```csharp
var smoothProvider = new SmoothingSpectrumProvider(
    channels: 2,
    sampleRate: 44100,
    fftSize: FftSize.Fft4096,
    attackTime: 0.03f,   // 30ms attack
    releaseTime: 0.12f,  // 120ms release
    frameRate: 60f
);

// Or use preset
smoothProvider.SetSmoothingPreset(SmoothingPreset.Medium);

// Feed audio data
smoothProvider.Add(leftSample, rightSample);

// Get smoothed FFT data
float[] fftData = new float[2048];
smoothProvider.GetFftData(fftData, this);
```

### Attack/Release Time Guide
| Time | Attack | Release | Use Case |
|------|--------|---------|----------|
| Very Fast | 10ms | 50ms | Gaming, reactive visuals |
| Fast | 20ms | 80ms | Electronic music |
| Medium | 30ms | 120ms | General music (default) |
| Slow | 50ms | 200ms | Ambient, classical |
| Very Slow | 80ms | 300ms | Decorative, smooth motion |

### How It Works
Uses exponential moving average formula:
```
coefficient = 1 - exp(-1 / (time * frameRate))
smoothed = previous + (current - previous) * coefficient
```

- **Attack** (rising): Uses faster coefficient for quick response
- **Release** (falling): Uses slower coefficient for gradual decay

---

## 3. HorizontalBarSpectrum

### Description
Displays frequency spectrum as horizontal bars extending from left to right. Each bar represents a frequency band, with bass at top and treble at bottom (or vice versa).

### Key Features
- **Horizontal Layout**: Bars grow from left to right
- **Peak Indicators**: Optional peak lines (with PeakHoldSpectrumProvider)
- **Size-Based Rendering**: Uses provided Size parameter
- **Gradient Support**: Color gradients from left to right
- **Compatible**: Works with any ISpectrumProvider

### Properties
- `BarCount` - Number of frequency bars
- `BarSpacing` - Spacing between bars in pixels
- `BarHeight` - Calculated height of each bar (read-only)
- `ShowPeakIndicators` - Enable/disable peak indicator lines
- `PeakIndicatorColor` - Color for peak lines

### Methods

#### CreateHorizontalBarSpectrum (Solid Color)
```csharp
public Bitmap CreateHorizontalBarSpectrum(
    Size size,           // Canvas size
    Brush brush,         // Bar color
    Color background,    // Background color
    bool highQuality     // Anti-aliasing
)
```

#### CreateHorizontalBarSpectrum (Gradient)
```csharp
public Bitmap CreateHorizontalBarSpectrum(
    Size size,
    Color color1,        // Start color (left)
    Color color2,        // End color (right)
    Color background,
    bool highQuality
)
```

### Usage Example

#### Basic Setup
```csharp
var horizontalSpectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    SpectrumProvider = spectrumProvider,
    BarCount = 30,
    BarSpacing = 2,
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Decibel,
    UseAverage = true
};

// Render
Bitmap image = horizontalSpectrum.CreateHorizontalBarSpectrum(
    new Size(800, 600),
    Color.Lime,
    Color.Red,
    Color.Black,
    true
);
```

#### With Peak Hold
```csharp
var peakProvider = new PeakHoldSpectrumProvider(2, 44100, FftSize.Fft4096);

var horizontalSpectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    SpectrumProvider = peakProvider,
    BarCount = 30,
    BarSpacing = 2,
    ShowPeakIndicators = true,      // Enable peak lines
    PeakIndicatorColor = Color.Red,
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Decibel
};
```

#### With Smoothing
```csharp
var smoothProvider = new SmoothingSpectrumProvider(2, 44100, FftSize.Fft4096);
smoothProvider.SetSmoothingPreset(SmoothingPreset.Medium);

var horizontalSpectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    SpectrumProvider = smoothProvider,
    BarCount = 40,
    BarSpacing = 1,
    ShowPeakIndicators = false,  // No peaks with smoothing
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Decibel
};
```

---

## Integration in YAMP-alpha

### PanelMode Enum
Added `HorizontalBars = 5` to cycle through visualization modes:
```
Cover ? Waveform ? Bars ? Circular ? Lyrics ? HorizontalBars ? (repeat)
```

### Setup in NewMain.cs
```csharp
private void SetupHorizontalBarsMode()
{
    const FftSize fftSize = FftSize.Fft4096;

    // Option 1: With peak hold (shows falling peaks)
    peakHoldProvider = new PeakHoldSpectrumProvider(
        channels: YAMPVars.CORE.Player.WaveSource.WaveFormat.Channels,
        sampleRate: YAMPVars.CORE.Player.WaveSource.WaveFormat.SampleRate,
        fftSize: fftSize,
        peakHoldFrames: 15,
        peakDecayRate: 0.95f
    );

    _horizontalBarSpectrum = new HorizontalBarSpectrum(fftSize)
    {
        SpectrumProvider = peakHoldProvider,
        BarCount = 30,
        BarSpacing = 2,
        ShowPeakIndicators = true,
        PeakIndicatorColor = Color.Red,
        IsXLogScale = true,
        ScalingStrategy = ScalingStrategy.Decibel
    };

    // Option 2: With smoothing (fluid motion)
    // smoothingProvider = new SmoothingSpectrumProvider(...);
    // _horizontalBarSpectrum.SpectrumProvider = smoothingProvider;
    // _horizontalBarSpectrum.ShowPeakIndicators = false;
}
```

---

## Comparison Table

| Provider | Peak Hold | Smoothing | Peaks Visible | Best For |
|----------|-----------|-----------|---------------|----------|
| BasicSpectrumProvider | ? | ? | ? | Raw data, technical |
| PeakHoldSpectrumProvider | ? | ? | ? | Professional analyzers |
| SmoothingSpectrumProvider | ? | ? | ? | Fluid animations |
| Peak + Smoothing* | ? | ? | ? | Best of both** |

*Can chain providers: `smoothingProvider` wrapping `peakHoldProvider` (advanced)
**Requires custom implementation

---

## Visual Examples

### Horizontal Bar Spectrum Layouts

#### With Peak Hold
```
Bass  ???????????????????????|     Peak indicator
Mid   ???????????????|
      ????????????????????|
Treble ???????|
```

#### With Smoothing
```
Bass  ???????????????????????
Mid   ????????????????
      ?????????????????????
Treble ?????????
```
(Bars transition smoothly, no sudden jumps)

---

## Performance Considerations

### PeakHoldSpectrumProvider
- **Overhead**: ~5-10% extra CPU (peak tracking)
- **Memory**: +FFT size/2 floats for peak array
- **Best**: Minimal impact, safe for real-time

### SmoothingSpectrumProvider
- **Overhead**: ~3-5% extra CPU (EMA calculation)
- **Memory**: +FFT size/2 floats for smoothed values
- **Best**: Very efficient, recommended

### HorizontalBarSpectrum
- **Rendering**: Similar to LineSpectrum
- **Peak Rendering**: +2-3% if enabled
- **Best**: No significant impact

---

## Tips & Best Practices

### For Professional Audio Analysis
```csharp
var peakProvider = new PeakHoldSpectrumProvider(2, 44100, FftSize.Fft4096, 
    peakHoldFrames: 20, 
    peakDecayRate: 0.96f
);

var spectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    SpectrumProvider = peakProvider,
    BarCount = 40,
    BarSpacing = 1,
    ShowPeakIndicators = true,
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Decibel
};
```

### For Aesthetic Visualization
```csharp
var smoothProvider = new SmoothingSpectrumProvider(2, 44100, FftSize.Fft4096);
smoothProvider.SetSmoothingPreset(SmoothingPreset.Slow);

var spectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    SpectrumProvider = smoothProvider,
    BarCount = 30,
    BarSpacing = 3,
    ShowPeakIndicators = false,
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Sqrt
};
```

### For Responsive Gaming/VJ
```csharp
var smoothProvider = new SmoothingSpectrumProvider(2, 44100, FftSize.Fft2048);
smoothProvider.SetSmoothingPreset(SmoothingPreset.VeryFast);

var spectrum = new HorizontalBarSpectrum(FftSize.Fft2048)
{
    SpectrumProvider = smoothProvider,
    BarCount = 20,
    BarSpacing = 2,
    IsXLogScale = false,  // Linear for faster response
    ScalingStrategy = ScalingStrategy.Linear
};
```

---

## Dependencies
- `CSCore` - Audio processing and FFT
- `CSCore.DSP` - FftProvider base class
- `System.Drawing` - Graphics rendering
- `.NET Framework 4.7.2`

## License
Same as parent project YAMP-alpha
