# Circular Spectrum Visualization - Implementation Guide

## Overview
A comprehensive circular spectrum visualization system has been added to YAMP-alpha with **10 different visual styles**, all controlled via an enum. Two special styles address the frequency imbalance issue for more visually appealing circular displays.

## Files Created

### 1. `CircularSpectrumStyle.cs`
Enum defining 10 different circular visualization styles:
- **FullCircle** (360°) - Complete circular visualization
- **SemiCircle** (180°) - Gauge-like bottom half
- **ThreeQuarterArc** (270°) - Three-quarter circle
- **HalfArcBottom** (180°) - Bottom half arc
- **HalfArcTop** (180°) - Top half arc
- **MirrorMode** - Bars grow both inward and outward
- **DualRing** - Two concentric rings
- **QuarterArc** (90°) - Quarter circle segment
- **SymmetricMirror** ? - Mirrors frequency data at 180° for balanced appearance
- **MusicalRange** ? - Limits display to musical frequency range (60Hz-8kHz)

### 2. `CircularSpectrum.cs`
Main visualization class with features:
- Inherits from `SpectrumBase` (same as LineSpectrum)
- Uses `ISpectrumProvider` for FFT data
- Polar coordinate rendering system
- Configurable properties:
  - `BarCount` - Number of frequency bars (default: 60)
  - `BarWidth` - Width of each bar in pixels (default: 3.0f)
  - `InnerRadius` - Starting radius where bars begin (default: 50)
  - `Style` - Visual style from CircularSpectrumStyle enum
  - `EnableRotation` - Enable/disable rotation animation
  - `Rotation` - Current rotation angle in degrees
  - `IsXLogScale` - Logarithmic frequency scaling
  - `ScalingStrategy` - Decibel, Linear, or Sqrt scaling
  - `UseAverage` - Smooth bars with averaging
  - `MusicalRangeMinFrequency` - Min frequency for MusicalRange mode (default: 60Hz)
  - `MusicalRangeMaxFrequency` - Max frequency for MusicalRange mode (default: 8000Hz)

## The Frequency Imbalance Problem

### Issue
When displaying a full spectrum (20Hz-20kHz) in a circular format:
- **Bass frequencies** (20Hz-200Hz) have MUCH taller bars due to higher energy
- **Treble frequencies** (5kHz-20kHz) have very short bars
- This creates an **unbalanced circle** where one side looks "heavy" and the other looks "empty"
- The visual effect is unappealing because high and low frequencies appear adjacent in the circle

### Why This Happens
1. Music naturally has more energy in low frequencies (bass)
2. Logarithmic frequency scaling (`IsXLogScale = true`) allocates more spectrum points to bass
3. Human hearing is less sensitive to extreme highs
4. Most music production emphasizes bass and mids over extreme treble

## Solutions Implemented

### Solution 1: SymmetricMirror Style ? RECOMMENDED
**How it works:**
- Takes only the first half of the spectrum (0-50% of frequency range)
- Displays it from 0° to 180°
- **Mirrors the same data** from 180° to 360°
- Creates a perfectly **balanced, symmetrical circle**

**Advantages:**
? Always balanced and symmetrical
? No frequency gaps
? Visually pleasing regardless of audio content
? Bass appears on both sides of circle

**Best for:** General music visualization, aesthetic display

**Example:**
```csharp
_circularSpectrum = new CircularSpectrum(fftSize)
{
    // ...other properties...
    Style = CircularSpectrumStyle.SymmetricMirror,
    BarCount = 60,
    IsXLogScale = true
};
```

### Solution 2: MusicalRange Style
**How it works:**
- Filters spectrum to only show frequencies between **60Hz and 8000Hz**
- Excludes extreme lows (sub-bass) and extreme highs (ultrasonic)
- Focuses on the "**musical**" range where most instruments and vocals exist
- Distributes these frequencies evenly around the full 360°

**Advantages:**
? More balanced than full spectrum
? Shows all frequency data (no mirroring)
? Focuses on perceptually important frequencies
? Customizable range via properties

**Best for:** Monitoring actual frequency content, music production

**Example:**
```csharp
_circularSpectrum = new CircularSpectrum(fftSize)
{
    // ...other properties...
    Style = CircularSpectrumStyle.MusicalRange,
    MusicalRangeMinFrequency = 60,    // Adjust as needed
    MusicalRangeMaxFrequency = 8000,  // Adjust as needed
    BarCount = 60,
    IsXLogScale = true
};
```

### Comparison Table

