using System;
using Persistity.Mappings;
using Persistity.Mappings.Mappers;

namespace Persistity.Registries
{
    public interface IMappingRegistry
    {
        ITypeMapper TypeMapper { get; }

        TypeMapping GetMappingFor<T>() where T : new();
        TypeMapping GetMappingFor(Type type);
    }
}