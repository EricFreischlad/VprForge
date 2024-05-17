using System.Text.Json.Serialization;

namespace VprModLib
{
    public class VolumeTimeline
    {
        /// <summary>
        /// Is the volume track hidden in the editor?
        /// </summary>
        public bool IsFolded { get; set; }
        public double Height { get; set; }
        public List<VolumeEvent> Events { get; } = new List<VolumeEvent>();
    }
}
namespace VprModLib.Serialization
{
    public class SerializedVolumeTimeline : ISerialized<VolumeTimeline>
    {
        public bool isFolded;
        public double height;
        public SerializedVolumeEvent[] events;

        [JsonConstructor]
        public SerializedVolumeTimeline(bool isFolded, double height, SerializedVolumeEvent[] events)
        {
            this.isFolded = isFolded;
            this.height = height;
            this.events = events;
        }
        public SerializedVolumeTimeline(VolumeTimeline model)
        {
            isFolded = model.IsFolded;
            height = model.Height;
            events = model.Events.Select(e => new SerializedVolumeEvent(e)).ToArray();
        }

        public bool IsValid()
        {
            return height >= 0.0
                && events is { }
                && events.All(e => e.IsValid());
        }

        public VolumeTimeline ToModel()
        {
            var model = new VolumeTimeline()
            {
                IsFolded = isFolded,
                Height = height,
            };
            model.Events.AddRange(events.Select(e => e.ToModel()));
            return model;
        }
    }
}