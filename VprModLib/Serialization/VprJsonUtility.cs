using System.Text.Json;

namespace VprModLib.Serialization
{
    public static class VprJsonUtility
    {
        public static readonly JsonSerializerOptions _jsonSerializationOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            IncludeFields = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict,
            PropertyNamingPolicy = null,
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = false,

            // NULL values will not be serialized.
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        };
        public static readonly JsonSerializerOptions _jsonDeserializationOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            IncludeFields = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict,
            PropertyNamingPolicy = null,
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = false,
        };

        static VprJsonUtility()
        {
            _jsonSerializationOptions.Converters.Add(
                new SerializedTrackConverter()
                );

            _jsonDeserializationOptions.Converters.Add(
                new SerializedTrackConverter()
                );
        }

        public static bool TryDeserialize(string sequenceJsonStr, out SerializedSequence? serializedSequence, out string message)
        {
            serializedSequence = null;
            message = $"Unhandled exception in TryDeserialize().";

            try
            {
                serializedSequence = JsonSerializer.Deserialize<SerializedSequence>(sequenceJsonStr, _jsonDeserializationOptions);

                message = "Success.";
                return true;
            }
            catch (ArgumentNullException)
            {
                message = "The provided string was NULL while deserializing from JSON. " + FileIO.INTERNAL_ERROR;
                return false;
            }
            catch (JsonException e)
            {
                message = "There was problem while deserializing the internal sequence file: " + e;
            }
            catch (NotSupportedException e)
            {
                message = $"Missing JSON converter for type. {FileIO.INTERNAL_ERROR}. Additional info: {e}";
            }

            return false;
        }
        public static bool TrySerialize(SerializedSequence sequence, out string? sequenceJsonStr, out string message)
        {
            sequenceJsonStr = null;
            message = $"Unhandled exception in TrySerialize().";

            try
            {
                sequenceJsonStr = JsonSerializer.Serialize(sequence, _jsonSerializationOptions);
                message = "Success.";
                return true;
            }
            catch (NotSupportedException e)
            {
                message = $"Missing JSON converter for type. {FileIO.INTERNAL_ERROR}. Additional info: {e}";
            }

            return false;
        }
    }
}