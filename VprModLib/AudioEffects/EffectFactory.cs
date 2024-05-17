using VprModLib.Serialization;

namespace VprModLib.AudioEffects
{
    public static class EffectFactory
    {
        public static Effect CreateEffectModelFromSerialized(SerializedEffect serialized)
        {
            var eType = EffectType.Get(serialized.id);
            var model = new Effect(eType)
            {
                ID = serialized.id,
                IsFolded = serialized.isFolded,
                IsBypassed = serialized.isBypassed
            };

            int paramCount = eType.ParameterDefinitions.Count;
            for (int i = 0; i < paramCount; i++)
            {
                var serializedParam = serialized.parameters[i];
                model.WeakParameters[serializedParam.name] = serializedParam.value;
            }

            return model;
        }
    }
}
