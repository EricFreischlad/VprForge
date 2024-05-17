using System.Text.Json.Serialization;

namespace VprModLib
{
    /// <summary>
    /// References an audio file within the project's Audio folder.
    /// </summary>
    public class AudioPartWav
    {
        // NOTE: One should be able to import custom audio this way, so long as it is never adjusted (and therefore overwritten by its "originalName" file).
        
        /// <summary>
        /// The name of the WAV file in the project's Audio folder. All effects have been applied.
        /// </summary>
        public AudioFileName Name { get; set; }
        /// <summary>
        /// The name of the original WAV file this was created from. This reference exists to know where to copy from before applying new effects.
        /// </summary>
        public AudioFileName OriginalName { get; set; }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedAudioPartWav : ISerialized<AudioPartWav>
    {
        public string name;
        public string originalName;

        [JsonConstructor]
        public SerializedAudioPartWav(string name, string originalName)
        {
            this.name = name;
            this.originalName = originalName;
        }
        public SerializedAudioPartWav(AudioPartWav model)
        {
            // .ToString() is overridden for AudioFileName.
            name = model.Name.ToString();
            originalName = model.OriginalName.ToString();
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(name)
                && !string.IsNullOrEmpty(originalName);
        }

        public AudioPartWav ToModel()
        {
            return new AudioPartWav()
            {
                Name = AudioFileName.CreateFromExisting(name),
                OriginalName = AudioFileName.CreateFromExisting(originalName),
            };
        }
    }
}