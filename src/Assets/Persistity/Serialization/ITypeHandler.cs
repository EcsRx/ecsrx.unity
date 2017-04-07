using System;

namespace Persistity.Serialization
{
    public interface ITypeHandler<Tin, Tout>
    {
        bool MatchesType(Type type);
        void HandleTypeSerialization(Tin state, object data, Type type);
        object HandleTypeDeserialization(Tout state, Type type);
    }
}