using System.Text.Json.Serialization;

namespace VprModLib
{
    public struct VibratoRate
    {
        /// <summary>
        /// Purpose is unclear.
        /// </summary>
        public int Pos { get; set; }
        /// <summary>
        /// 0 to 127. 0 is very slow. 127 is very fast.
        /// </summary>
        public int Value { get; set; }
    }
}

namespace VprModLib.Serialization
{
    public class SerializedVibratoRate : ISerialized<VibratoRate>
    {
        public int pos;
        public int value;

        [JsonConstructor]
        public SerializedVibratoRate(int pos, int value)
        {
            this.pos = pos;
            this.value = value;
        }
        public SerializedVibratoRate(VibratoRate model)
        {
            pos = model.Pos;
            value = model.Value;
        }
        public bool IsValid()
        {
            return value >= 0 && value <= 127;
        }

        public VibratoRate ToModel()
        {
            return new VibratoRate
            {
                Pos = pos,
                Value = value,
            };
        }
    }
}