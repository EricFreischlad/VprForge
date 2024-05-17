using System.Collections.Generic;

namespace VprModLib
{
    /// <summary>
    /// A subdivision of a beat. Contains helper methods for quick maths when working with rhythmic content. All instances of the type are available as static readonly instances, and can be enumerated/counted through Subdivision.All;
    /// </summary>
    public sealed class BeatSubdivision
    {
        // Collection of all subdivisions.
        private static readonly List<BeatSubdivision> _allSubdivisions = new List<BeatSubdivision>();

        // Enumeration/Counting access.
        /// <summary>
        /// Collection of all rhythmic subdivisions for enumeration or counting.
        /// </summary>
        public static ICollection<BeatSubdivision> All => _allSubdivisions;

        // Static instances.
        public static readonly BeatSubdivision QuarterNotes = new BeatSubdivision("Beats", "quarter note", 1);
        public static readonly BeatSubdivision QuarterNoteTriplets = new BeatSubdivision("Third Beats", "quarter note triplet", 3);
        public static readonly BeatSubdivision EighthNotes = new BeatSubdivision("1/2 Beats", "eighth note", 2);
        public static readonly BeatSubdivision EighthNoteTriplets = new BeatSubdivision("1/6 Beats", "eighth note triplet", 6);
        public static readonly BeatSubdivision SixteenthNotes = new BeatSubdivision("1/4 Beats", "sixteenth note", 4);
        public static readonly BeatSubdivision SixteenthNoteTriplets = new BeatSubdivision("1/12 Beats", "sixteenth note triplet", 12);
        public static readonly BeatSubdivision ThirtySecondNotes = new BeatSubdivision("1/8 Beats", "thirty-second note", 8);
        public static readonly BeatSubdivision ThirtySecondNoteTriplets = new BeatSubdivision("1/24 Beats", "thirty-second note triplet", 24);
        public static readonly BeatSubdivision SixtyFourthNotes = new BeatSubdivision("1/16 Beats", "sixty-fourth note", 16);
        public static readonly BeatSubdivision SixtyFourthNoteTriplets = new BeatSubdivision("1/48 Beats", "sixty-fourth note triplet", 48);
        public static readonly BeatSubdivision Unaligned = new BeatSubdivision("Unaligned", "frame", NoteTime.FRAMES_PER_BEAT);

        // Instance members.

        /// <summary>
        /// The name of the subdivision in relation to a beat.
        /// </summary>
        public string BeatDivisionName { get; }
        /// <summary>
        /// The name of the subdivision according to music theory students.
        /// </summary>
        public string SheetMusicName { get; }
        /// <summary>
        /// Number of subdivisions per beat. (Ex. There are 4 sixteenth notes, or 1/4 beats, in a beat.)
        /// </summary>
        public int QuantityPerBeat { get; }
        /// <summary>
        /// Number of frames in the subdivision. Same as (frames per beat / quantity per beat). (Ex. An eighth note is 12 frames long.)
        /// </summary>
        public int Duration { get; }

        private BeatSubdivision(string beatDivisionName, string sheetMusicName, int quantityPerBeat)
        {
            BeatDivisionName = beatDivisionName;
            SheetMusicName = sheetMusicName;
            QuantityPerBeat = quantityPerBeat;
            Duration = NoteTime.FRAMES_PER_BEAT / quantityPerBeat;

            _allSubdivisions.Add(this);
        }
        /// <summary>
        /// Returns a value whether this subdivision is aligned with a given frame index. (Ex. "1/8 note triplets" includes frame indexes 0, 80, 160, 240, 320, and 400.)
        /// </summary>
        /// <param name="frameIndex">any rhythmic frame index</param>
        /// <returns>true if the subdivision includes the exact frame index</returns>
        public bool AlignsWithIndex(int frameIndex)
        {
            if (frameIndex < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(frameIndex));
            }

            return frameIndex % Duration == 0;
        }
        /// <summary>
        /// Get the most recent frame index that aligned with this subdivision.
        /// </summary>
        /// <param name="frameIndex">any rhythmic frame index</param>
        /// <returns>the most recent frame index (without going over)</returns>
        public int GetPreviousAlignedFrameIndex(int frameIndex)
        {
            if (frameIndex < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(frameIndex));
            }

            return frameIndex - (frameIndex % Duration);
        }
        /// <summary>
        /// Get the next frame index that aligns with this subdivision (or itself, if it aligns).
        /// </summary>
        /// <param name="frameIndex">any rhythmic frame index</param>
        /// <returns>the next frame index (or itself)</returns>
        public int GetNextAlignedFrameIndex(int frameIndex)
        {
            if (frameIndex < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(frameIndex));
            }

            int remainder = frameIndex % Duration;

            return remainder == 0 ? frameIndex : frameIndex + (Duration - remainder);
        }
        /// <summary>
        /// Get the highest subdivision of a given frame index. If the frame index aligns with multiple subdivisions, this method returns the one with the least subdivisions per beat.
        /// </summary>
        /// <param name="frameIndex">any rhythmic frame index</param>
        public static BeatSubdivision GetHighestSubdivisionOfFrameIndex(int frameIndex)
        {
            // This is done a lot and a switch lookup table is easier on CPU than using math.
            // A switch statement ends up looking cleaner than a switch expression in this case.
            int remainder = Math.Abs(frameIndex % NoteTime.FRAMES_PER_BEAT);
            switch (remainder)
            {
                case 20:
                case 100:
                case 140:
                case 220:
                case 260:
                case 340:
                case 380:
                case 460:
                    return ThirtySecondNoteTriplets;
                case 30:
                case 90:
                case 150:
                case 210:
                case 270:
                case 330:
                case 390:
                case 450:
                    return SixtyFourthNotes;
                case 40:
                case 200:
                case 280:
                case 440:
                    return SixteenthNoteTriplets;
                case 60:
                case 180:
                case 300:
                case 420:
                    return ThirtySecondNotes;
                case 80:
                case 400:
                    return EighthNoteTriplets;
                case 120:
                case 360:
                    return SixteenthNotes;
                case 160:
                case 320:
                    return QuarterNoteTriplets;
                case 240:
                    return EighthNotes;
                case 0:
                    return QuarterNotes;
                default:
                    if (remainder > 0)
                    {
                        return Unaligned;
                    }
                    else
                    {
                        // Input was negative.
                        throw new System.ArgumentOutOfRangeException(nameof(frameIndex));
                    }
            }
        }
    }
}
