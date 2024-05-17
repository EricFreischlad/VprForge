using System.Text.Json.Serialization;

namespace VprModLib
{
    public class TimeSignatureTimeline
    {
        public bool IsFolded { get; set; }
        public List<TimeSignatureTimelineEvent> Events { get; } = new List<TimeSignatureTimelineEvent>();
    }
}
namespace VprModLib.Serialization
{
    public class SerializedTimeSignatureTimeline : ISerialized<TimeSignatureTimeline>
    {
        public bool isFolded;
        public SerializedTimeSignatureTimelineEvent[] events;

        [JsonConstructor]
        public SerializedTimeSignatureTimeline(bool isFolded, SerializedTimeSignatureTimelineEvent[] events)
        {
            this.isFolded = isFolded;
            this.events = events;
        }
        public SerializedTimeSignatureTimeline(TimeSignatureTimeline model)
        {
            isFolded = model.IsFolded;
            events = model.Events.Select(e => new SerializedTimeSignatureTimelineEvent(e)).ToArray();
        }

        public bool IsValid()
        {
            return events is { }
                && events.All(e => e.IsValid());
        }

        public TimeSignatureTimeline ToModel()
        {
            var model = new TimeSignatureTimeline()
            {
                IsFolded = isFolded,
            };
            model.Events.AddRange(events.Select(e => e.ToModel()));
            return model;
        }
    }
}