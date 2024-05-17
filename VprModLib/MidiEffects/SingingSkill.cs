namespace VprModLib.MidiEffects
{
    public enum SingingSkillParameterName
    {
        /*
         * From Yamaha: "A MIDI effect that automatically applies a suitable pitch bend, dynamics, etc., according to the sequence."
         * From Me: "Basically, automated Attack and Release expressions with less control over how they're performed."
         * 
         * See NoteSingingSkill for details.
         */

        /// <summary>
        /// The default intensity of the pitch expressions created by Singing Skill, when not overridden at the note level. 0 is none, 10 is extreme.
        /// </summary>
        AMOUNT = 0,
        /// <summary>
        /// The GUID of the Singing Skill preset, presumably.
        /// </summary>
        NAME = 1,
        /// <summary>
        /// Unclear effect on expression. 0 to 10.
        /// </summary>
        SKILL = 2,
    }
}