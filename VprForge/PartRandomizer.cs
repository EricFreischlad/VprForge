using VprModLib;

namespace VprForge
{
    public class PartRandomizer
    {
        private readonly NoteTimeValueAdjustment? _noteTimeValueAdjustment = null;
        private readonly ValueAdjustment? _velocityValueAdjustment = null;
        private readonly ValueAdjustment? _openingValueAdjustment = null;
        private readonly Dictionary<ControllerType, ValueAdjustment>? _controllerValueAdjustments = null;

        private readonly Random _rng;
        private readonly TransitionAssembler? _transitionAssembler;

        public PartRandomizer(
            (NoteTime maxDurationForRunNotes,
            NoteTime maxSeparationForPhraseNotes,
            NoteTimeValueAdjustment noteTimeValueAdjustment)? noteTimeSpecs = null,
            (int maxReduction, int maxIncrease)? velocitySpec = null,
            (int maxReduction, int maxIncrease)? openingSpec = null,
            IReadOnlyDictionary<ControllerType, (int maxReduction, int maxIncrease)>? controllerSpecs = null,
            int? rngSeed = null)
        {
            _rng = rngSeed.HasValue ? new Random(rngSeed.Value) : new Random();

            if (noteTimeSpecs is { })
            {
                _noteTimeValueAdjustment = noteTimeSpecs.Value.noteTimeValueAdjustment;
                _transitionAssembler = new TransitionAssembler(noteTimeSpecs.Value.maxDurationForRunNotes, noteTimeSpecs.Value.maxSeparationForPhraseNotes);
            }

            if (velocitySpec is { })
            {
                _velocityValueAdjustment = new ValueAdjustment(velocitySpec.Value.maxReduction, velocitySpec.Value.maxIncrease, 0, 127);
            }

            if (openingSpec is { })
            {
                _openingValueAdjustment = new ValueAdjustment(openingSpec.Value.maxReduction, openingSpec.Value.maxIncrease, 0, 127);
            }

            if (controllerSpecs is { })
            {
                _controllerValueAdjustments = new Dictionary<ControllerType, ValueAdjustment>();
                foreach (var (type, limits) in controllerSpecs)
                {
                    _controllerValueAdjustments[type] = new ValueAdjustment(limits.maxReduction, limits.maxIncrease, type.MinValue, type.MaxValue);
                }
            }

            // TODO:
            //      SingingSkill randomization (separator placement, attack intensity, release intensity), possibly by singing skill type.
            //      DVQM randomization (TopFactor value, length), by DVQM type.
            //      Vibrato randomization (amplitude and frequency) possibly by vibrato type
        }
        public VocaloidPart? RandomizeAsNewPart(VocaloidPart part)
        {
            VocaloidPart? newPart = null;
            if (part is null || part.Notes is null || part.Notes.Count == 0)
            {
                return newPart;
            }

            // Convert back and forth from serialized. Bit of a hack for cloning a part.
            newPart = new VprModLib.Serialization.SerializedVocaloidPart(part!).ToModel();

            // Sort the notes by note time. If there are duplicates for any frame indexes, select by the highest note number.
            // Notes that come before the first frame of the part are strictly ignored.
            var orderedNotes = newPart!.Notes!
                .OrderBy(n => n.Pos.FrameIndex)
                .Where(n => n.Pos.FrameIndex >= 0)
                .GroupBy(n => n.Pos.FrameIndex)
                .Select(group => group
                    .OrderByDescending(n => n.Number)
                    .First())
                .ToList();

            bool hasControllersToRandomize = newPart!.Controllers is { } && _controllerValueAdjustments is { } && _controllerValueAdjustments.Count != 0;
            List<Controller>? controllersToRandomize =
                hasControllersToRandomize ?
                newPart!.Controllers!.FindAll(c => _controllerValueAdjustments!.ContainsKey(c.Type)) :
                null;

            Dictionary<Note, ControllerValueSnapshot>? controllerValueSnapshotByNote = null;
            if (hasControllersToRandomize)
            {
                var controllerValueAssembler = new ControllerValueAssembler(orderedNotes, controllersToRandomize!);
                controllerValueSnapshotByNote = controllerValueAssembler.Assemble();
            }

            if (_noteTimeValueAdjustment is { })
            {
                foreach (var transition in _transitionAssembler!.Assemble(orderedNotes))
                {
                    // Get new absolute position.
                    var newPos = _noteTimeValueAdjustment!.GetNewRandomNoteTime(transition.Pos, _rng);

                    // Ensure the new position isn't before the start of the part (note will end up hidden).
                    if (newPos.FrameIndex < 0)
                    {
                        newPos = new NoteTime(0);
                    }

                    // Convert absolute position to delta.
                    transition.Move(newPos - transition.Pos);
                }
            }

            if (hasControllersToRandomize)
            {
                // Randomize Note-on controllers.
                foreach (var note in orderedNotes)
                {
                    if (_velocityValueAdjustment is { })
                    {
                        note.Velocity = _velocityValueAdjustment.GetRandomNewValue(note.Velocity, _rng);
                    }
                    if (_openingValueAdjustment is { })
                    {
                        note.Opening = _openingValueAdjustment.GetRandomNewValue(note.Opening, _rng);
                    }
                }

                // Randomize continuous controllers.
                foreach (var controller in controllersToRandomize!)
                {
                    var valueAdjustment = _controllerValueAdjustments![controller.Type];

                    // Clear all events for controller type.
                    controller.Events.Clear();

                    foreach (var (note, controllerValue) in controllerValueSnapshotByNote!)
                    {
                        int newValue = valueAdjustment.GetRandomNewValue(controllerValueSnapshotByNote[note].CcValuesByController[controller.Type], _rng);
                        controller.Events.Add(new ControllerEvent()
                        {
                            Pos = note.Pos,
                            Value = newValue,
                        });
                    }
                }
            }

            return newPart;
        }
    }
}