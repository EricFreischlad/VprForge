using System.Text.Json.Serialization;

namespace VprModLib
{
    public class ControllerEvent
    {
        public NoteTime Pos { get; set; }
        public int Value { get; set; }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedControllerEvent : ISerialized<ControllerEvent>
    {
        public int pos;
        public int value;

        [JsonConstructor]
        public SerializedControllerEvent(int pos, int value)
        {
            this.pos = pos;
            this.value = value;
        }
        public SerializedControllerEvent(ControllerEvent model)
        {
            pos = model.Pos.FrameIndex;
            value = model.Value;
        }

        public bool IsValid()
        {
            // NOTE: Controller value range depends on the controller. Call the other IsValid() method for more accuracy.
            return pos >= 0;
        }
        public bool IsValid(ControllerType controllerType)
        {
            return IsValid()
                && value >= controllerType.MinValue
                && value <= controllerType.MaxValue;
        }

        public ControllerEvent ToModel()
        {
            return new ControllerEvent()
            {
                Pos = new NoteTime(pos),
                Value = value,
            };
        }
    }
}