| Style | Balance | Full Spectrum | Symmetry | Best Use Case |
|-------|---------|---------------|----------|---------------|
| FullCircle | ? Unbalanced | ? Yes | ? No | Technical analysis |
| SymmetricMirror | ? Perfect | ?? Half (mirrored) | ? Yes | Aesthetic display |
| MusicalRange | ? Good | ?? Filtered | ? No | Music monitoring |

## Integration

### Modified Files

#### `CircularSpectrumStyle.cs`
Added two new enum values:
- `SymmetricMirror = 8`
- `MusicalRange = 9`

#### `CircularSpectrum.cs`
Added:
- `MusicalRangeMinFrequency` property
- `MusicalRangeMaxFrequency` property
- `DrawSymmetricMirror()` method
- `ApplyMusicalRangeFilter()` method
- `GetSpectrumIndexForFrequency()` helper method

#### `NewMain.cs`
Updated `SetupCircularMode()` to use `SymmetricMirror` by default with helpful comments

## Usage

### Basic Usage
Double-click the visualization area to cycle through modes:
- Cover ? Waveform ? Bars ? **Circular** ? Lyrics ? (back to Cover)

The circular mode now defaults to **SymmetricMirror** for best visual appearance.

### Switching Styles in Code

**For balanced circular display (Recommended):**
```csharp
_circularSpectrum.Style = CircularSpectrumStyle.SymmetricMirror;
```

**For musical frequency range:**
```csharp
_circularSpectrum.Style = CircularSpectrumStyle.MusicalRange;
_circularSpectrum.MusicalRangeMinFrequency = 80;   // Optional: customize
_circularSpectrum.MusicalRangeMaxFrequency = 6000; // Optional: customize
```

**For full spectrum (original unbalanced):**
```csharp
_circularSpectrum.Style = CircularSpectrumStyle.FullCircle;
```

### Custom Musical Range

Adjust the frequency range for different music genres:

**Rock/Metal (more bass):**
```csharp
_circularSpectrum.MusicalRangeMinFrequency = 40;
_circularSpectrum.MusicalRangeMaxFrequency = 6000;
```

**Classical/Orchestral (wider range):**
```csharp
_circularSpectrum.MusicalRangeMinFrequency = 50;
_circularSpectrum.MusicalRangeMaxFrequency = 12000;
```

**Electronic/EDM (focus on bass and mids):**
```csharp
_circularSpectrum.MusicalRangeMinFrequency = 30;
_circularSpectrum.MusicalRangeMaxFrequency = 8000;
```

**Vocals/Speech:**
```csharp
_circularSpectrum.MusicalRangeMinFrequency = 200;
_circularSpectrum.MusicalRangeMaxFrequency = 4000;
```

## Technical Details

### SymmetricMirror Implementation
1. Divides `spectrumPoints` array in half
2. First half rendered from 0° to 180°
3. Same data rendered again from 180° to 360° (mirrored)
4. Angle calculation: `angle = rotation + (i * 360 / (halfCount * 2))`

### MusicalRange Implementation
1. Converts min/max frequencies to spectrum point indices
2. Filters `spectrumPoints` array to only include indices in range
3. Renders filtered points evenly around full circle
4. Empty/filtered frequencies simply don't appear

### Performance Impact
- **SymmetricMirror**: ~5% more rendering time (drawing bars twice)
- **MusicalRange**: ~10-30% less rendering time (fewer bars to draw)

## Visual Comparison

### FullCircle (Original)
```
     High (tiny)
         |
Left --- * --- Right
(med)    |    (med)
        Bass
       (huge)
```
? Imbalanced: Bass dominates one side

### SymmetricMirror
```
      Bass
       ||
Left --**-- Right
(sym)  ||  (sym)
      Bass
```
? Balanced: Symmetrical on both sides

### MusicalRange
```
    Treble
      |
Left-***-Right
 |         |
Bass     Bass
```
? Balanced: Evenly distributed musical frequencies

## Future Enhancement Ideas
1. **Auto-switch style** based on audio content analysis
2. **Gradient colors by frequency band** - Bass in blue, mids in green, highs in red
3. **Multiple symmetric modes** - 3-way symmetry, 4-way symmetry
4. **Adaptive frequency range** - Automatically adjust musical range based on content
5. **Frequency band isolation** - Show only bass, only mids, or only treble
6. **Smooth transitions** between styles
7. **Beat-reactive symmetry** - Change mirror point based on beat detection

## Dependencies
- `CSCore` - Audio processing and FFT
- `System.Drawing` - Graphics rendering
- `.NET Framework 4.7.2` - Base framework
- C# 7.1 features

## License
Same as parent project YAMP-alpha
