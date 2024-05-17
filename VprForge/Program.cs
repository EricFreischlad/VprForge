using VprForge;
using VprModLib;
using VprModLib.Serialization;

void StereoDoubleProject(string sourceFilePath, string destinationFilePath)
{
    if (!FileIO.TryRead(sourceFilePath, out var session, out var message))
    {
        Console.WriteLine("Error on read: " + message);
    }

    var leftRandomizer = CreateStereoDoublePartRandomizer(69);
    var rightRandomizer = CreateStereoDoublePartRandomizer(1337);

    var stereoDoubler = new StereoDoubler(
        leftRandomizer,
        rightRandomizer,
        // Track will be doubled if the name ends with "#double".
        willBeDoubled: t => t.Name.Trim().EndsWith("#double", StringComparison.CurrentCultureIgnoreCase),
        // Ex.) "VOCALOID 1 #double" becomes "VOCALOID 1 (left double)" and "VOCALOID 1 (right double)"
        getDoubledTrackName: (name, isLeftChannel) => $"{name[0..name.LastIndexOf("#double", StringComparison.CurrentCultureIgnoreCase)]} ({(isLeftChannel ? "left" : "right")} double)",
        getDoubledTrackBusNo: (busNo, isLeftChannel) =>
        {
            return busNo switch
            {
                // Doubled vocal line splits to 3 and 4.
                3 => isLeftChannel ? 3 : 4,
                // Harmony lines stay on one bus.
                _ => busNo,
            };
        },
        // Doubled tracks will be all the way left and right.
        normalizedStereoSpread: 1.0,
        // Since we're importing into an existing Vocaloid project in FL Studio, we only need to add the new tracks.
        onlyOutputNewTracks: true);

    stereoDoubler.ProcessSequence(session!.Project!.Sequence!);

    if (!FileIO.TryWrite(destinationFilePath, session!, out message))
    {
        Console.WriteLine("Error on write: " + message);
    }
}
PartRandomizer CreateStereoDoublePartRandomizer(int seed)
{
    // Create a Vocaloid part randomizer.
    return new PartRandomizer(
        noteTimeSpecs:
        // 1/16th notes and shorter are runs and won't have their durations changed.
        (maxDurationForRunNotes: new NoteTime(0, BeatSubdivision.SixteenthNotes, 1),
        // Any gaps between notes of an 1/8th note duration or shorter are preserved. Their spacing won't change.
        maxSeparationForPhraseNotes: new NoteTime(0, BeatSubdivision.EighthNotes, 1),
        noteTimeValueAdjustment: new NoteTimeValueAdjustment(
            // Notes can come in up to one 1/64th note early.
            new NoteTime(0, BeatSubdivision.SixtyFourthNotes, 1),
            // Notes can come in up to one 1/64th note late.
            new NoteTime(0, BeatSubdivision.SixtyFourthNotes, 1),
            // We do need to notes to be able to come in early.
            dontAllowNegativeValues: false)),
        // Note velocity value (consonant length) can be anywhere between 16 less or more than where it started (12.5% in either direction).
        velocitySpec: (16, 16),
        // Note opening value ("Mouth" in editor) is rather important for vowel tone, so let's not mess with it.
        openingSpec: null,
        controllerSpecs: new Dictionary<ControllerType, (int maxReduction, int maxIncrease)>()
        {
            // Portamento Timing value for each note can be anywhere between 6 less or more than where it started, which is a pretty small change overall.
            { ControllerType.Get(ControllerName.PORTAMENTO), (6, 6)},
            // Brightness value for each note can be anywhere between 8 less or more than where it started.
            { ControllerType.Get(ControllerName.BRIGHTNESS), (8, 8)},
        },
        // Optional RNG seed for predictable randomness.
        rngSeed: seed);
}

string sourceFilePath = "C:\\Users\\Kodakami\\Desktop\\nuclear 2024-02-01T0250.vpr";
string destinationFilePath = "C:\\Users\\Kodakami\\Desktop\\nuclear 2024-02-01T0250 DOUBLED LINES.vpr";
StereoDoubleProject(sourceFilePath, destinationFilePath);
