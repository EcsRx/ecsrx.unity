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
        public EverythingTypeMapper(IEnumerable<Type> knownPrimitives = null) : base(knownPrimitives)
        {
        }

        public override IEnumerable<PropertyInfo> GetPropertiesFor(Type type)
        {
            return base.GetPropertiesFor(type)
                .Where(x => x.GetSetMethod().IsPublic &&
                            x.GetGetMethod().IsPublic);
        }
    }
}