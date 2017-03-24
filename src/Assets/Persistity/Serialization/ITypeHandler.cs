using System;

namespace Persistity.Serialization
{
    public interface ITypeHandler<Tin, Tout>
    {
        bool MatchesType(Type type);
        void HandleTypeIn(Tin state, object data);
        object HandleTypeOut(Tout state);
    }
}