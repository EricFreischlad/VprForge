using System.Text.Json.Serialization;

namespace VprModLib
{
    public class AudioPartRegion
    {
        // TODO: It's unclear exactly how these values are determined. Investigate.
        // Adjusting the length of the audio part in the editor results in clean floating-point values.
        // Example: Manually making the part take 2 beats results in the "end" property being "1000.0".
        // Is the unit just 1/500 of a beat? Is it tied to tempo? Is it affected by time sig?
        // If it's in miliseconds, then it's a really dodgey way to store that number. Could've been samples as an int.
        // At 192k audio rate, signed int max is hit after about 3.1 hours.
        // I guess floating point is better, sort of, but at the cost of precision which is kinda important...
        
        public double Begin { get; set; }
        public double End { get; set; }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedAudioPartRegion : ISerialized<AudioPartRegion>
    {
        public double begin;
        public double end;

        [JsonConstructor]
        public SerializedAudioPartRegion(double begin, double end)
        {
            this.begin = begin;
            this.end = end;
        }
        public SerializedAudioPartRegion(AudioPartRegion model)
        {
            begin = model.Begin;
            end = model.End;
        }

        public bool IsValid()
        {
            return begin >= 0.0
                && end >= 0.0
                && begin <= end;
        }

        public AudioPartRegion ToModel()
        {
            return new AudioPartRegion
            {
                Begin = begin,
                End = end
            };
        }
    }
}