using System.Text.Json.Serialization;

namespace VprModLib
{
    public struct VibratoDepth
    {
        /// <summary>
        /// Purpose is unclear.
        /// </summary>
        public int Pos { get; set; }
        /// <summary>
        /// 0 to 127. 0 is no vibrato. 127 is extreme.
        /// </summary>
        public int Value { get; set; }
    }
}

namespace VprModLib.Serialization
{
    public class SerializedVibratoDepth : ISerialized<VibratoDepth>
    {
        public int pos;
        public int value;

        [JsonConstructor]
        public SerializedVibratoDepth(int pos, int value)
        {
            this.pos = pos;
            this.value = value;
        }
        public SerializedVibratoDepth(VibratoDepth model)
        {
            pos = model.Pos;
            value = model.Value;
        }

        public bool IsValid()
        {
            return value >= 0 && value <= 127;
        }

        public VibratoDepth ToModel()
        {
            return new VibratoDepth
            {
                Pos = pos,
                Value = value,
            };
        }
    }
}