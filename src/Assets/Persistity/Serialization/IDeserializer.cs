namespace Persistity.Serialization
{
    public interface IDeserializer
    {
        object Deserialize(DataObject data);
        T Deserialize<T>(DataObject data) where T : new();
    }
}