using VprModLib.Serialization;

namespace VprModLib.AudioEffects
{
    public partial class EffectType
    {
        private static readonly Dictionary<string, EffectType> _registeredEffectTypes;
        public static IReadOnlyDictionary<string, EffectType> RegisteredEffectTypes => _registeredEffectTypes;
        public static EffectType Get(string id) => _registeredEffectTypes.TryGetValue(id, out var effectType)
            ? effectType
            : throw new ArgumentException($"Unrecognized effect ID: \"{id}\".", nameof(id));



        private readonly Dictionary<string, EffectParameterDefinition> _parameterDefinitions;
        public IReadOnlyDictionary<string, EffectParameterDefinition> ParameterDefinitions => _parameterDefinitions;

        public string ID { get; }
        public string Name { get; }
        public EffectType(string id, string name, List<EffectParameterDefinition> parameterDefinitions)
        {
            ID = id;
            Name = name;
            _parameterDefinitions = parameterDefinitions.ToDictionary(epd => epd.Name);
        }

        // NOTE: Effect type definitions are located within this class, but in another document.
    }
}