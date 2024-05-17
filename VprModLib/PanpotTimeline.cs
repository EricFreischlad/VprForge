using System.Text.Json.Serialization;

namespace VprModLib
{
    public class PanpotTimeline
    {
        /// <summary>
        /// Is the panning track hidden in the editor?
        /// </summary>
        public bool IsFolded { get; set; }
        public double Height { get; set; }
        public List<PanpotEvent> Events { get; } = new List<PanpotEvent>();
    }
}
namespace VprModLib.Serialization
{
    public class SerializedPanpotTimeline : ISerialized<PanpotTimeline>
    {
        public bool isFolded;
        public double height;
        public SerializedPanpotEvent[] events;

        [JsonConstructor]
        public SerializedPanpotTimeline(bool isFolded, double height, SerializedPanpotEvent[] events)
        {
            this.isFolded = isFolded;
            this.height = height;
            this.events = events;
        }
        public SerializedPanpotTimeline(PanpotTimeline model)
        {
            isFolded = model.IsFolded;
            height = model.Height;
            events = model.Events.Select(e => new SerializedPanpotEvent(e)).ToArray();
        }

        public bool IsValid()
        {
            return height >= 0.0
                && events is { }
                && events.All(e => e.IsValid());
        }

        public PanpotTimeline ToModel()
        {
            var model = new PanpotTimeline()
            {
                IsFolded = isFolded,
                Height = height,
            };
            model.Events.AddRange(events.Select(e => e.ToModel()));
            return model;
        }
    }
}