namespace VprModLib
{
    public class AudioFiles
    {
        private readonly Dictionary<string, byte[]> _filesByName;

        public IReadOnlyCollection<string> Names => _filesByName.Keys;

        public AudioFiles(Dictionary<string, byte[]> filesByName)
        {
            _filesByName = filesByName;
        }
        public bool TryGetBytes(string name, out ReadOnlySpan<byte> bytes)
        {
            if (_filesByName.TryGetValue(name, out var origBytes))
            {
                bytes = origBytes;
                return true;
            }

            bytes = null;
            return false;
        }
        public bool Add(string name, byte[] bytes)
        {
            if (_filesByName.ContainsKey(name))
            {
                return false;
            }
            
            _filesByName.Add(name, bytes);
            return true;
        }
        public bool Remove(string name)
        {
            return _filesByName.Remove(name);
        }
        public Dictionary<string, byte[]> GetForSerialization()
        {
            return _filesByName;
        }
    }
}