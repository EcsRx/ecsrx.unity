using System;
using System.Collections.Generic;

namespace Persistity.Mappings
{
    public class TypeMapping
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public IList<Mapping> InternalMappings { get; private set; }

        public TypeMapping()
        {
            InternalMappings = new List<Mapping>();
        }
    }
}