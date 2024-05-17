namespace VprModLib
{
    /// <summary>
    /// A tempo number. Vocaloid projects store tempo as an integer value 100 times larger than beats-per-minute (BPM * 100). This struct handles conversion to actual BPM and back.
    /// </summary>
    public readonly struct Tempo
    {
        /// <summary>
        /// Min tempo in editor = 20 BPM.
        /// </summary>
        public static readonly Tempo Min = CreateFromProjectValue(2000);
        /// <summary>
        /// Max tempo in editor = 300 BPM.
        /// </summary>
        public static readonly Tempo Max = CreateFromProjectValue(30000);

        // Instance members.

        /// <summary>
        /// The tempo as stored in the Vocaloid project file.
        /// </summary>
        public readonly int ProjectValue;
        /// <summary>
        /// The tempo in Beats Per Minute (BPM).
        /// </summary>
        public readonly float BpmValue;

        private Tempo(int projectValue, float bpmValue)
        {
            ProjectValue = projectValue;
            BpmValue = bpmValue;
        }

        public static Tempo CreateFromProjectValue(int projectValue)
        {
            return new Tempo(projectValue, projectValue / 100);
        }
        public static Tempo CreateFromBpmValue(float bpmValue)
        {
            // Truncation is intended.
            return new Tempo((int)(bpmValue * 100), bpmValue);
        }
        public static bool IsValid(int projectValue)
        {
            return projectValue >= Min.ProjectValue && projectValue <= Max.ProjectValue;
        }
    }
}