namespace YAMP_alpha
{
    /// <summary>
    /// Defines the behavior mode for peak hold indicators
    /// </summary>
    public enum PeakHoldMode
    {
        /// <summary>
        /// Peaks fall gradually after hold period with decay rate
        /// Standard behavior for professional audio analyzers
        /// </summary>
        FallingPeak = 0,

        /// <summary>
        /// Peaks never fall - stay at maximum value forever until exceeded
        /// Useful for finding absolute maximum levels during a session
        /// </summary>
        NeverFall = 1,

        /// <summary>
        /// Peaks instantly follow the bar height (no independent tracking)
        /// Acts as a visual accent on top of each bar
        /// </summary>
        InstantFall = 2,

        /// <summary>
        /// No peaks displayed - equivalent to setting ShowPeakIndicators = false
        /// Provided for convenience when programmatically switching modes
        /// </summary>
        NoPeaks = 3
    }
}
