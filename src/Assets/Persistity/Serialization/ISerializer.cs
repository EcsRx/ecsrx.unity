namespace Persistity.Serialization
{
    public interface ISerializer
    {
        DataObject Serialize(object data);
    }
}