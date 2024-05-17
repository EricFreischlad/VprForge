using VprModLib;

namespace VprForge
{
    public class TransitionAssembler
    {
        private readonly NoteTime _maxDurationForRunNotes;
        private readonly NoteTime _maxSeparationForPhraseNotes;

        private List<Transition> _transitions = new List<Transition>();
        private List<Note?> _currentTransitionNotes = new List<Note?>();

        public TransitionAssembler(NoteTime maxDurationForRunNotes, NoteTime maxSeparationForPhraseNotes)
        {
            _maxDurationForRunNotes = maxDurationForRunNotes;
            _maxSeparationForPhraseNotes = maxSeparationForPhraseNotes;
        }

        public List<Transition> Assemble(List<Note> orderedNotes)
        {
            _transitions = new List<Transition>();

            int noteCount = orderedNotes.Count;

            StartNewPhrase();

            for (int noteIndex = 0; noteIndex < noteCount; noteIndex++)
            {
                // Need one note of lookahead to determine the ends of phrases.
                Note thisNote = orderedNotes[noteIndex];
                Note? nextNote = noteIndex < noteCount - 1 ? orderedNotes[noteIndex + 1] : null;

                // Add this note to the transition. If it's short, the next will be added on the next loop.
                AddNoteToTransition(thisNote);

                // If this note is short enough to be part of a run,
                if (thisNote.Duration <= _maxDurationForRunNotes)
                {
                    // Continue the run.
                    continue;
                }

                // End run or phrase with this note.

                // Check the next note to see if the phrase is ending.
                if (nextNote is { })
                {
                    // If the gap between this note and the next is large enough,
                    if (nextNote.Pos - (thisNote.Pos + thisNote.Duration) >= _maxSeparationForPhraseNotes)
                    {
                        // End of phrase.
                        EndCurrentPhrase();
                        StartNewPhrase();
                    }
                    else
                    {
                        // Last note of the transition. Same phrase.

                        // Set this note as the end of the transition.
                        _transitions.Add(new Transition(_currentTransitionNotes));

                        // Set this note as the beginning of the next transition.
                        _currentTransitionNotes = new List<Note?>() { thisNote };
                    }
                }
                else
                {
                    // Last note of the part.
                    EndCurrentPhrase();
                    break;
                }
            }
            return _transitions;
        }
        private void StartNewPhrase()
        {
            _currentTransitionNotes = new List<Note?>() { null };
        }
        private void AddNoteToTransition(Note note)
        {
            _currentTransitionNotes.Add(note);
        }
        private void EndCurrentPhrase()
        {
            _currentTransitionNotes.Add(null);
            _transitions.Add(new Transition(_currentTransitionNotes));
        }
    }
}