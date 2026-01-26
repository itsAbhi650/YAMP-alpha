# Bar Spectrum Render Direction - Multi-Directional Visualization

## Overview

The `HorizontalBarSpectrum` now supports **four different rendering directions** via the `BarSpectrumRenderDirection` enum, allowing bars to grow in any direction from any edge of the canvas.

---

## BarSpectrumRenderDirection Enum

```csharp
public enum BarSpectrumRenderDirection
{
    HorizontalLeftToRight = 0,    // Bars grow left ? right (base at left)
    HorizontalRightToLeft = 1,    // Bars grow right ? left (base at right)
    VerticalBottomToTop = 2,      // Bars grow bottom ? top (base at bottom) DEFAULT
    VerticalTopToBottom = 3       // Bars grow top ? bottom (base at top)
}
```

---

## Visual Representations

### 1. HorizontalLeftToRight
```
?? Base (Left Edge)
?
? Bass    ????????????????
?         ????????????
? Mid     ??????????
?         ????????
? Treble  ????
?
??????????????????????????
   LEFT              RIGHT
```
- **Base**: Left edge
- **Growth**: Rightward ?
- **Layout**: Bass at top, Treble at bottom (or vice versa)

### 2. HorizontalRightToLeft
```
                Base (Right Edge) ??
                                   ?
        ????????????????    Bass   ?
            ????????????           ?
              ??????????    Mid    ?
                  ????????         ?
                      ????  Treble ?
                                   ?
????????????????????????????????????
RIGHT              LEFT
```
- **Base**: Right edge
- **Growth**: Leftward ?
- **Layout**: Bass at top, Treble at bottom (or vice versa)

### 3. VerticalBottomToTop (DEFAULT)
```
     TOP
      ?
      ? ??? (Treble)
      ? ????
      ? ????????
      ? ???????????????? (Bass)
      ?
?????????????????????????? Base (Bottom Edge)
Bass ??????????????? Treble
```
- **Base**: Bottom edge
- **Growth**: Upward ?
- **Layout**: Bass on left, Treble on right
- **Standard spectrum analyzer layout**

### 4. VerticalTopToBottom
```
?????????????????????????? Base (Top Edge)
      ?
      ? ???????????????? (Bass)
      ? ????????
      ? ????
      ? ??? (Treble)
      ?
   BOTTOM
Bass ??????????????? Treble
```
- **Base**: Top edge
- **Growth**: Downward ?
- **Layout**: Bass on left, Treble on right

---

## Usage

###Basic Setup
```csharp
var spectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    SpectrumProvider = peakHoldProvider,
    BarCount = 30,
    BarSpacing = 2,
    ShowPeakIndicators = true,
    
    // Set render direction
    RenderDirection = BarSpectrumRenderDirection.VerticalBottomToTop,  // DEFAULT
    
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Decibel
};
```

### Changing Direction Dynamically
```csharp
// Change to horizontal left-to-right
spectrum.RenderDirection = BarSpectrumRenderDirection.HorizontalLeftToRight;

// Change to vertical top-to-bottom
spectrum.RenderDirection = BarSpectrumRenderDirection.VerticalTopToBottom;

// Back to default (vertical bottom-to-top)
spectrum.RenderDirection = BarSpectrumRenderDirection.VerticalBottomToTop;
```

---

## Technical Details

### Automatic Bar Sizing

The bar thickness is automatically calculated based on the render direction:

**Vertical modes (BottomToTop, TopToBottom):**
- Bars distributed across **width**
- Bar thickness = `(width - spacing × (barCount + 1)) / barCount`

**Horizontal modes (LeftToRight, RightToLeft):**
- Bars distributed across **height**
- Bar thickness = `(height - spacing × (barCount + 1)) / barCount`

### Bar Rectangle Calculation

Each direction has its own calculation method:

#### HorizontalLeftToRight
```csharp
RectangleF(
    0,                      // X: Left edge (base)
    yPosition,              // Y: Vertical placement
    barValue,               // Width: Bar length (grows right)
    barThickness            // Height: Bar thickness
)
```

#### HorizontalRightToLeft
```csharp
RectangleF(
    width - barValue,       // X: Right edge minus length
    yPosition,              // Y: Vertical placement
    barValue,               // Width: Bar length (grows left)
    barThickness            // Height: Bar thickness
)
```

#### VerticalBottomToTop
```csharp
RectangleF(
    xPosition,              // X: Horizontal placement
    height - barValue,      // Y: Bottom edge minus height
    barThickness,           // Width: Bar thickness
    barValue                // Height: Bar length (grows up)
)
```

