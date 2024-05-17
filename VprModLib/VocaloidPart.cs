using System.Text.Json.Serialization;
using System.Xml.Linq;
using VprModLib.AudioEffects;

namespace VprModLib
{
    /// <summary>
    /// A part in a Vocaloid Track (Type 0). Though the Vocaloid project file shares the term "part" between Vocaloid and audio tracks, they have nothing in common.
    /// </summary>
    public class VocaloidPart
    {
        public NoteTime Pos { get ; set; }
        public NoteTime Duration { get; set; }
        public string StyleName { get; set; } = string.Empty;
        public PartVoice Voice { get; set; } = new PartVoice();
        
        // Only included if there are any audio effects on the part.
        public List<Effect>? AudioEffects { get; set; }
        
        // Always included since midi effects can't be removed.
        public List<Effect> MidiEffects { get; } = new List<Effect>();

        // Only included if there are any notes in the part.
        public List<Note>? Notes { get; set; }
        
        // Only included if any controllers were modified for the part.
        public List<Controller>? Controllers { get; set; }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedVocaloidPart : ISerialized<VocaloidPart>
    {
        public int pos;
        public int duration;
        public string styleName;
        public SerializedPartVoice voice;
        public SerializedEffect[]? audioEffects;
        public SerializedEffect[] midiEffects;
        public SerializedNote[]? notes;
        public SerializedController[]? controllers;

        [JsonConstructor]
        public SerializedVocaloidPart(int pos, int duration, string styleName, SerializedPartVoice voice, SerializedEffect[]? audioEffects, SerializedEffect[] midiEffects, SerializedNote[]? notes, SerializedController[]? controllers)
        {
            this.pos = pos;
            this.duration = duration;
            this.styleName = styleName;
            this.voice = voice;
            this.audioEffects = audioEffects;
            this.midiEffects = midiEffects;
            this.notes = notes;
            this.controllers = controllers;
        }
        public SerializedVocaloidPart(VocaloidPart model)
        {
            pos = model.Pos.FrameIndex;
            duration = model.Duration.FrameIndex;
            styleName = model.StyleName;
            voice = new SerializedPartVoice(model.Voice);
            audioEffects = model.AudioEffects is { } ?
                model.AudioEffects.Select(ae => new SerializedEffect(ae)).ToArray() :
                null;
            midiEffects = model.MidiEffects.Select(me => new SerializedEffect(me)).ToArray();
            notes = model.Notes is { } ?
                model.Notes.Select(n => new SerializedNote(n)).ToArray() :
                null;
            controllers = model.Controllers is { } ?
                model.Controllers.Select(c => new SerializedController(c)).ToArray() :
                null;
        }

        public bool IsValid()
        {
            return pos >= 0
                && duration >= 0
                && !string.IsNullOrEmpty(styleName)
                && voice is { }
                && voice.IsValid()
                && (audioEffects is null
                    || audioEffects.All(ae => ae.IsValid()))
                && midiEffects is { }
                && midiEffects.All(me => me.IsValid())
                && (notes is null
                    || notes.All(n => n.IsValid()))
                && (controllers is null
                    || controllers.All(c => c.IsValid()));
        }

        public VocaloidPart ToModel()
        {
            var model = new VocaloidPart()
            {
                Pos = new NoteTime(pos),
                Duration = new NoteTime(duration),
                StyleName = styleName,
                Voice = voice.ToModel(),
                AudioEffects = audioEffects is { } ? audioEffects
                    .Select(ae => ae.ToModel())
                    .ToList() : null,
                Notes = notes is { } ? notes
                    .Select(n => n.ToModel())
                    .ToList() : null,
                Controllers = controllers is { } ? controllers
                    .Select(c => c.ToModel())
                    .ToList() : null,
            };
            model.MidiEffects.AddRange(midiEffects.Select(me => me.ToModel()));
            return model;
        }
    }
}