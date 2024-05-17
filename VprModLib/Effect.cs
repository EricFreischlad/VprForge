using System.Text.Json;
using System.Text.Json.Serialization;
using VprModLib.AudioEffects;

namespace VprModLib
{
    /// <summary>
    /// 
    /// </summary>
    public class Effect
    {
        public EffectType EffectType { get; }

        public Effect(EffectType effectType)
        {
            EffectType = effectType;
        }

        /// <summary>
        /// A GUID for audio effects. A name for MIDI effects.
        /// </summary>
        public string ID { get; set; } = string.Empty;
        /// <summary>
        /// Will the synthesis engine ignore this effect?
        /// </summary>
        public bool IsBypassed { get; set; }
        /// <summary>
        /// Is the audio effect hidden in the editor?
        /// </summary>
        public bool IsFolded { get; set; }
        /// <summary>
        /// A dictionary of weakly-typed parameter values keyed by parameter name.
        /// </summary>
        public Dictionary<string, object> WeakParameters { get; } = new Dictionary<string, object>();
    }
}
namespace VprModLib.Serialization
{
    public class SerializedEffect
    {
        public string id;
        public bool isBypassed;
        public bool isFolded;
        public SerializedEffectParameter[] parameters;

        [JsonConstructor]
        public SerializedEffect(string id, bool isBypassed, bool isFolded, SerializedEffectParameter[] parameters)
        {
            this.id = id;
            this.isBypassed = isBypassed;
            this.isFolded = isFolded;
            this.parameters = parameters;
        }
        public SerializedEffect(Effect model)
        {
            id = model.ID;
            isBypassed = model.IsBypassed;
            isFolded = model.IsFolded;
            parameters = model.WeakParameters.Select(kvp => new SerializedEffectParameter()
            {
                name = kvp.Key,
                value = kvp.Value,
            }).ToArray();
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(id)
                && (EffectType.Get(id) is EffectType eType)
                && parameters is { }
                && parameters.All(p => eType.ParameterDefinitions[p.name] == p.value);
        }

        public Effect ToModel()
        {
            return EffectFactory.CreateEffectModelFromSerialized(this);
        }
    }
    public class SerializedEffectParameter
    {
        public string name;
        public object value;
    }
}