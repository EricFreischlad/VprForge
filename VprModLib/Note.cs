using System.Text.Json.Serialization;

namespace VprModLib
{
    public class Note
    {
        /// <summary>
        /// The written lyric for the note.
        /// </summary>
        public string Lyric { get; set; } = string.Empty;
        /// <summary>
        /// The combination of phonemes for the note.
        /// </summary>
        public string Phoneme { get; set; } = string.Empty;
        /// <summary>
        /// Is the not protected from automatic changes? If not, it can only be manually overwritten. This appears to be for the lyric and phonemes.
        /// </summary>
        public bool IsProtected { get; set; }
        /// <summary>
        /// The position of the note relative to the start of the part. Notes can have a negative position if they come before the official beginning of the part. This can happen when changing a part's start time causes notes to be lost. The notes stay in the part, but are effectively disabled until the part's start time is adjusted back.
        /// </summary>
        public NoteTime Pos { get; set; }
        /// <summary>
        /// The duration of the note.
        /// </summary>
        public NoteTime Duration { get; set; }
        /// <summary>
        /// The center pitch of the note. This uses unmodified MIDI note numbers.
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// The length of the consonant, 0 ~ 127. 0 = longest, 127 = shortest.
        /// </summary>
        public int Velocity { get; set; }
        /// <summary>
        /// "Mouth" in the editor. Contained within the "exp" property, but the object only has one property.
        /// </summary>
        public int Opening { get; set; }

        // Only included if there are explicit expressions attached to the note (not including the effects of Singing Skill).
        public Dvqm? Dvqm { get; set; }
        
        // Only included if there are changes to the singing skill at the note level.
        public NoteSingingSkill? SingingSkill { get; set; }

        // Always included, even if no vibrato expression is used. See the class itself for which values can be omitted.
        public Vibrato Vibrato { get; set; } = new Vibrato();
    }
}
namespace VprModLib.Serialization
{
    public class SerializedNote : ISerialized<Note>
    {
        public string lyric;
        public string phoneme;
        public bool isProtected;
        public int pos;
        public int duration;
        public int number;
        public int velocity;
        public SerializedNoteExpression exp;
        public SerializedDvqm? dvqm;
        public SerializedNoteSingingSkill? singingSkill;
        public SerializedVibrato vibrato;

        [JsonConstructor]
        public SerializedNote(string lyric, string phoneme, bool isProtected, int pos, int duration, int number, int velocity, SerializedNoteExpression exp, SerializedDvqm? dvqm, SerializedNoteSingingSkill? singingSkill, SerializedVibrato vibrato)
        {
            this.lyric = lyric;
            this.phoneme = phoneme;
            this.isProtected = isProtected;
            this.pos = pos;
            this.duration = duration;
            this.number = number;
            this.velocity = velocity;
            this.exp = exp;
            this.dvqm = dvqm;
            this.singingSkill = singingSkill;
            this.vibrato = vibrato;
        }
        public SerializedNote(Note model)
        {
            lyric = model.Lyric;
            phoneme = model.Phoneme;
            isProtected = model.IsProtected;
            pos = model.Pos.FrameIndex;
            duration = model.Duration.FrameIndex;
            number = model.Number;
            velocity = model.Velocity;
            exp = new SerializedNoteExpression()
            {
                opening = model.Opening,
            };
            dvqm = model.Dvqm is { } ?
                new SerializedDvqm(model.Dvqm) :
                null;
            singingSkill = model.SingingSkill is { } ?
                new SerializedNoteSingingSkill(model.SingingSkill) :
                null;
            vibrato = new SerializedVibrato(model.Vibrato);
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(lyric)
                && !string.IsNullOrEmpty(phoneme)
                && duration >= 0
                && number >= 0
                && number <= 127
                && velocity >= 0
                && velocity <= 127
                && exp is { } && exp.opening >= 0 && exp.opening <= 127
                && (dvqm is null
                    || dvqm.IsValid())
                && (singingSkill is null
                    || singingSkill.IsValid())
                && vibrato is { } && vibrato.IsValid()

                // DVQM and Singing Skill are mutually exclusive.
                // At least one (if not both) should be NULL.
                && (dvqm is null || singingSkill is null);
        }

        public Note ToModel()
        {
            return new Note()
            {
                Lyric = lyric,
                Phoneme = phoneme,
                IsProtected = isProtected,
                Pos = new NoteTime(pos),
                Duration = new NoteTime(duration),
                Number = number,
                Velocity = velocity,
                Opening = exp.opening,
                Dvqm = dvqm is { } ? dvqm.ToModel() : null,
                SingingSkill = singingSkill is { } ? singingSkill.ToModel() : null,
                Vibrato = vibrato.ToModel(),
            };
        }
    }
    public class SerializedNoteExpression
    {
        public int opening;
    }
}