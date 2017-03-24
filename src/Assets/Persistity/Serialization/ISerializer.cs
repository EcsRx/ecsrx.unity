using Persistity.Mappings;

namespace Persistity.Serialization
{
    public interface ISerializer
    {
        byte[] SerializeData<TInput>(TypeMapping typeMapping, TInput data) where TInput : new();
        byte[] SerializeData(TypeMapping typeMapping, object data);
    }
}