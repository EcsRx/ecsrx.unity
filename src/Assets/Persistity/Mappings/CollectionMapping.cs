using System;
using System.Collections;
using System.Collections.Generic;

namespace Persistity.Mappings
{
    public class CollectionMapping : Mapping
    {
        public Type CollectionType { get; set; }
        public Func<object, IList> GetValue { get; set; }
        public Action<object, object> SetValue { get; set; }
        public IList<Mapping> InternalMappings { get; private set; }
        public bool IsArray { get; set; }
        public bool IsElementDynamicType { get; set; }

        public CollectionMapping()
        { InternalMappings = new List<Mapping>(); }
    }
}