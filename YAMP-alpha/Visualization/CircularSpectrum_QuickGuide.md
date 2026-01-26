# Circular Spectrum - Balanced Visualization Guide

## Quick Reference: Solving the Frequency Imbalance

### The Problem
When music is displayed in a full circular spectrum, **bass frequencies** (low) create very tall bars while **treble frequencies** (high) create tiny bars. This makes the circle look unbalanced and unappealing.

**Why?** Music has more energy in bass + logarithmic frequency scaling + human hearing sensitivity.

---

## ? Solution 1: SymmetricMirror (RECOMMENDED for Aesthetics)

### What It Does
Mirrors the frequency data at the 180° point, creating perfect symmetry.

### Visual Effect
```
Before (FullCircle):          After (SymmetricMirror):
    tiny                           TALL
     |                              ||
med --*-- med         ?        med--**--med
     |                              ||
   TALL                            TALL
```

### Code
```csharp
_circularSpectrum.Style = CircularSpectrumStyle.SymmetricMirror;
```

### Pros & Cons
? Always perfectly balanced
? Beautiful, symmetrical appearance
? Works with any audio
?? Shows same frequencies on both sides (not unique data on each side)

---

## ? Solution 2: MusicalRange (RECOMMENDED for Analysis)

### What It Does
Cuts off extreme frequencies and only shows the "musical" range (60Hz-8kHz by default).

### Visual Effect
```
Before (Full 20Hz-20kHz):     After (Musical 60Hz-8kHz):
  
  tinytiny                        balanced
   |    |                             |
bass--*--mid--*--treble    ?     bass--*--mid--*--treble
   |                                  |
  TALL                             TALL (but more even)
```

### Code
```csharp
_circularSpectrum.Style = CircularSpectrumStyle.MusicalRange;

// Optional: Customize the range
_circularSpectrum.MusicalRangeMinFrequency = 60;    // Default
_circularSpectrum.MusicalRangeMaxFrequency = 8000;  // Default
```

### Pros & Cons
? Shows actual frequency distribution
? More balanced than full spectrum
? Customizable frequency range
?? Filters out some data (sub-bass and ultrasonic)

---

## Style Comparison Chart

| Style | Visual Balance | Shows Full Data | Use Case |
|-------|---------------|-----------------|----------|
| **SymmetricMirror** ? | Perfect (5/5) | Half (mirrored) | Aesthetic display, eye candy |
| **MusicalRange** ? | Good (4/5) | Filtered (60Hz-8kHz) | Music monitoring, mixing |
| FullCircle | Poor (2/5) | Yes (all frequencies) | Technical analysis only |

---

## Quick Setup Examples

### For Visual Beauty (Default)
```csharp
_circularSpectrum = new CircularSpectrum(fftSize)
{
    SpectrumProvider = spectrumProvider,
    Style = CircularSpectrumStyle.SymmetricMirror,
    BarCount = 60,
    BarWidth = 3.0f,
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Decibel
};
```

### For Music Production/Analysis
```csharp
_circularSpectrum = new CircularSpectrum(fftSize)
{
    SpectrumProvider = spectrumProvider,
    Style = CircularSpectrumStyle.MusicalRange,
    MusicalRangeMinFrequency = 60,
    MusicalRangeMaxFrequency = 8000,
    BarCount = 80,
    BarWidth = 2.5f,
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Decibel
};
```

### For Bass-Heavy Music (EDM, Hip-Hop)
```csharp
_circularSpectrum = new CircularSpectrum(fftSize)
{
    SpectrumProvider = spectrumProvider,
    Style = CircularSpectrumStyle.MusicalRange,
    MusicalRangeMinFrequency = 30,   // Include sub-bass
    MusicalRangeMaxFrequency = 6000,  // Less treble
    BarCount = 60,
    IsXLogScale = true
};
```

### For Vocals/Podcasts
```csharp
_circularSpectrum = new CircularSpectrum(fftSize)
{
    SpectrumProvider = spectrumProvider,
    Style = CircularSpectrumStyle.MusicalRange,
    MusicalRangeMinFrequency = 200,   // Focus on voice range
    MusicalRangeMaxFrequency = 4000,
    BarCount = 40,
    IsXLogScale = false  // Linear scale for speech
};
```

---

## All Available Styles

1. **FullCircle** - Full 360° (unbalanced)
2. **SemiCircle** - Bottom half (180°)
3. **ThreeQuarterArc** - 270° arc
4. **HalfArcBottom** - Bottom 180°
5. **HalfArcTop** - Top 180°
6. **MirrorMode** - Bars grow inward + outward
7. **DualRing** - Two concentric rings
8. **QuarterArc** - 90° segment
9. **SymmetricMirror** ? - Mirrored for balance
10. **MusicalRange** ? - Frequency cutoff for balance

---

## Tips for Best Results

### For Maximum Visual Appeal
1. Use `SymmetricMirror` style
2. Set `BarCount` between 60-120 (more = smoother)
3. Enable `UseAverage = true` for smoother bars
4. Use `ScalingStrategy.Decibel` for better dynamics
5. Enable `EnableRotation = true` for mesmerizing effect

### For Accurate Frequency Analysis
1. Use `MusicalRange` style
2. Adjust min/max frequencies for your genre
3. Use `IsXLogScale = true` for musical perception
4. Higher `BarCount` (80-120) for detail
5. Keep rotation off to read frequencies easily

### Performance Tips
- Lower `BarCount` for slower machines (30-40)
- Use `MusicalRange` to reduce rendering load
- Disable `UseAverage` if experiencing lag
- Turn off `EnableRotation` if CPU is struggling

---

## Troubleshooting

**Problem: Circle still looks unbalanced**
- Switch to `SymmetricMirror` style
- OR adjust `MusicalRangeMaxFrequency` lower (try 5000-6000 Hz)

**Problem: Too few bars visible**
- Increase `BarCount` property
- Check if `MusicalRange` filter is too narrow

**Problem: Bars too thick/thin**
- Adjust `BarWidth` property (1.5f - 5.0f)

**Problem: Circle too small/large**
- Adjust `InnerRadius` property

---

## Summary

?? **Use SymmetricMirror** - For beautiful, balanced circular visualization (default)
?? **Use MusicalRange** - For accurate frequency monitoring with better balance
?? **Avoid FullCircle** - Unless you specifically need all frequencies displayed

The default configuration in `SetupCircularMode()` uses **SymmetricMirror** for the best visual experience!
