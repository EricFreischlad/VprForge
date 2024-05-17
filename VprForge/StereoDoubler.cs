using VprModLib;
using VprModLib.Serialization;

namespace VprForge
{
    public delegate string GetDoubledTrackNameDelegate(string originalTrackName, bool isLeftChannel);
    public delegate int GetDoubledTrackBusNoDelegate(int originalBusNumber, bool isLeftChannel);
    public class StereoDoubler
    {
        private readonly PartRandomizer _leftrandomizer;
        private readonly PartRandomizer _rightrandomizer;
        private readonly Predicate<VocaloidTrack> _willBeDoubled;
        private readonly GetDoubledTrackNameDelegate _getDoubledTrackName;
        private readonly GetDoubledTrackBusNoDelegate? _getDoubledTrackBusNo;
        private readonly double _normalizedStereoSpread;
        private readonly bool _onlyOutputNewTracks;

        public StereoDoubler(PartRandomizer leftRandomizer, PartRandomizer rightRandomizer, Predicate<VocaloidTrack> willBeDoubled, GetDoubledTrackNameDelegate getDoubledTrackName, GetDoubledTrackBusNoDelegate? getDoubledTrackBusNo = null, double normalizedStereoSpread = 1.0, bool onlyOutputNewTracks = false)
        {
            _leftrandomizer = leftRandomizer;
            _rightrandomizer = rightRandomizer;
            _willBeDoubled = willBeDoubled;
            _getDoubledTrackName = getDoubledTrackName;
            _normalizedStereoSpread = normalizedStereoSpread;
            _onlyOutputNewTracks = onlyOutputNewTracks;
            _getDoubledTrackBusNo = getDoubledTrackBusNo;
        }
        public void ProcessSequence(Sequence sequence)
        {
            if (sequence is null || sequence.Tracks is null || sequence.Tracks.Count == 0)
            {
                // Only processes sequences that have at least one track.
                return;
            }

            var tracksToAdd = new List<Track>();
            foreach (var weakTrack in sequence.Tracks)
            {
                if (weakTrack is not VocaloidTrack origTrack)
                {
                    // Only works with Vocaloid tracks.
                    continue;
                }
                if (origTrack.Parts is null || origTrack.Parts.Count == 0)
                {
                    // Only doubles tracks with at least 1 part.
                    continue;
                }
                if (!_willBeDoubled(origTrack))
                {
                    // Only doubles tracks that meet the requirements (such as a keyword in the track name).
                    continue;
                }

                // Make both new tracks (right and left).
                var leftTrack = CreateTrackForStereoChannel(origTrack, true);
                var rightTrack = CreateTrackForStereoChannel(origTrack, false);

                // Randomize parts for left channel.
                leftTrack.Parts = RandomizeToNewParts(origTrack.Parts!, _leftrandomizer);

                // Randomize parts for right channel.
                rightTrack.Parts = RandomizeToNewParts(origTrack.Parts!, _rightrandomizer);

                // Mute the original track for convenience (the two stereo tracks are designed to replace it).
                origTrack.IsMuted = true;
                origTrack.IsSoloMode = false;

                // Hold onto the tracks if they ended up having any parts.
                if (leftTrack.Parts.Count != 0)
                {
                    tracksToAdd.Add(leftTrack);
                    tracksToAdd.Add(rightTrack);
                }
            }

            if (_onlyOutputNewTracks)
            {
                sequence.Tracks.Clear();
            }
            
            sequence.Tracks.AddRange(tracksToAdd);
        }
        private VocaloidTrack CreateTrackForStereoChannel(VocaloidTrack origTrack, bool isLeft)
        {
            var newTrack = new VocaloidTrack()
            {
                Color = origTrack.Color,
                Height = origTrack.Height,
                IsFolded = origTrack.IsFolded,
                IsMuted = origTrack.IsMuted,
                IsSoloMode = origTrack.IsSoloMode,
                LastScrollPositionNoteNumber = origTrack.LastScrollPositionNoteNumber,
                Panpot = new SerializedPanpotTimeline(origTrack.Panpot).ToModel(),
                Volume = new SerializedVolumeTimeline(origTrack.Volume).ToModel(),

                BusNo = _getDoubledTrackBusNo is { } ? _getDoubledTrackBusNo(origTrack.BusNo, isLeft) : origTrack.BusNo,
                Name = _getDoubledTrackName(origTrack.Name, isLeft),
            };

            // Panpot's "normalized" constructor takes a value between -1.0 (left) and 1.0 (right).
            double firstPanpotEventNormalizedValue = isLeft ? -_normalizedStereoSpread : _normalizedStereoSpread;

            // Set the first panpot event's value to full left or right. One panpot value is guaranteed to exist at position 0.
            newTrack.Panpot.Events[0].Value = Panpot.CreateFromNormalizedValue((float)firstPanpotEventNormalizedValue);

            return newTrack;
        }
        private static List<VocaloidPart> RandomizeToNewParts(IEnumerable<VocaloidPart> origParts, PartRandomizer leftOrRightRandomizer)
        {
            var newParts = new List<VocaloidPart>();
            
            // For each part...
            foreach (var origPart in origParts)
            {
                if (origPart.Notes is null || origPart.Notes.Count == 0)
                {
                    // Only doubles parts with at least 1 note.
                    continue;
                }

                var newPart = leftOrRightRandomizer.RandomizeAsNewPart(origPart);
                newParts.Add(newPart!);
            }

            return newParts;
        }
    }
}