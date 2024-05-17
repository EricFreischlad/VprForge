namespace VprModLib.AudioEffects
{
    public enum DistortionParameterName
    {
        /// <summary>
        /// Distortion pre-gain boost. 0.0 is "0", 1.0 is "10".
        /// </summary>
        DRIVE = 0,
        /// <summary>
        /// Seems like a low-pass filter after the Drive before the Output. Sounds like the cutoff frequency. Represented by a double, but with only 4 valid values: 0.0 (off), 0.33 ("1"), 0.67 ("2") and 1.0 ("3").
        /// </summary>
        LO_FI_TYPE = 1,
        /// <summary>
        /// Seems like a low-pass filter after the Drive before the Output. Sounds like the resonance. 0.0 is "0", 1.0 is "10".
        /// </summary>
        LO_FI_AMOUNT = 2,
        /// <summary>
        /// Distortion post-gain boost. 0.0 is -12 dB, 1.0 is +12 dB.
        /// </summary>
        OUTPUT = 3,
    }
}