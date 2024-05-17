namespace VprModLib
{
    /// <summary>
    /// The breath mode for the Breath MIDI effect. This controls how often a breath is inserted.
    /// </summary>
    public enum BreathMode
    {
        /// <summary>
        /// Often inserts a breath. Represented as Mode "0".
        /// </summary>
        OFTEN = 0,
        /// <summary>
        /// Sometimes inserts a breath. Represented as Mode "1".
        /// </summary>
        SOMETIMES = 1,
        /// <summary>
        /// Rarely inserts a breath. Represented as Mode "2".
        /// </summary>
        RARELY = 2,
    }
}