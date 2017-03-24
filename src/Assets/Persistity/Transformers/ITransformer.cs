using System;

namespace Persistity.Transformers
{
    public interface ITransformer
    {
        byte[] Transform<T>(T data) where T : new();
        byte[] Transform(Type type, object data);
        T Transform<T>(byte[] data) where T : new();
        object Transform(Type type, byte[] data);
    }
}