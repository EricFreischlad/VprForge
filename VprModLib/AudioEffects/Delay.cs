namespace VprModLib.AudioEffects
{
    public enum DelayParameterName
    {
        /// <summary>
        /// Dry/wet mix. More dry = more original sound, More wet = more echoes. 0.0 is (0% echoes, 100% original sound), 1.0 is (100% echoes, 0% original sound).
        /// </summary>
        DRY_WET = 6,
        /// <summary>
        /// Low pass filter frequency on echoes. Basically how much more muffled each echo is from the last.
        /// </summary>
        HIGH_DAMP = 4,
        /// <summary>
        /// The delay value for the left channel for manually-timed delay effects. "Left" under "Delay Time" in the editor when TempoSync is off.
        /// </summary>
        LCH_DELAY1 = 0,
        /// <summary>
        /// Gain for the feedback on the left channel. "Left" under "Feedback" in the editor.
        /// </summary>
        LCH_FB_GAIN = 1,
        /// <summary>
        /// Delay rate for the left channel while tempo-synched. "Left" under "Delay Time" in the editor while TempoSync is on. Cycles through clean subdivisions of a beat, then triplet subdivisions, then dotted subdivisions.
        /// </summary>
        LCH_SYNC_NOTE = 9,
        /// <summary>
        /// The stereo mode of the delay effect. Represented by a double, but with only 1 valid values: 0.0 (Stereo), and 1.0 ("Cross"). Cross mode is also known as "Ping-pong" mode.
        /// </summary>
        MODE = 7,
        /// <summary>
        /// The delay value for the right channel for manually-timed delay effects. "Right" under "Delay Time" in the editor when TempoSync is off.
        /// </summary>
        RCH_DELAY1 = 2,
        /// <summary>
        /// Gain for the feedback on the right channel. "Right" under "Feedback" in the editor.
        /// </summary>
        RCH_FB_GAIN = 3,
        /// <summary>
        /// Delay rate for the right channel while tempo-synched. "Right" under "Delay Time" in the editor while TempoSync is on. Cycles through clean subdivisions of a beat, then triplet subdivisions, then dotted subdivisions.
        /// </summary>
        RCH_SYNC_NOTE = 10,
        /// <summary>
        /// How separated the left and right channels are. 0.0 is 100% merged, 1.0 is 100% separated
        /// </summary>
        SPATIAL = 5,
        /// <summary>
        /// If on, the delay echoes will be timed based on the tempo of the song (using the LchSyncNote and RchSyncNote parameters). If off, the echo rate will be set manually (using the LchDelay and RchDelay parameters).
        /// </summary>
        TEMPO_SYNC = 8,
    }
}