#### VerticalTopToBottom
```csharp
RectangleF(
    xPosition,              // X: Horizontal placement
    0,                      // Y: Top edge (base)
    barThickness,           // Width: Bar thickness
    barValue                // Height: Bar length (grows down)
)
```

### Peak Indicator Orientation

Peak indicators automatically adapt to the render direction:

| Direction | Peak Line Orientation |
|-----------|----------------------|
| HorizontalLeftToRight | Vertical line |
| HorizontalRightToLeft | Vertical line |
| VerticalBottomToTop | Horizontal line |
| VerticalTopToBottom | Horizontal line |

---

## Use Cases

### VerticalBottomToTop (Default)
? **Best for**: Standard spectrum analyzers, music players
- Matches professional audio software
- Intuitive bass-left, treble-right layout
- Natural upward growth metaphor

### VerticalTopToBottom
? **Best for**: Inverted displays, creative visualizations
- Unique aesthetic
- Waterfall-like effect
- Good for overhead displays

### HorizontalLeftToRight
? **Best for**: Wide displays, horizontal layouts
- Efficient use of wide screens
- Side-panel visualizations
- Landscape-oriented displays

### HorizontalRightToLeft
? **Best for**: RTL (right-to-left) UI layouts, creative effects
- Matches RTL language interfaces
- Unique visual style
- Complements right-aligned elements

---

## Examples

### Classic Spectrum Analyzer
```csharp
var spectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    RenderDirection = BarSpectrumRenderDirection.VerticalBottomToTop,
    BarCount = 40,
    BarSpacing = 2,
    ShowPeakIndicators = true,
    PeakIndicatorColor = Color.Red,
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Decibel
};
```

### Horizontal Sidebar Visualizer
```csharp
var spectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    RenderDirection = BarSpectrumRenderDirection.HorizontalLeftToRight,
    BarCount = 30,
    BarSpacing = 1,
    ShowPeakIndicators = true,
    PeakIndicatorColor = Color.Cyan,
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Sqrt
};
```

### Waterfall Effect
```csharp
var spectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    RenderDirection = BarSpectrumRenderDirection.VerticalTopToBottom,
    BarCount = 50,
    BarSpacing = 1,
    ShowPeakIndicators = false,
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Linear
};
```

### RTL Layout
```csharp
var spectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    RenderDirection = BarSpectrumRenderDirection.HorizontalRightToLeft,
    BarCount = 25,
    BarSpacing = 3,
    ShowPeakIndicators = true,
    PeakIndicatorColor = Color.Orange,
    IsXLogScale = true,
    ScalingStrategy = ScalingStrategy.Decibel
};
```

---

## Gradient Behavior

Gradients are automatically oriented based on render direction:

| Direction | Gradient Orientation |
|-----------|---------------------|
| HorizontalLeftToRight | Horizontal (left ? right) |
| HorizontalRightToLeft | Horizontal (left ? right)* |
| VerticalBottomToTop | Horizontal (left ? right) |
| VerticalTopToBottom | Horizontal (left ? right) |

*Note: Gradient always flows left?right. For custom gradients, create your own brush.

---

## Performance

All render directions have **identical performance**:
- Same number of draw calls
- Same memory usage
- Only position calculations differ
- No additional overhead

---

## Migration from Previous Version

**Old code (before multi-direction support):**
```csharp
var spectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    // No direction specified - defaulted to vertical bottom-to-top
};
```

**New code (explicit direction):**
```csharp
var spectrum = new HorizontalBarSpectrum(FftSize.Fft4096)
{
    RenderDirection = BarSpectrumRenderDirection.VerticalBottomToTop,  // Explicit
};
```

**Backward Compatibility**: ? Fully compatible - default is `VerticalBottomToTop`

---

## Files Modified/Created

1. **`BarSpectrumRenderDirection.cs`** (NEW)
   - Enum defining four render directions

2. **`HorizontalBarSpectrum.cs`** (MODIFIED)
   - Added `RenderDirection` property
   - Added four bar creation methods
   - Updated `UpdateFrequencyMapping()` for dynamic sizing
   - Updated `DrawPeakIndicator()` for all directions

---

## Build Status

? **Build Successful** - All render directions work correctly!

---

## Summary

The `HorizontalBarSpectrum` is now a true **multi-directional spectrum visualizer** supporting:
- ? 4 render directions
- ? Automatic bar sizing
- ? Adaptive peak indicators
- ? Backward compatible
- ? Zero performance overhead
- ? Dynamic direction switching

Choose the direction that best fits your UI layout and visual style! ??
