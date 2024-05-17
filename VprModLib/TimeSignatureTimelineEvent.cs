using System.Text.Json.Serialization;

namespace VprModLib
{
    public class TimeSignatureTimelineEvent
    {
        /// <summary>
        /// The index of the measure at which bar this new time signature begins.
        /// </summary>
        public int Bar { get; set; }
        /// <summary>
        /// The numerator for the time signature (top number. 3 of 3/4 time).
        /// </summary>
        public int Numer { get; set; }
        /// <summary>
        /// The denominator for the time signature (bottom number. 4 of 3/4 time).
        /// </summary>
        public int Denom { get; set; }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedTimeSignatureTimelineEvent : ISerialized<TimeSignatureTimelineEvent>
    {
        public int bar;
        public int numer;
        public int denom;

        [JsonConstructor]
        public SerializedTimeSignatureTimelineEvent(int bar, int numer, int denom)
        {
            this.bar = bar;
            this.numer = numer;
            this.denom = denom;
        }
        public SerializedTimeSignatureTimelineEvent(TimeSignatureTimelineEvent model)
        {
            bar = model.Bar;
            numer = model.Numer;
            denom = model.Denom;
        }

        public bool IsValid()
        {
            return bar >= 0
                && numer >= 0
                && denom >= 0;
        }

        public TimeSignatureTimelineEvent ToModel()
        {
            return new TimeSignatureTimelineEvent
            {
                Bar = bar,
                Numer = numer,
                Denom = denom
            };
        }
    }
}