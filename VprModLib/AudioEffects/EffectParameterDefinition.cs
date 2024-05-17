namespace VprModLib.AudioEffects
{
    public abstract class EffectParameterDefinition
    {
        public string Name { get; }
        public Type ValueType { get; }
        public object DefaultValue { get; }

        protected EffectParameterDefinition(string name, Type type, object defaultValue)
        {
            Name = name;
            ValueType = type;
            DefaultValue = defaultValue;
        }

        public abstract bool IsValid(object testedValue);
        public object GetValue(Effect effect)
        {
            var value = effect.WeakParameters[Name];
            if (value.GetType() != ValueType)
            {
                throw new InvalidOperationException($"Type mismatch when getting the value of \"{Name}\". Type was \"{value.GetType()}\". Expected: \"{ValueType}\".");
            }
            return value;
        }
        public void SetValue(Effect effect, object value)
        {
            if (value.GetType() != ValueType)
            {
                throw new ArgumentException($"Type mismatch when setting the value of \"{Name}\". Type was \"{value.GetType()}\". Expected: \"{ValueType}\".", nameof(value));
            }

            effect.WeakParameters[Name] = value;
        }
    }
    public abstract class EffectParameterDefinition<T> : EffectParameterDefinition
    {
        private readonly Predicate<T>? _isValidPredicate;

        protected EffectParameterDefinition(string name, T defaultValue, Predicate<T>? isValidPredicate = null)
            : base(name, typeof(T), defaultValue!)
        {
            _isValidPredicate = isValidPredicate;
        }

        public override bool IsValid(object testedValue)
        {
            // Type check.
            return testedValue is T strongValue
                // Explicit predicate check. NULL predicate is ignored.
                && (_isValidPredicate is not { } || _isValidPredicate(strongValue));
        }
        new public T GetValue(Effect effect)
        {
            return (T)effect.WeakParameters[Name];
        }
        public void SetValue(Effect effect, T value)
        {
            effect.WeakParameters[Name] = value!;
        }
    }
    
    /*
     *      Doubles
     */

    public class KnobDoubleEffectParameterDefinition : EffectParameterDefinition<double>
    {
        public KnobDoubleEffectParameterDefinition(string name, double defaultValue, (double Min, double Max)? limits)
            // No limits provided or value is within provided limits.
            :base(name, defaultValue, d => limits is null || (d >= limits.Value.Min && d <= limits.Value.Max))
        { }
    }
    public class ButtonDoubleEffectParameterDefinition : EffectParameterDefinition<double>
    {
        public ButtonDoubleEffectParameterDefinition(string name, double defaultValue, (double Off, double On) states)
            // Button is in "on" or "off" state.
            : base(name, defaultValue, d => d == states.Off || d == states.On)
        { }
    }
    public class EnumDoubleEffectParameterDefinition : EffectParameterDefinition<double>
    {
        public IReadOnlyList<string> Options { get; }
        
        public EnumDoubleEffectParameterDefinition(string name, string defaultOption, List<string> options)
            :base(name,
                 // Get the normalized index based on the total number of options.
                 options.IndexOf(defaultOption) / (double)options.Count,
                 // Get the index from the normalized index based on the total number of options.
                 d => d >= 0.0 && d <= 1.0 && d % (1 / (double)options.Count) == 0)
        {
            Options = options;
        }
    }
    public class TempoSyncEffectParameterDefinition : EnumDoubleEffectParameterDefinition
    {
        public TempoSyncEffectParameterDefinition(string name)
            :base(name, "Hz", new List<string>()
            {
                "Hz",
                "Tempo"
            })
        { }
    }
    public class SyncNoteEffectParameterDefinition : EnumDoubleEffectParameterDefinition
    {
        public SyncNoteEffectParameterDefinition(string name)
            :base(name, "1/1", new List<string>()
            {
                // "Clean" divisions of the beat.
                "1/1",
                "1/2",
                "1/4",
                "1/8",
                "1/16",
                "1/32",

                // Triplet divisions of the beat. 1/3 the nominal value.
                "1/1T",
                "1/2T",
                "1/4T",
                "1/8T",
                "1/16T",
                "1/32T",

                // Dotted divisions of the beat. 3/2 the nominal value.
                "1/1D",
                "1/2D",
                "1/4D",
                "1/8D",
                "1/16D",
                "1/32D",
            })
        { }
    }

    /*
     *      Ints
     */

    public class KnobIntEffectParameterDefinition : EffectParameterDefinition<int>
    {
        public KnobIntEffectParameterDefinition(string name, int defaultValue, (int Min, int Max)? limits)
            // No limits provided or value is within provided limits.
            : base(name, defaultValue, i => limits is null || (i >= limits.Value.Min && i <= limits.Value.Max))
        { }
    }
    public class ButtonIntEffectParameterDefinition : EffectParameterDefinition<double>
    {
        public ButtonIntEffectParameterDefinition(string name, int defaultValue, (int Off, int On) states)
            // Button is in "on" or "off" state.
            : base(name, defaultValue, i => i == states.Off || i == states.On)
        { }
    }
    public class EnumIntEffectParameterDefinition : EffectParameterDefinition<int>
    {
        public IReadOnlyList<string> Options { get; }

        public EnumIntEffectParameterDefinition(string name, string defaultOption, List<string> options)
            : base(name,
                 // Get the index of this option.
                 options.IndexOf(defaultOption),
                 // Get total number of options.
                 i => i >= 0 && i <= options.Count)
        {
            Options = options;
        }
    }

    /*
     *      Strings
     */

    public class StringEffectParameterDefinition : EffectParameterDefinition<string>
    {
        public StringEffectParameterDefinition(string name, string defaultValue)
            :base(name, defaultValue,
                 s => s is not null)
        { }
    }
}