namespace VprModLib.AudioEffects
{
    public enum EqualizerParameterName
    {
        /// <summary>
        /// The frequency knob for the "high" band.
        /// </summary>
        HIGH_F = 0,
        /// <summary>
        /// The gain knob for the "high" band.
        /// </summary>
        HIGH_G = 1,
        /// <summary>
        /// The frequency knob for the "high-mid" band.
        /// </summary>
        HIGH_MID_F = 2,
        /// <summary>
        /// The gain knob for the "high-mid" band.
        /// </summary>
        HIGH_MID_G = 3,
        /// <summary>
        /// The bandwidth knob for the "high-mid" band.
        /// </summary>
        HIGH_MID_Q = 4,
        /// <summary>
        /// The "Low Cut" button (a.k.a. "high-pass filter"). If on, the bottom frequencies are rolled off.
        /// </summary>
        HPF = 5,
        /// <summary>
        /// The frequency knob for the "low" band.
        /// </summary>
        LOW_F = 6,
        /// <summary>
        /// The gain knob for the "low" band.
        /// </summary>
        LOW_G = 7,
        /// <summary>
        /// The frequency knob for the "low-mid" band.
        /// </summary>
        LOW_MID_F = 8,
        /// <summary>
        /// The gain knob for the "low-mid" band.
        /// </summary>
        LOW_MID_G = 9,
        /// <summary>
        /// The bandwidth knob for the "low-mid" band.
        /// </summary>
        LOW_MID_Q = 10,
    }
}