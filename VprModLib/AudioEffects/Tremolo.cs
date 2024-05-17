namespace VprModLib.AudioEffects
{
    public enum TremoloParameterName
    {
        /// <summary>
        /// Amplitude of the tremolo LFO. "Depth" in the editor.
        /// </summary>
        AM_DEPTH = 0,
        /// <summary>
        /// Frequency for manually-timed tremolo effects. "Rate" in the editor while TempoSync is off.
        /// </summary>
        LFO_FREQ = 1,
        /// <summary>
        /// Possibly the pulse length of the pulse-modulation LFO for the tremolo effect. "PM Depth" in the editor.
        /// </summary>
        PM_DEPTH = 2,
        /// <summary>
        /// The shape type of the tremolo effect. Represented by a double, but with only 2 valid values: 0.0 ("Sine"), and 1.0 ("Square").
        /// </summary>
        SHAPE_TYPE = 3,
        /// <summary>
        /// Frequency for tempo-synched tremolo effects. "Rate" in the editor while TempoSync is on. Cycles through clean subdivisions of a beat, then triplet subdivisions, then dotted subdivisions.
        /// </summary>
        SYNC_NOTE = 4,
        /// <summary>
        /// If on, the tremolo LFO will be timed based on the tempo of the song (using the SyncNote parameter). If off, the LFO frequency will be set manually (using the LfoFreq parameter).
        /// </summary>
        TEMPO_SYNC = 5,
    }
}