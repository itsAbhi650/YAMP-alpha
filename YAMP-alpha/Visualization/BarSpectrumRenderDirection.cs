namespace YAMP_alpha
{
    /// <summary>
    /// Defines the rendering direction for bar spectrum visualization
    /// </summary>
    public enum BarSpectrumRenderDirection
    {
        /// <summary>
        /// Bars grow horizontally from left to right (base at left edge)
        /// </summary>
        HorizontalLeftToRight = 0,

        /// <summary>
        /// Bars grow horizontally from right to left (base at right edge)
        /// </summary>
        HorizontalRightToLeft = 1,

        /// <summary>
        /// Bars grow vertically from bottom to top (base at bottom edge) - DEFAULT
        /// </summary>
        VerticalBottomToTop = 2,

        /// <summary>
        /// Bars grow vertically from top to bottom (base at top edge)
        /// </summary>
        VerticalTopToBottom = 3
    }
}
