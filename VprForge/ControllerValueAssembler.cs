using VprModLib;

namespace VprForge
{
    public class ControllerValueAssembler
    {
        private readonly List<Note> _orderedNotes;
        private readonly List<Controller> _controllers;

        public ControllerValueAssembler(List<Note> orderedNotes, List<Controller> controllers)
        {
            _orderedNotes = orderedNotes;
            _controllers = controllers;
        }

        public Dictionary<Note, ControllerValueSnapshot> Assemble()
        {
            // Set up the output object.
            var controllerValuesByNote = new Dictionary<Note, ControllerValueSnapshot>();
            foreach (var note in _orderedNotes)
            {
                controllerValuesByNote.Add(note, new ControllerValueSnapshot());
            }
            
            // Time for some manual enumeration in C#. Get your hard hats on.
            var controllersWithEvents = _controllers
                .FindAll(c => c.Events is { });

            var eventEnumerators = new Dictionary<ControllerType, List<ControllerEvent>.Enumerator>(
                controllersWithEvents
                    .ConvertAll(c => KeyValuePair.Create(c.Type, c.Events!.GetEnumerator())));

            var currentEvents = new Dictionary<ControllerType, ControllerEvent>(
                controllersWithEvents
                    .ConvertAll(c => KeyValuePair.Create(c.Type, new ControllerEvent() { Pos = new NoteTime(0), Value = c.Type.DefaultValue })));

            for (int noteIndex = 0; noteIndex < _orderedNotes.Count; noteIndex++)
            {
                Note? note = _orderedNotes[noteIndex];

                // Get new target note time.
                var now = note.Pos;

                // Note-on controller values.
                controllerValuesByNote[note].NoteOnVelocity = note.Velocity;
                controllerValuesByNote[note].NoteOnOpening = note.Opening;

                // Update all continuous current controller values to their values at the target note time.
                foreach (var (type, iter) in eventEnumerators)
                {
                    // Skip if the next event is after now.
                    if (currentEvents[type].Pos > now)
                    {
                        continue;
                    }
                    
                    // Keep moving to the next event for this controller (stop if there are no more).
                    while (iter.MoveNext())
                    {
                        // If the current event is after now,
                        if (iter.Current.Pos > now)
                        {
                            // Stop.
                            break;
                        }

                        // Update the current event for this controller.
                        currentEvents[type] = iter.Current;
                    }
                }

                // Copy all continuous controller values for the current note.
                foreach (var controller in controllersWithEvents)
                {
                    controllerValuesByNote[note].CcValuesByController[controller.Type] = currentEvents[controller.Type]!.Value;
                }
            }

            return controllerValuesByNote;
        }
    }
}