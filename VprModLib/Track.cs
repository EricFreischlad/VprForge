using System.Text.Json;
using System.Text.Json.Serialization;

namespace VprModLib
{
    public abstract class Track
    {
        public TrackType Type { get; private set; }
        public string Name { get; set; } = string.Empty;
        // TODO: Enum these.
        public int Color { get; set; }
        public int BusNo { get; set; }
        /// <summary>
        /// Is the track hidden in the editor?
        /// </summary>
        public bool IsFolded { get; set; }
        public double Height { get; set; }
        public VolumeTimeline Volume { get; set; } = new VolumeTimeline();
        public PanpotTimeline Panpot { get; set; } = new PanpotTimeline();
        public bool IsMuted { get; set; }
        public bool IsSoloMode { get; set; }

        protected Track(TrackType trackType)
        {
            Type = trackType;
        }
    }
    public class VocaloidTrack : Track
    {
        /// <summary>
        /// Appears to be a MIDI note number.
        /// </summary>
        public int LastScrollPositionNoteNumber { get; set; }
        
        // Only included if the Track has at least one Part.
        public List<VocaloidPart>? Parts { get; set; } = new List<VocaloidPart>();

        public VocaloidTrack() : base(TrackType.VOCALOID) { }
    }
    public class AudioTrack : Track
    {
        public List<AudioPart>? Parts { get; set; } = new List<AudioPart>();

        public AudioTrack() : base(TrackType.AUDIO) { }
    }
}
namespace VprModLib.Serialization
{
    public abstract class SerializedTrack
    {
        public int type;
        public string name;
        public int color;
        public int busNo;
        public bool isFolded;
        public double height;
        public SerializedVolumeTimeline volume;
        public SerializedPanpotTimeline panpot;
        public bool isMuted;
        public bool isSoloMode;

        protected SerializedTrack(int type, string name, int color, int busNo, bool isFolded, double height, SerializedVolumeTimeline volume, SerializedPanpotTimeline panpot, bool isMuted, bool isSoloMode)
        {
            this.type = type;
            this.name = name;
            this.color = color;
            this.busNo = busNo;
            this.isFolded = isFolded;
            this.height = height;
            this.volume = volume;
            this.panpot = panpot;
            this.isMuted = isMuted;
            this.isSoloMode = isSoloMode;
        }
        protected SerializedTrack(Track weakModel)
        {
            type = (int)weakModel.Type;
            name = weakModel.Name;
            color = weakModel.Color;
            busNo = weakModel.BusNo;
            isFolded = weakModel.IsFolded;
            height = weakModel.Height;
            volume = new SerializedVolumeTimeline(weakModel.Volume);
            panpot = new SerializedPanpotTimeline(weakModel.Panpot);
            isMuted = weakModel.IsMuted;
            isSoloMode = weakModel.IsSoloMode;
        }


        // NOTE: Not implementing ISerialized interface, but this is used by classes that do.
        public bool IsValid()
        {
            // NOTE: Unable to validate color until new infrastructure is developed.
            // NOTE: Unable to validate busNo until new infrastructure is developed.
            return (type == 0 || type == 1)
                && !string.IsNullOrEmpty(name)
                && color >= 0
                && busNo >= 0
                && height >= 0.0
                && volume is { }
                && volume.IsValid()
                && panpot is { }
                && panpot.IsValid();
        }

        // NOTE: Not implementing ISerialized interface, but this is used by classes that do.
        public void PopulateModel(Track concreteInstance)
        {
            concreteInstance.Name = name;
            concreteInstance.Color = color;
            concreteInstance.BusNo = busNo;
            concreteInstance.IsFolded = isFolded;
            concreteInstance.Height = height;
            concreteInstance.Volume = volume.ToModel();
            concreteInstance.Panpot = panpot.ToModel();
            concreteInstance.IsMuted = isMuted;
            concreteInstance.IsSoloMode = isSoloMode;
        }

        // NOTE: Not implementing ISerialized interface, but this is used by classes that do.
        public abstract Track ToConcreteModel();

