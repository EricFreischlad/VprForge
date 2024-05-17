using System.Text.Json.Serialization;

namespace VprModLib
{
    /// <summary>
    /// Information about a specific voicebank used in the project.
    /// </summary>
    public class Voice
    {
        /// <summary>
        /// An ID string that appears to be unique to the voicebank, but not the computer.
        /// </summary>
        public string CompID { get; set; } = string.Empty;
        /// <summary>
        /// The human-readable name of the voicebank.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
namespace VprModLib.Serialization
{
    public class SerializedVoice : ISerialized<Voice>
    {
        public string compID;
        public string name;

        [JsonConstructor]
        public SerializedVoice(string compID, string name)
        {
            this.compID = compID;
            this.name = name;
        }
        public SerializedVoice(Voice model)
        {
            compID = model.CompID;
            name = model.Name;
        }

        public bool IsValid()
        {
            // NOTE: Unable to validate compID or name.
            return !string.IsNullOrEmpty(compID)
                && !string.IsNullOrEmpty(name);
        }

        public Voice ToModel()
        {
            return new Voice()
            {
                CompID = compID,
                Name = name,
            };
        }
    }
}