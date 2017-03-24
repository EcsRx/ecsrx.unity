using System;

namespace Persistity.Mappings.Mappers
{
    public interface ITypeMapper
    {
        TypeMapping GetTypeMappingsFor(Type type);
    }
}