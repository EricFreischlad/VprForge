using System.Text.Json.Serialization;

namespace VprModLib
{
    public class VolumeEvent
    {
        public NoteTime Pos { get; set; }
        public Volume Value { get; set; }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedVolumeEvent : ISerialized<VolumeEvent>
    {
        public int pos;
        public int value;

        [JsonConstructor]
        public SerializedVolumeEvent(int pos, int value)
        {
            this.pos = pos;
            this.value = value;
        }
        public SerializedVolumeEvent(VolumeEvent model)
        {
            pos = model.Pos.FrameIndex;
            value = model.Value.ProjectValue;
        }

        public bool IsValid()
        {
            return pos >= 0
                && Volume.IsValid(value);
        }

        public VolumeEvent ToModel()
        {
            return new VolumeEvent()
            {
                Pos = new NoteTime(pos),
                Value = Volume.CreateFromProjectValue(value),
            };
        }
    }
}