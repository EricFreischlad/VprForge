namespace VprModLib.MidiEffects
{
    public enum BreathParameterNames
    {
        /// <summary>
        /// The volume of the breaths that are inserted. 0 = very quiet, 10 = very loud.
        /// </summary>
        EXHALATION = 0,
        /// <summary>
        /// How frequently breaths are inserted. Modes are detailed in <see cref="BreathMode"/>.
        /// </summary>
        MODE = 1,
        /// <summary>
        /// The pitch/gender of the breaths that are inserted. Modes are detailed in <see cref="BreathType"/>.
        /// </summary>
        TYPE = 2,
    }
}