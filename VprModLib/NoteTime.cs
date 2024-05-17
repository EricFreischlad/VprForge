using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VprModLib
{
    public struct NoteTime : IComparable<NoteTime>
    {
        /// <summary>
        /// The number of rhythmic frames in every beat. Constant. No relation to graphics or physics frames.
        /// </summary>
        public const int FRAMES_PER_BEAT = 480;

        public readonly int FrameIndex;

        public NoteTime(int frameIndex)
        {
            FrameIndex = frameIndex;
        }
        public NoteTime(int beatIndex, BeatSubdivision subdivision, int subdivisionIndex)
            : this(subdivision is { } ?
                 (beatIndex * FRAMES_PER_BEAT) + (subdivisionIndex * subdivision.Duration)
                 : throw new ArgumentNullException(nameof(subdivision)))
        {
            // NOTE: Calculate frameIndex from provided values and reduce them to simplest terms.
            // User can insert strange data like (beat: 0, subdivision: eighth note triplets, subdivisionIndex: 664). Same as FrameIndex 53120.
            // This will cause that to be reduced to simplest form. (beat: 110, subdivision: eighth note triplets, subdivisionIndex : 4)
        }
        public readonly RelativeNoteTimeInfo GetRelativeNoteTimeInfo()
        {
            bool isNegative = FrameIndex < 0;
            int beatIndex = Math.Abs(FrameIndex / FRAMES_PER_BEAT);
            var highestSub = BeatSubdivision.GetHighestSubdivisionOfFrameIndex(FrameIndex);
            int subdivisionIndex = Math.Abs(FrameIndex % FRAMES_PER_BEAT / highestSub.Duration);
            return new RelativeNoteTimeInfo(beatIndex, highestSub, subdivisionIndex, isNegative);
        }
        public readonly AbsoluteNoteTimeInfo GetAbsoluteNoteTimeInfo()
        {
            int beatIndex = FrameIndex / FRAMES_PER_BEAT;
            var highestSub = BeatSubdivision.GetHighestSubdivisionOfFrameIndex(FrameIndex);
            int subdivisionIndex = Math.Abs(FrameIndex % FRAMES_PER_BEAT / highestSub.Duration);
            return new AbsoluteNoteTimeInfo(beatIndex, highestSub, subdivisionIndex);
        }
        public readonly int CompareTo(NoteTime other)
        {
            return FrameIndex.CompareTo(other.FrameIndex);
        }

        #region Operators
        public static NoteTime operator +(NoteTime a, NoteTime b)
        {
            return new NoteTime(a.FrameIndex + b.FrameIndex);
        }
        public static NoteTime operator -(NoteTime a, NoteTime b)
        {
            return new NoteTime(a.FrameIndex - b.FrameIndex);
        }
        public static bool operator <(NoteTime a, NoteTime b)
        {
            return a.FrameIndex < b.FrameIndex;
        }
        public static bool operator >(NoteTime a, NoteTime b)
        {
            return a.FrameIndex > b.FrameIndex;
        }
        public static bool operator ==(NoteTime a, NoteTime b)
        {
            return a.FrameIndex == b.FrameIndex;
        }
        public static bool operator !=(NoteTime a, NoteTime b)
        {
            return a.FrameIndex != b.FrameIndex;
        }
        public override readonly bool Equals(object? obj)
        {
            if (obj is NoteTime other)
            {
                return FrameIndex == other.FrameIndex;
            }
            return false;
        }
        public override readonly int GetHashCode()
        {
            return FrameIndex.GetHashCode();
        }
        public static bool operator >=(NoteTime a, NoteTime b)
        {
            return a > b || a == b;
        }
        public static bool operator <=(NoteTime a, NoteTime b)
        {
            return a < b || a == b;
        }
        #endregion
    }
    public class RelativeNoteTimeInfo
    {
        public int Beat { get; set; }
        public BeatSubdivision Subdivision { get; set; }
        public int SubdivisionCount { get; set; }
        public bool IsBeforeStart { get; set; }
        public RelativeNoteTimeInfo(int beat, BeatSubdivision subdivision, int subdivisions, bool isBeforeStart)
        {
            Beat = beat;
            Subdivision = subdivision;
            SubdivisionCount = subdivisions;
            IsBeforeStart = isBeforeStart;
        }
        public override string ToString()
        {
            bool isOnBeat = Subdivision == BeatSubdivision.QuarterNotes;
            string beatPluralityString = Beat != 1 ? "beats" : "beat";

            if (IsBeforeStart)
            {
                if (isOnBeat)
                {
                    // Aligned with beats - no need to list subdivision.
                    // Example: "3 beats before the start"
                    return $"{Beat} {beatPluralityString} before the start";
                }
                else
                {
                    string subdivPluralityString = Subdivision.SheetMusicName + (SubdivisionCount != 1 ? $"s" : "");

                    // Example: "3 beats and 7 sixty-fourth notes before the start"
                    return $"{Beat} {beatPluralityString} and {SubdivisionCount} {subdivPluralityString} before the start";
                }
            }
            else
            {
                if (isOnBeat)
                {
                    // Aligned with beats - no need to list subdivision.
                    // Example: "3 beats"
                    return $"{Beat} {beatPluralityString}";
                }
                else
                {
                    string subdivPluralityString = Subdivision.SheetMusicName + (SubdivisionCount != 1 ? $"s" : "");

                    // Example: "3 beats and 7 sixty-fourth notes"
                    return $"{Beat} {beatPluralityString} and {SubdivisionCount} {subdivPluralityString}";
                }
            }
        }
    }
    public class AbsoluteNoteTimeInfo
    {
        public int BeatIndex { get; set; }
        public BeatSubdivision Subdivision { get; set; }
        public int SubdivisionIndex { get; set; }
        public AbsoluteNoteTimeInfo(int beatIndex, BeatSubdivision subdivision, int subdivisionIndex)
        {
            BeatIndex = beatIndex;
            Subdivision = subdivision;
            SubdivisionIndex = subdivisionIndex;
        }
        public override string ToString()
        {
            bool isOnBeat = Subdivision == BeatSubdivision.QuarterNotes;

            if (BeatIndex < 0)
            {
                int beatOrdinalMagnitude = Math.Abs(BeatIndex - 1);
                string beatOrdinalSuffix = GetOrdinalSuffix(beatOrdinalMagnitude);
                int subdivOrdinalMagnitude = (Subdivision.QuantityPerBeat - SubdivisionIndex) + 1;
                string subdivOrdinalSuffix = GetOrdinalSuffix(subdivOrdinalMagnitude);

                if (isOnBeat)
                {
                    // Aligned with beats - no need to list subdivision.
                    // Example: "4th beat before the start"
                    return $"{beatOrdinalMagnitude}{beatOrdinalSuffix} beat before the start";
                }
                else
                {
                    // Example: "58th sixty-fourth note of the 3rd beat before the start"
                    return $"{subdivOrdinalMagnitude}{subdivOrdinalSuffix} {Subdivision.SheetMusicName} of the {beatOrdinalMagnitude}{beatOrdinalSuffix} beat before the start";
                }
            }
            else
            {
                int beatOrdinalMagnitude = BeatIndex + 1;
                string beatOrdinalSuffix = GetOrdinalSuffix(beatOrdinalMagnitude);
                int subdivOrdinalMagnitude = SubdivisionIndex + 1;
                string subdivOrdinalSuffix = GetOrdinalSuffix(subdivOrdinalMagnitude);

                if (isOnBeat)
                {
                    // Aligned with beats - no need to list subdivision.
                    // Example: "4th beat"
                    return $"{beatOrdinalMagnitude}{beatOrdinalSuffix} beat";
                }
                else
                {
                    // Example: "8th sixty-fourth note of the 4th beat"
                    return $"{subdivOrdinalMagnitude}{subdivOrdinalSuffix} {Subdivision.SheetMusicName} of the {beatOrdinalMagnitude}{beatOrdinalSuffix} beat";
                }
            }
        }
        private static string GetOrdinalSuffix(int number)
        {
            // I'm proud of this completely unnecessary expression.
            return Math.Abs(number % 100) switch
            {
                11 => "th",
                12 => "th",
                13 => "th",
                int x when true => (x % 10) switch
                {
                    1 => "st",
                    2 => "nd",
                    3 => "rd",
                    _ => "th",
                },
            };
        }
    }
}