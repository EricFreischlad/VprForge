using VprModLib;

namespace VprForge
{
    public class ValueAdjustment
    {
        public int MaxReduction { get; set; }
        public int MaxIncrease { get; set; }
        public int? HardMin { get; set; }
        public int? HardMax { get; set; }

        public ValueAdjustment(int maxReduction, int maxIncrease, int? hardMin = null, int? hardMax = null)
        {
            MaxReduction = maxReduction;
            MaxIncrease = maxIncrease;
            HardMin = hardMin;
            HardMax = hardMax;
        }
        public int GetRandomNewValue(int originalValue, Random rng)
        {
            int theoreticalMin = originalValue - MaxReduction;
            int min = HardMin.HasValue ? Math.Max(theoreticalMin, HardMin.Value) : theoreticalMin;

            int theoreticalMax = originalValue + MaxIncrease;
            int max = HardMax.HasValue ? Math.Min(theoreticalMax, HardMax.Value) : theoreticalMax;

            return rng.Next(min, max + 1);
        }
    }
    public class NoteTimeValueAdjustment : ValueAdjustment
    {
        public NoteTimeValueAdjustment(NoteTime maxEarly, NoteTime maxLate, bool dontAllowNegativeValues)
            : base(maxEarly.FrameIndex, maxLate.FrameIndex, dontAllowNegativeValues ? 0 : null, null)
        { }
        public NoteTime GetNewRandomNoteTime(NoteTime original, Random rng)
        {
            return new NoteTime(GetRandomNewValue(original.FrameIndex, rng));
        }
    }
}