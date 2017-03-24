using Persistity.Mappings;

namespace Persistity.Serialization
{
    public interface IDeserializer
    {
        TOutput DeserializeData<TOutput>(TypeMapping typeMapping, byte[] data) where TOutput : new();
        object DeserializeData(TypeMapping typeMapping, byte[] data);
    }
}