namespace VprModLib.Serialization
{
    public interface ISerialized<T>
    {
        T ToModel();
        bool IsValid();
    }
}