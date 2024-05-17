using System.Text.Json.Serialization;

namespace VprModLib
{
    /// <summary>
    /// Section for Attack and Release expressions for a note. It is unclear what DVQM stands for. Don't Vorry Qbout Mt.
    /// </summary>
    public class Dvqm
    {
        public DvqmAttackRelease? Attack { get; set; }
        public DvqmAttackRelease? Release { get; set; }
    }
}
namespace VprModLib.Serialization
{
    public class SerializedDvqm : ISerialized<Dvqm>
    {
        public SerializedDvqmAttackRelease? attack;
        public SerializedDvqmAttackRelease? release;

        [JsonConstructor]
        public SerializedDvqm(SerializedDvqmAttackRelease attack, SerializedDvqmAttackRelease release)
        {
            this.attack = attack;
            this.release = release;
        }
        public SerializedDvqm(Dvqm model)
        {
            attack = model.Attack is { } ? new SerializedDvqmAttackRelease(model.Attack) : null;
            release = model.Release is { } ? new SerializedDvqmAttackRelease(model.Release) : null;
        }

        public bool IsValid()
        {
            return (attack is { } || release is { })
                && (attack is null
                    || attack.IsValid())
                && (release is null
                    || release.IsValid());
        }

        public Dvqm ToModel()
        {
            return new Dvqm()
            {
                Attack = attack is { } ? attack.ToModel() : null,
                Release = release is { } ? release.ToModel() : null,
            };
        }
    }
}