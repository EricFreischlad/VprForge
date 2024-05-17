namespace VprModLib.AudioEffects
{
    public enum ReverbParameterName
    {
        /// <summary>
        /// Amount of time before the first echo. This knob is labeled "Delay" in the editor. 0.0 is 0.1ms, 1.0 is 200.0ms.
        /// </summary>
        INITIAL_DELAY = 0,
        /// <summary>
        /// Dry/wet mix. More dry = more original sound, More wet = more reverb. 0.0 is (0% reverb, 100% original sound), 1.0 is (100% reverb, 0% original sound).
        /// </summary>
        MIX = 1,
        /// <summary>
        /// How long the echoes last. 0.0 is 0.486s, 1.0 is 48.6s.
        /// </summary>
        REVERB_TIME = 2,
        /// <summary>
        /// What type of reverb to immitate. Represented by a double, but with only 3 valid values: 0.0 (hall), 0.5 (room), and 1.0 (plate).
        /// </summary>
        TYPE = 3,
    }
}