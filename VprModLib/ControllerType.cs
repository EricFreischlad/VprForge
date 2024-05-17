namespace VprModLib
{
    public partial class ControllerType
    {
        public string ProjectName { get; }
        public string DisplayName { get; }
        public int MinValue { get; }
        public int MaxValue { get; }
        public int DefaultValue { get; }
        public ControllerType(string projectName, string displayName, int minValue, int maxValue, int defaultValue)
        {
            ProjectName = projectName;
            DisplayName = displayName;
            MinValue = minValue;
            MaxValue = maxValue;
            DefaultValue = defaultValue;
        }

        private static readonly Dictionary<ControllerName, ControllerType> _registeredControllerTypes;
        public static IReadOnlyDictionary<ControllerName, ControllerType> RegisteredControllerTypes => _registeredControllerTypes;
        public static ControllerType Get(ControllerName name) => _registeredControllerTypes.TryGetValue(name, out var effectType)
            ? effectType
            : throw new ArgumentException($"Unrecognized controller name: \"{name}\".", nameof(name));
        public static ControllerType Get(string projectName) => _registeredControllerTypes.Values.First(ct => ct.ProjectName.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
        public static bool IsNameValid(string projectName) => _registeredControllerTypes.Values.Any(ct => ct.ProjectName.Equals(projectName, StringComparison.InvariantCulture));

        static ControllerType()
        {
            _registeredControllerTypes = new Dictionary<ControllerName, ControllerType>()
            {
                { ControllerName.AIR, new ControllerType("air", "Air", 0, 127, 0) },
                { ControllerName.BREATHINESS, new ControllerType("breathiness", "Breathiness", 0, 127, 0) },
                { ControllerName.BRIGHTNESS, new ControllerType("brightness", "Brightness", 0, 127, 64) },
                { ControllerName.CHARACTER, new ControllerType("character", "character", -64, 63, 0) },
                { ControllerName.CLEARNESS, new ControllerType("clearness", "Clearness", 0, 127, 0) },
                { ControllerName.DYNAMICS, new ControllerType("dynamics", "Dynamics", 0, 127, 64) },
                { ControllerName.EXCITER, new ControllerType("exciter", "Exciter", -64, 63, 0) },
                { ControllerName.GROWL, new ControllerType("growl", "Growl", 0, 127, 0) },
                { ControllerName.PITCH_BEND, new ControllerType("pitchBend", "Pitch Bend", -8192, 8191, 0) },
                { ControllerName.PITCH_BEND_SENSITIVITY, new ControllerType("pitchBendSens", "Pitch Bend Sensitivity", 0, 24, 2) },
                { ControllerName.PORTAMENTO, new ControllerType("portamento", "Portamento Timing", 0, 127, 64) },
            };
        }
    }
}
