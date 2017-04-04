using System;
using Persistity.Mappings.Types;

namespace Persistity.Mappings.Mappers
{
    public interface ITypeMapper
    {
        ITypeAnalyzer TypeAnalyzer { get; }
        TypeMapping GetTypeMappingsFor(Type type);       
    }
}