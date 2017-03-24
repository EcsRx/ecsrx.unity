using System;
using System.Collections.Generic;

namespace Persistity.Mappings.Mappers
{
    public class MappingConfiguration
    {
        public IEnumerable<Type> KnownPrimitives { get; set; }
        public IEnumerable<Type> IgnoredTypes { get; set; }

        public static MappingConfiguration Default
        {
            get
            {
                return new MappingConfiguration
                {
                    KnownPrimitives = new Type[0],
                    IgnoredTypes = new Type[0]
                };
            }
        }
    }
}