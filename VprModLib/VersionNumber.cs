using System.Text.Json.Serialization;

namespace VprModLib
{
    public record struct VersionNumber(int Major, int Minor, int Revision)
    {
        public override string ToString()
        {
            return $"{Major}.{Minor}.{Revision}";
        }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedVersionNumber : ISerialized<VersionNumber>
    {
        public int major;
        public int minor;
        public int revision;

        [JsonConstructor]
        public SerializedVersionNumber(int major, int minor, int revision)
        {
            this.major = major;
            this.minor = minor;
            this.revision = revision;
        }
        public SerializedVersionNumber(VersionNumber model)
        {
            major = model.Major;
            minor = model.Minor;
            revision = model.Revision;
        }

        public VersionNumber ToModel()
        {
            return new VersionNumber(major, minor, revision);
        }
        public bool IsValid()
        {
            return major >= 0
                && minor >= 0
                && revision >= 0;
        }
    }
}