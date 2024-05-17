using System.Text.Json.Serialization;

namespace VprModLib
{
    public class Vibrato
    {
        public int Type { get; set; }
        public NoteTime Duration { get; set; }


        // Without a vibrato expression type applied, these are omitted from the JSON.
        public List<VibratoDepth>? Depths { get; set; }
        public List<VibratoRate>? Rates { get; set; }
    }
}

namespace VprModLib.Serialization
{
    public class SerializedVibrato : ISerialized<Vibrato>
    {
        public int type;
        public int duration;
        public SerializedVibratoDepth[]? depths;
        public SerializedVibratoRate[]? rates;

        [JsonConstructor]
        public SerializedVibrato(int type, int duration, SerializedVibratoDepth[]? depths, SerializedVibratoRate[]? rates)
        {
            this.type = type;
            this.duration = duration;
            this.depths = depths;
            this.rates = rates;
        }
        public SerializedVibrato(Vibrato model)
        {
            type = model.Type;
            duration = model.Duration.FrameIndex;
            depths = model.Depths is { } ?
                model.Depths.Select(d => new SerializedVibratoDepth(d)).ToArray() :
                null;
            rates = model.Rates is { } ?
                model.Rates.Select(r => new SerializedVibratoRate(r)).ToArray() :
                null;
        }
        public bool IsValid()
        {
            // NOTE: Unclear what the range of values is for Type.
            return duration >= 0
                && depths is null == rates is null
                && (depths is null
                    || depths.All(d => d.IsValid()))
                && (rates is null
                    || rates.All(r => r.IsValid()));
        }

        public Vibrato ToModel()
        {
            return new Vibrato()
            {
                Type = type,
                Duration = new NoteTime(duration),
                Depths = depths is { } ? depths.Select(d => d.ToModel()).ToList() : null,
                Rates = rates is { } ? rates.Select(r => r.ToModel()).ToList() : null,
            };
        }
    }
}