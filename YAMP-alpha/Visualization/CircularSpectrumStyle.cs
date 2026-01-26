namespace YAMP_alpha
{
    /// <summary>
    /// Defines the visual style for circular spectrum visualization
    /// </summary>
    public enum CircularSpectrumStyle
    {
        /// <summary>
        /// Full circle (360°) - bars radiate outward from center in all directions
        /// </summary>
        FullCircle = 0,

        /// <summary>
        /// Semi-circle (180°) - bars radiate from bottom, creating a gauge-like appearance
        /// </summary>
        SemiCircle = 1,

        /// <summary>
        /// Three-quarter arc (270°) - bars span 270 degrees around the circle
        /// </summary>
        ThreeQuarterArc = 2,

        /// <summary>
        /// Half arc bottom (180°) - bars span bottom half from left to right
        /// </summary>
        HalfArcBottom = 3,

        /// <summary>
        /// Half arc top (180°) - bars span top half from left to right
        /// </summary>
        HalfArcTop = 4,

        /// <summary>
        /// Mirror mode - bars grow both inward and outward from a middle ring
        /// </summary>
        MirrorMode = 5,

        /// <summary>
        /// Dual ring - inner ring for one representation, outer ring for another
        /// </summary>
        DualRing = 6,

        /// <summary>
        /// Quarter arc (90°) - bars span 90 degrees
        /// </summary>
        QuarterArc = 7,

        /// <summary>
        /// Symmetric mirror - mirrors frequency data at 180° for balanced circular appearance
        /// Displays same frequencies on opposite sides of the circle
        /// </summary>
        SymmetricMirror = 8,

        /// <summary>
        /// Musical range - limits frequency display to most musical range (60Hz-8kHz)
        /// Creates more balanced visualization by cutting off extreme high frequencies
        /// </summary>
        MusicalRange = 9
    }
}
