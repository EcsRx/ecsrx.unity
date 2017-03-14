using System;
using System.Collections;
using System.Collections.Generic;

namespace Tests.Editor.Helpers.Mapping
{
    public class CollectionPropertyMapping : Mapping
    {
        public Type CollectionType { get; set; }
        public Func<object, IList> GetValue { get; set; }
        public Action<object, object> SetValue { get; set; }
        public List<Mapping> InternalMappings { get; private set; }
        public bool IsArray { get; set; }

        public CollectionPropertyMapping()
        { InternalMappings = new List<Mapping>(); }
    }
}