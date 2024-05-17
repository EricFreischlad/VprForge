using System.Text.Json.Serialization;

namespace VprModLib
{
    public class PanpotEvent
    {
        public NoteTime Pos { get; set; }
        public Panpot Value { get; set; }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedPanpotEvent : ISerialized<PanpotEvent>
    {
        public int pos;
        public int value;

        [JsonConstructor]
        public SerializedPanpotEvent(int pos, int value)
        {
            this.pos = pos;
            this.value = value;
        }
        public SerializedPanpotEvent(PanpotEvent model)
        {
            pos = model.Pos.FrameIndex;
            value = model.Value.ProjectValue;
        }

        public bool IsValid()
        {
            return pos >= 0
                && Panpot.IsValid(value);
        }

        public PanpotEvent ToModel()
        {
            return new PanpotEvent()
            {
                Pos = new NoteTime(pos),
                Value = Panpot.CreateFromProjectValue(value),
            };
        }
    }
}