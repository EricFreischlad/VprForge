namespace VprModLib
{
    /// <summary>
    /// An amount of panning left or right. Vocaloid projects store panning as an integer value between -64 (100% left) and +64 (100% right). This struct handles conversion to normalized float values and back. Name comes from the project file layout and is short for "pan potentiometer".
    /// </summary>
    public readonly struct Panpot
    {
        /// <summary>
        /// Far Left in editor = -64.
        /// </summary>
        public static readonly Panpot FarLeft = CreateFromProjectValue(-64);
        /// <summary>
        /// Far right in editor = +64.
        /// </summary>
        public static readonly Panpot FarRight = CreateFromProjectValue(64);
        /// <summary>
        /// Center in editor = 0.
        /// </summary>
        public static readonly Panpot Center = CreateFromProjectValue(0);

        // Instance members.

        /// <summary>
        /// The panning as stored in the Vocaloid project file.
        /// </summary>
        public readonly int ProjectValue;
        /// <summary>
        /// The panning as a float normalized between -1.0 and 1.0.
        /// </summary>
        public readonly float NormalizedValue;

        private Panpot(int projectValue, float normalizedValue)
        {
            ProjectValue = projectValue;
            NormalizedValue = normalizedValue;
        }

        public static Panpot CreateFromProjectValue(int projectValue)
        {
            return new Panpot(projectValue, projectValue / 64f);
        }
        /// <summary>
        /// Create a new panning value from a float value between -1.0 (100% left) and 1.0 (100% right).
        /// </summary>
        public static Panpot CreateFromNormalizedValue(float normalizedValue)
        {
            // Truncation is intended.
            return new Panpot((int)(normalizedValue * 64), normalizedValue);
        }
        public static bool IsValid(int projectValue)
        {
            return projectValue >= FarLeft.ProjectValue && projectValue <= FarRight.ProjectValue;
        }
    }
}