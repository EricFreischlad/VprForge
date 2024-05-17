using System.Text.Json.Serialization;

namespace VprModLib
{
    /// <summary>
    /// Voicebank info for a Vocaloid Part. CompID must match a voicebank listed in the Voices property of the Sequence.
    /// </summary>
    public class PartVoice
    {
        public string CompID { get; set; } = string.Empty;
        public LangID LangID { get; set; }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedPartVoice : ISerialized<PartVoice>
    {
        public string compID;
        public int langID;

        [JsonConstructor]
        public SerializedPartVoice(string compID, int langID)
        {
            this.compID = compID;
            this.langID = langID;
        }
        public SerializedPartVoice(PartVoice model)
        {
            compID = model.CompID;
            langID = (int)model.LangID;
        }

        public bool IsValid()
        {
            // NOTE: CompID can't be validated.
            return !string.IsNullOrEmpty(compID)
                && langID >= 0;
        }

        public PartVoice ToModel()
        {
            return new PartVoice
            {
                CompID = compID,
                LangID = (LangID)langID,
            };
        }
    }
}