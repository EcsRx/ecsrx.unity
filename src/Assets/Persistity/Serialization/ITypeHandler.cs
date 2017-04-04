using System;

namespace Persistity.Serialization
{
    public interface ITypeHandler<Tin, Tout>
    {
        bool MatchesType(Type type);
        void HandleTypeSerialization(Tin state, object data);
        object HandleTypeDeserialization(Tout state);
    }
}