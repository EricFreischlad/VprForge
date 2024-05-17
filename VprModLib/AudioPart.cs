using System.Text.Json.Serialization;

namespace VprModLib
{
    /// <summary>
    /// A part in an Audio Track (Type 1). Though the Vocaloid project file shares the term "part" between Vocaloid and audio tracks, they have nothing in common.
    /// </summary>
    public class AudioPart
    {
        public string Name { get; set; } = string.Empty;
        public NoteTime Pos { get; set; }
        public AudioPartWav Wav { get; set; } = new AudioPartWav();
        public AudioPartRegion Region { get; set; } = new AudioPartRegion();
        
        // Alway present due to the two mandatory audio effects PitchChange and ReverseAudio.
        public List<Effect> AudioEffects { get; } = new List<Effect>();
    }
}
namespace VprModLib.Serialization
{
    public class SerializedAudioPart : ISerialized<AudioPart>
    {
        public string name;
        public int pos;
        public SerializedAudioPartWav wav;
        public SerializedAudioPartRegion region;
        public SerializedEffect[] audioEffects;

        [JsonConstructor]
        public SerializedAudioPart(string name, int pos, SerializedAudioPartWav wav, SerializedAudioPartRegion region, SerializedEffect[] audioEffects)
        {
            this.name = name;
            this.pos = pos;
            this.wav = wav;
            this.region = region;
            this.audioEffects = audioEffects;
        }
        public SerializedAudioPart(AudioPart model)
        {
            name = model.Name;
            pos = model.Pos.FrameIndex;
            wav = new SerializedAudioPartWav(model.Wav);
            region = new SerializedAudioPartRegion(model.Region);
            audioEffects = model.AudioEffects.Select(ae => new SerializedEffect(ae)).ToArray();
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(name)
                && pos >= 0
                && wav is { }
                && wav.IsValid()
                && region is { }
                && region.IsValid()
                && audioEffects is { }
                && audioEffects.All(ae => ae.IsValid());
        }

        public AudioPart ToModel()
        {
            var model = new AudioPart()
            {
                Name = name,
                Pos = new NoteTime(pos),
                Wav = wav.ToModel(),
                Region = region.ToModel(),
            };
            model.AudioEffects.AddRange(audioEffects.Select(ae => ae.ToModel()));
            return model;
        }
    }
}