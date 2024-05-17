namespace VprModLib
{
    public readonly struct AudioFileName
    {
        public string Name { get; }
        public string Extension { get; }

        private AudioFileName(string name, string extension)
        {
            Name = name;
            Extension = extension;
        }
        public override string ToString()
        {
            return $"{Name}.{Extension}";
        }

        public static AudioFileName CreateFromExisting(string name, string extension)
        {
            return new AudioFileName(name, extension);
        }
        public static AudioFileName CreateFromExisting(string nameWithExtension)
        {
            int extensionLength = nameWithExtension.Split('.').Last().Length;
            return new AudioFileName(nameWithExtension[..^extensionLength], nameWithExtension[^extensionLength..]);
        }
        public static AudioFileName CreateWithRandomName(string extension)
        {
            return new AudioFileName(Guid.NewGuid().ToString().ToLower(), extension);
        }
    }
}
