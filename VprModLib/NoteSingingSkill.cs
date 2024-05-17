using System.Text.Json.Serialization;

namespace VprModLib
{
    public class NoteSingingSkill
    {
        /*
         * From Yamaha:
         * "If Singing Skill is applied and the Attack Release Effect is not applied to
         * the note, the screen below can be displayed to adjust the 'Accent' by
         * clicking the orange belt section."
         * 
         * "If an effect other than the Attack Release Effect Vibrato is applied to
         * the note, the screen below can be displayed to adjust the "Accent" by
         * clicking the orange belt section."
         * 
         * 
         * __Overview__
         * A note affected by Singing Skill (due to the presence of the "Singing Skill" MIDI effect being enabled on the part)
         * is divided into two halves:
         *  - The first half, which appears to be a type of attack expression unique to the current Singing Skill type.
         *  - The second half, which appears to be a type of release expression unique to the current Singing Skill type.
         * 
         * 
         * __Duration (the dividing line)__
         * The dividing line between the two halves can be moved earlier and later. The value is called "duration" in the project file,
         * and stored as the number of rhythmic frames in the first (attack) half.
         *
         * The default position of the dividing line is 33% of the note duration.
         * The minimum value for the duration is 0 (no attack, all release), and the maximum appears to be 6 frames less than the total note duration.
         *
         * This appears to be a bug, as 6 frames is almost always too short to be noticeable.
         * Opening a project with maximum Singing Skill duration on a note will cause it to be reverted to 6 frames less.
         *
         * This position is updated whenever the note length changes such that the percentage of the total note duration is preserved,
         * rather than the exact number of frames. For example, if you move the dividing line to 75% of the way through the note,
         * then double the total note length, the dividing line will move to the new 75% point, rather than remain at what would now be 37.5%.
         * 
         * 
         * __Weight (intensity of each half)__
         * Both halves have independent intensity controls ("pre" and "post" within "weight" in the project file).
         * Their values range from 0 to 127 with 0 being no expression (a flat line) and 127 being the most extreme expression.
         * 
         * 
         * __Incompatibility__
         * Placing an explicit Attack or Release expression (a.k.a. DVQM Attack or Release) on a note
         * will remove the effect of Singing Skill on that note. A note cannot use an explicit Attack expression
         * and a Singing Skill "release expression" or vice versa.
         * If all explicit expressions are removed from a note (and Singing Skill is still enabled),
         * then Singing Skill will take over again (with default values).
         */

        /// <summary>
        /// The length of the attack part of the Singing Skill expression. 0 ~ (note duration - 6).
        /// </summary>
        public NoteTime Duration { get; set; }
        /// <summary>
        /// The intensity of the Singing Skill expression on the two halves of the note.
        /// </summary>
        public NoteSingingSkillWeight Weight { get; set; }
    }
}

namespace VprModLib.Serialization
{
    public class SerializedNoteSingingSkill : ISerialized<NoteSingingSkill>
    {
        public int duration;
        public SerializedNoteSingingSkillWeight weight;

        [JsonConstructor]
        public SerializedNoteSingingSkill(int duration, SerializedNoteSingingSkillWeight weight)
        {
            this.duration = duration;
            this.weight = weight;
        }
        public SerializedNoteSingingSkill(NoteSingingSkill model)
        {
            duration = model.Duration.FrameIndex;
            weight = new SerializedNoteSingingSkillWeight(model.Weight);
        }

        public bool IsValid()
        {
            return duration >= 0
                && weight.IsValid();
        }

        public NoteSingingSkill ToModel()
        {
            return new NoteSingingSkill()
            {
                Duration = new NoteTime(duration),
                Weight = weight.ToModel(),
            };
        }
    }
}