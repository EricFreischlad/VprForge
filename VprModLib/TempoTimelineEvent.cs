using System.Text.Json.Serialization;

namespace VprModLib
{
    public class TempoTimelineEvent
    {
        public NoteTime Pos { get; set; }
        public Tempo Value { get; set; }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedTempoTimelineEvent : ISerialized<TempoTimelineEvent>
    {
        public int pos;
        public int value;

        [JsonConstructor]
        public SerializedTempoTimelineEvent(int pos, int value)
        {
            this.pos = pos;
            this.value = value;
        }
        public SerializedTempoTimelineEvent(TempoTimelineEvent model)
        {
            pos = model.Pos.FrameIndex;
            value = model.Value.ProjectValue;
        }

        public bool IsValid()
        {
            return pos >= 0
                && Tempo.IsValid(value);
        }

        public TempoTimelineEvent ToModel()
        {
            return new TempoTimelineEvent()
            {
                Pos = new NoteTime(pos),
                Value = Tempo.CreateFromProjectValue(value),
            };
        }
    }
}