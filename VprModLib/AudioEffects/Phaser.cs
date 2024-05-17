namespace VprModLib.AudioEffects
{
    public enum PhaserParameterName
    {
        /// <summary>
        /// Amplitude of the phaser LFO. "Depth" in the editor.
        /// </summary>
        DEPTH = 0,
        /// <summary>
        /// Dry/wet mix. More dry = more original sound, More wet = more phaser. 0.0 is (0% phaser, 100% original sound), 1.0 is (100% phaser, 0% original sound).
        /// </summary>
        DRY_WET = 1,
        /// <summary>
        /// Feedback for the phaser. 0.0 is -63 dB, 1.0 is +63 dB.
        /// </summary>
        FEEDBACK_LEVEL = 2,
        /// <summary>
        /// Phase offset for the phaser. 0.0 is "0", 1.0 is "127" (units unknown).
        /// </summary>
        PHASE_OFFSET = 3,
        /// <summary>
        /// Frequency when manually timed. "Rate" in the editor while TempoSync is off.
        /// </summary>
        RATE = 4,
        /// <summary>
        /// The stage for the phaser. 0.0 is "3", 1.0 is "10".
        /// </summary>
        STAGE = 5,
        /// <summary>
        /// Frequency for tempo-synched phaser effects. "Rate" in the editor while TempoSync is on. Cycles through clean subdivisions of a beat, then triplet subdivisions, then dotted subdivisions.
        /// </summary>
        SYNC_NOTE = 6,
        /// <summary>
        /// If on, the phaser LFO will be timed based on the tempo of the song (using the SyncNote parameter). If off, the LFO frequency will be set manually (using the Rate parameter).
        /// </summary>
        TEMPO_SYNC = 7,
        /// <summary>
        /// The type of phaser effect. Represented by a double, but with only 2 valid values: 0.0 ("Phaser 1"), and 1.0 ("Phaser 2").
        /// </summary>
        TYPE = 8,
    }
}