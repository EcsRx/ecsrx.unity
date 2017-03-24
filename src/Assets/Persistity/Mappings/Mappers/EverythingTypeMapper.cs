using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Persistity.Mappings.Mappers
{
    /// <summary>
    /// This type mapper doesnt care about attributes
    /// and will attempt to map everything and anything
    /// in a class, so use with caution
    /// </summary>
    public class EverythingTypeMapper : TypeMapper
    {
        public EverythingTypeMapper(MappingConfiguration configuration = null) : base(configuration)
        {}
    }
}