using System.Text.Json.Serialization;

namespace VprModLib
{
    /// <summary>
    /// Either an attack or release expression in the DVQM section of a note object.
    /// </summary>
    public class DvqmAttackRelease
    {
        /// <summary>
        /// Unique identifier for the AttackRelease pack this effect belongs to. Stands for "Component ID", probably.
        /// </summary>
        public string CompID { get; set; } = string.Empty;
        /// <summary>
        /// Ordered series of tags for an expression preset. Essentially its name.
        /// </summary>
        public List<string> LevelNames { get; } = new List<string>();
        /// <summary>
        /// Is the expression protected from automatic changes? If not, it can only be manually overwritten.
        /// </summary>
        public bool IsProtected { get; set; }
        /// <summary>
        /// How long the expression takes to complete. 0 to 100. 0 = instant, 100 = long.
        /// </summary>
        public int Speed { get; set; }
        /// <summary>
        /// The intensity of the expression. 0.0 to 2.0. 0.0 = non-existant, 2.0 = extreme.
        /// </summary>
        public double TopFactor { get; set; } = 0.5;
    }
}

namespace VprModLib.Serialization
{
    public class SerializedDvqmAttackRelease : ISerialized<DvqmAttackRelease>
    {
        public string compID;
        public string[] levelNames;
        public bool isProtected;
        public int speed;
        public double topFactor;

        [JsonConstructor]
        public SerializedDvqmAttackRelease(string compID, string[] levelNames, bool isProtected, int speed, double topFactor)
        {
            this.compID = compID;
            this.levelNames = levelNames;
            this.isProtected = isProtected;
            this.speed = speed;
            this.topFactor = topFactor;
        }
        public SerializedDvqmAttackRelease(DvqmAttackRelease model)
        {
            compID = model.CompID;
            levelNames = model.LevelNames.ToArray();
            isProtected = model.IsProtected;
            speed = model.Speed;
            topFactor = model.TopFactor;
        }

        public bool IsValid()
        {
            // NOTE: Unable to check validity of "compID".
            // NOTE: Unable to check validity of individual "levelNames" entries.
            return levelNames is { }
                && levelNames.Length > 0
                && speed >= 0 && speed <= 100
                && topFactor >= 0.0 && topFactor <= 2.0;
        }

        public DvqmAttackRelease ToModel()
        {
            var model = new DvqmAttackRelease()
            {
                CompID = compID,
                IsProtected = isProtected,
                Speed = speed,
                TopFactor = topFactor
            };
            model.LevelNames.AddRange(levelNames);
            return model;
        }
    }
}