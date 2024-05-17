using VprModLib;

namespace VprForge
{
    public class Transition
    {
        private readonly IReadOnlyList<Note?> _notes;
        private Note? Start => _notes[0];
        private Note? End => _notes[^1];

        // Return the first frame index of the transition. (If Start is NULL, then End is not NULL.)
        public NoteTime Pos => Start is { } ? Start.Pos + Start.Duration : End!.Pos;

        public Transition(IReadOnlyList<Note?> notes)
        {
            _notes = notes;
        }

        public void Move(NoteTime deltaNoteTime)
        {
            // Start note or NULL.
            if (Start is { })
            {
                UpdateNoteDuration(Start, deltaNoteTime);
            }
            // Middle notes, guaranteed NOT NULL.
            for (int i = 1; i < _notes.Count - 1; i++)
            {
                _notes[i]!.Pos += deltaNoteTime;
            }
            // End note or NULL.
            if (End is { })
            {
                End.Pos += deltaNoteTime;
                
                // Do the inverse of what we did to the Start.
                UpdateNoteDuration(End, new NoteTime(-deltaNoteTime.FrameIndex));
            }
        }
        private static void UpdateNoteDuration(Note note, NoteTime deltaNoteTime)
        {
            // IMPORTANT: If a note's vibrato duration is ever longer than the duration of the note itself,
            // then a failsafe kicks in and sets the vibrato duration to 0.
            // We're accounting for that here by always changing the vibrato duration by the same amount as the note duration.

            note.Duration += deltaNoteTime;
            if (note.Vibrato is { })
            {
                note.Vibrato.Duration += deltaNoteTime;
            }
        }
    }
}