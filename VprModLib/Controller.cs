using System.Text.Json.Serialization;

namespace VprModLib
{
    /// <summary>
    /// Part-level modifications of a parameter tracked over time such as "Clearness", "Pitch", or "Portamento" (but not "Velocity" or "Mouth"). Called a "Control Parameter" in official documentation. One of these is included if the parameter has been modified at all, and it includes all the events for that parameter.
    /// </summary>
    public class Controller
    {
        public ControllerType Type { get; }
        public string Name { get; }
        public List<ControllerEvent> Events { get; } = new List<ControllerEvent>();

        public Controller(ControllerType type)
        {
            Type = type;
            Name = type.ProjectName;
        }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedController : ISerialized<Controller>
    {
        public string name;
        public SerializedControllerEvent[] events;

        [JsonConstructor]
        public SerializedController(string name, SerializedControllerEvent[] events)
        {
            this.name = name;
            this.events = events;
        }
        public SerializedController(Controller model)
        {
            name = model.Name;
            events = model.Events.Select(e => new SerializedControllerEvent(e)).ToArray();
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(name)
                && ControllerType.Get(name) is ControllerType type
                && events != null
                && events.Length > 0
                && events.GroupBy(e => e.pos)
                    // All controller events have different positions (no events on top of each other).    
                    .All(group => group.Count() == 1
                    // Check if value is in range based on controller type.
                    && group.First().IsValid(type));                
        }

        public Controller ToModel()
        {
            var type = ControllerType.Get(name);            
            var model = new Controller(type);
            model.Events.AddRange(events.Select(e => e.ToModel()));
            return model;
        }
    }
}