        public static SerializedTrack CreateFromModel(Track weakModel)
        {
            return weakModel.Type switch
            {
                TrackType.VOCALOID => new SerializedVocaloidTrack((VocaloidTrack)weakModel),
                TrackType.AUDIO => new SerializedAudioTrack((AudioTrack)weakModel),
                _ => throw new ArgumentException("Track type was invalid while serializing.")
            };
        }
    }
    public class SerializedVocaloidTrack : SerializedTrack, ISerialized<VocaloidTrack>
    {
        public int lastScrollPositionNoteNumber;
        public SerializedVocaloidPart[]? parts;

        [JsonConstructor]
        public SerializedVocaloidTrack(int type, string name, int color, int busNo, bool isFolded, double height, SerializedVolumeTimeline volume, SerializedPanpotTimeline panpot, bool isMuted, bool isSoloMode, int lastScrollPositionNoteNumber, SerializedVocaloidPart[]? parts) : base(type, name, color, busNo, isFolded, height, volume, panpot, isMuted, isSoloMode)
        {
            this.lastScrollPositionNoteNumber = lastScrollPositionNoteNumber;
            this.parts = parts;
        }
        public SerializedVocaloidTrack(VocaloidTrack strongModel)
            :base(strongModel)
        {
            lastScrollPositionNoteNumber = strongModel.LastScrollPositionNoteNumber;
            parts = strongModel.Parts is { } ?
                strongModel.Parts.Select(p => new SerializedVocaloidPart(p)).ToArray() :
                null;
        }

        new public bool IsValid()
        {
            // NOTE: Could use a MIDI Note Number class.
            return base.IsValid()
                && lastScrollPositionNoteNumber >= 0
                && lastScrollPositionNoteNumber <= 127
                && (parts is null
                    || parts.All(p => p.IsValid()));
        }
        public VocaloidTrack ToModel()
        {
            var model = new VocaloidTrack()
            {
                LastScrollPositionNoteNumber = lastScrollPositionNoteNumber,
                Parts = parts is { } ? parts.Select(p => p.ToModel()).ToList() : null,
            };
            PopulateModel(model);
            return model;
        }
        public override Track ToConcreteModel() => ToModel();
    }
    public class SerializedAudioTrack : SerializedTrack, ISerialized<AudioTrack>
    {
        public SerializedAudioPart[]? parts;

        [JsonConstructor]
        public SerializedAudioTrack(int type, string name, int color, int busNo, bool isFolded, double height, SerializedVolumeTimeline volume, SerializedPanpotTimeline panpot, bool isMuted, bool isSoloMode, SerializedAudioPart[]? parts) : base(type, name, color, busNo, isFolded, height, volume, panpot, isMuted, isSoloMode)
        {
            this.parts = parts;
        }
        public SerializedAudioTrack(AudioTrack strongModel)
            : base(strongModel)
        {
            parts = strongModel.Parts is { } ?
                strongModel.Parts.Select(p => new SerializedAudioPart(p)).ToArray() :
                null;
        }

        new public bool IsValid()
        {
            return base.IsValid()
                && (parts is null || parts.All(p => p.IsValid()));
        }

        public AudioTrack ToModel()
        {
            var model = new AudioTrack()
            {
                Parts = parts is { } ? parts.Select(p => p.ToModel()).ToList() : null,
            };
            PopulateModel(model);
            return model;
        }
        public override Track ToConcreteModel() => ToModel();
    }

    public class SerializedTrackConverter : JsonConverter<SerializedTrack>
    {
        public override SerializedTrack? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);

            int type = doc.RootElement.GetProperty("type").GetInt32();

            return type switch
            {
                (int)TrackType.VOCALOID => JsonSerializer.Deserialize<SerializedVocaloidTrack>(doc.RootElement.GetRawText(), options),
                (int)TrackType.AUDIO => JsonSerializer.Deserialize<SerializedAudioTrack>(doc.RootElement.GetRawText(), options),
                _ => throw new JsonException($"Unknown Track type value: {type}."),
            };
        }

        public override void Write(Utf8JsonWriter writer, SerializedTrack value, JsonSerializerOptions options)
        {
            switch (value.type)
            {
                case (int)TrackType.VOCALOID:
                    JsonSerializer.Serialize(writer, (SerializedVocaloidTrack)value, options);
                    break;
                case (int)TrackType.AUDIO:
                    JsonSerializer.Serialize(writer, (SerializedAudioTrack)value, options);
                    break;
                default:
                    throw new JsonException($"Unknown Track type value: {value.type}.");
            };
        }
    }
}