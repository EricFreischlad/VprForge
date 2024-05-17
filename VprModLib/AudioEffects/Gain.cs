using VprModLib.AudioEffects;

namespace VprModLib.AudioEffects
{
    public enum GainParameterName
    {
        /// <summary>
        /// The single slider value of the gain knob. Essentially, how much louder or quieter the sound will get. 0.0 = -24 dB, 0.7142857313156128 = +0 dB, 1.0 = +24 dB. The slider appears to have more precision on the "cut" values than the "boost" values.
        /// </summary>
        GAIN = 0,
    }
}