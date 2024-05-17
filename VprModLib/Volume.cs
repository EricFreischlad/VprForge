namespace VprModLib
{
    /// <summary>
    /// A volume number. Vocaloid projects store volume as an integer value 10 times larger than decibels (dB * 10). This struct handles conversion to actual dB and back.
    /// </summary>
    public readonly struct Volume
    {
        /// <summary>
        /// Min volume in editor = -89.8 dB.
        /// </summary>
        public static readonly Volume Min = CreateFromProjectValue(-898);
        /// <summary>
        /// Max volume in editor = +6.0 dB.
        /// </summary>
        public static readonly Volume Max = CreateFromProjectValue(60);
        /// <summary>
        /// Default volume in editor = +0.0 dB.
        /// </summary>
        public static readonly Volume Default = CreateFromProjectValue(60);

        // Instance members.

        /// <summary>
        /// The volume as stored in the Vocaloid project file.
        /// </summary>
        public readonly int ProjectValue;
        /// <summary>
        /// The volume in decibels (dB).
        /// </summary>
        public readonly float DbValue;

        private Volume(int projectValue, float dbValue)
        {
            ProjectValue = projectValue;
            DbValue = dbValue;
        }

        public static Volume CreateFromProjectValue(int projectValue)
        {
            return new Volume(projectValue, projectValue / 10);
        }
        /// <summary>
        /// Create a new volume value from a float value between -89.8 (minimum volume) and 6.0 (maximum volume).
        /// </summary>
        public static Volume CreateFromDbValue(float dbValue)
        {
            // Truncation is intended.
            return new Volume((int)(dbValue * 10), dbValue);
        }
        public static bool IsValid(int projectValue)
        {
            return projectValue >= Min.ProjectValue && projectValue <= Max.ProjectValue;
        }
    }
}