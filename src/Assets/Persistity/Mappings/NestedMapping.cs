using System;
using System.Collections.Generic;

namespace Persistity.Mappings
{
    public class NestedMapping : Mapping
    {
        public Func<object, object> GetValue { get; set; }
        public Action<object, object> SetValue { get; set; }
        public List<Mapping> InternalMappings { get; private set; }

        public NestedMapping()
        { InternalMappings = new List<Mapping>(); }
    }
}