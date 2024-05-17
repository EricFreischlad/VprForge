using System.Text.Json.Serialization;

namespace VprModLib
{
    public class TempoTimeline
    {
        /// <summary>
        /// Is the tempo track hidden in the editor?
        /// </summary>
        public bool IsFolded { get; set; }
        public double Height { get; set; }
        /// <summary>
        /// Is global tempo enabled? (Or is it handled by an external DAW?)
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// Global tempo value. Can be enabled or disabled.
        /// </summary>
        public Tempo Value { get; set; }
        public List<TempoTimelineEvent> Events { get; } = new List<TempoTimelineEvent>();
    }
}
namespace VprModLib.Serialization
{
    public class SerializedTempoTimeline : ISerialized<TempoTimeline>
    {
        public bool isFolded;
        public double height;
        public SerializedTempoTimelineGlobal global;
        public SerializedTempoTimelineEvent[] events;

        [JsonConstructor]
        public SerializedTempoTimeline(bool isFolded, double height, SerializedTempoTimelineGlobal global, SerializedTempoTimelineEvent[] events)
        {
            this.isFolded = isFolded;
            this.height = height;
            this.global = global;
            this.events = events;
        }
        public SerializedTempoTimeline(TempoTimeline model)
        {
            isFolded = model.IsFolded;
            height = model.Height;
            global = new SerializedTempoTimelineGlobal()
            {
                isEnabled = model.IsEnabled,
                value = model.Value.ProjectValue,
            };
            events = model.Events.Select(e => new SerializedTempoTimelineEvent(e)).ToArray();
        }

        public bool IsValid()
        {
            return height >= 0.0
                && global is { }
                && Tempo.IsValid(global.value)
                && events is { }
                && events.All(e => e.IsValid())
                // And no events share a position.
                && events.GroupBy(e => e.pos)
                    .All(g => g.Count() == 1);
        }

        public TempoTimeline ToModel()
        {
            var model = new TempoTimeline()
            {
                IsFolded = isFolded,
                Height = height,
                IsEnabled = global.isEnabled,
                Value = Tempo.CreateFromProjectValue(global.value),
            };
            model.Events.AddRange(events.Select(e => e.ToModel()));
            return model;
        }
    }
    public class SerializedTempoTimelineGlobal
    {
        // Serialization handled in the one class that uses this.

        public bool isEnabled;
        public int value;
    }
}