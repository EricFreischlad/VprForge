namespace VprModLib
{
    /// <summary>
    /// A mode for the Robot Voice effect.
    /// </summary>
    public enum RobotVoiceMode
    {
        /// <summary>
        /// Large pitch changes are broken once at the midpoint, and last longer. Represented as Mode "0".
        /// </summary>
        HARD = 0,
        /// <summary>
        /// Large pitch changes are broken once at the midpoint, and last shorter. Represented as Mode "1".
        /// </summary>
        NORMAL = 1,
        /// <summary>
        /// All pitch changes are immediate. Represented as Mode "2".
        /// </summary>
        SOFT = 2,
    }
}