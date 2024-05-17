using VprModLib;

namespace VprForge
{
    public class ControllerValueSnapshot
    {
        /// <summary>
        /// The controller value for Velocity. This is not a continuous controller - its value is set once at the beginning of the note (called a "note-on" event in MIDI).
        /// </summary>
        public int NoteOnVelocity { get; set; }
        /// <summary>
        /// The controller value for Opening ("Mouth" in the editor). This is not a continuous controller - its value is set once at the beginning of the note (called a "note-on" event in MIDI).
        /// </summary>
        public int NoteOnOpening { get; set; }
        /// <summary>
        /// The controller values which can change over time. These are caled "continuous controllers" (CCs) in MIDI since they can be modified while a note is playing. In this implementation, only the value at the start of the note is preserved.
        /// </summary>
        public Dictionary<ControllerType, int> CcValuesByController { get; } = new Dictionary<ControllerType, int>();
    }
}