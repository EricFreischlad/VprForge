using System.Text.Json.Serialization;

namespace VprModLib
{
    public class Loop
    {
        public bool IsEnabled { get; set; }
        public NoteTime Begin { get; set; }
        public NoteTime End { get; set; }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedLoop : ISerialized<Loop>
    {
        public bool isEnabled;
        public int begin;
        public int end;

        [JsonConstructor]
        public SerializedLoop(bool isEnabled, int begin, int end)
        {
            this.isEnabled = isEnabled;
            this.begin = begin;
            this.end = end;
        }
        public SerializedLoop(Loop model)
        {
            isEnabled = model.IsEnabled;
            begin = model.Begin.FrameIndex;
            end = model.End.FrameIndex;
        }

        public bool IsValid()
        {
            return begin >= 0
                && end >= 0
                && begin < end;
        }

        public Loop ToModel()
        {
            return new Loop
            {
                IsEnabled = isEnabled,
                Begin = new NoteTime(begin),
                End = new NoteTime(end),
            };
        }
    }
}