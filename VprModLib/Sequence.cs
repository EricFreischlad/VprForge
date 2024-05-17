using System.Text.Json.Serialization;

namespace VprModLib
{
    public class Sequence
    {
        /// <summary>
        /// The version of Vocaloid that this project was created with (external programs should leave this alone).
        /// </summary>
        public VersionNumber VersionNumber { get; set; }
        /// <summary>
        /// The name of the vendor. Unknown if there are other possibilities besides "Yamaha Corporation".
        /// </summary>
        public string Vender { get; set; } = string.Empty;
        /// <summary>
        /// The title of the project file, as set by the Vocaloid Editor (not necessarily the same as the file name).
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// The master control track which controls looping, tempo, time signatures, and global volume events/states.
        /// </summary>
        public MasterTrack MasterTrack { get; set; } = new MasterTrack();
        /// <summary>
        /// All voices used in the project.
        /// </summary>
        public List<Voice> Voices { get; } = new List<Voice>();
        /// <summary>
        /// The tracks which contain either Vocaloid or Audio Parts.
        /// </summary>
        public List<Track> Tracks { get; } = new List<Track>();

        // TODO: Convenience method for getting all Vocaloid or Audio tracks by type.
    }
}
namespace VprModLib.Serialization
{
    public class SerializedSequence : ISerialized<Sequence>
    {
        public SerializedVersionNumber version;
        public string vender;
        public string title;
        public SerializedMasterTrack masterTrack;
        
        // Empty array when no parts exist, but not omitted.
        public SerializedVoice[] voices;

        // Always present.
        public SerializedTrack[] tracks;

        [JsonConstructor]
        public SerializedSequence(SerializedVersionNumber version, string vender, string title, SerializedMasterTrack masterTrack, SerializedVoice[] voices, SerializedTrack[] tracks)
        {
            this.version = version;
            this.vender = vender;
            this.title = title;
            this.masterTrack = masterTrack;
            this.voices = voices;
            this.tracks = tracks;
        }
        public SerializedSequence(Sequence model)
        {
            version = new SerializedVersionNumber(model.VersionNumber);
            vender = model.Vender;
            title = model.Title;
            masterTrack = new SerializedMasterTrack(model.MasterTrack);
            voices = model.Voices.Select(v => new SerializedVoice(v)).ToArray();
            tracks = model.Tracks.Select(SerializedTrack.CreateFromModel).ToArray();
        }

        public bool IsValid()
        {
            return version is { }
                && version.IsValid()
                // I actually don't care if these are empty.
                && vender is { }
                && title is { }
                && masterTrack is { }
                && masterTrack.IsValid()
                && voices is { }
                && voices.All(v => v.IsValid())
                && tracks is { }
                && tracks.All(t => t.IsValid());
        }

        public Sequence ToModel()
        {
            var model = new Sequence()
            {
                VersionNumber = version.ToModel(),
                Vender = vender,
                Title = title,
                MasterTrack = masterTrack.ToModel(),
            };
            model.Voices.AddRange(voices.Select(v => v.ToModel()));
            model.Tracks.AddRange(tracks.Select(t => t.ToConcreteModel()));
            return model;
        }
    }
}