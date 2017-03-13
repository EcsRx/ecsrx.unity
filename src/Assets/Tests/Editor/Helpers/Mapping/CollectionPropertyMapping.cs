using System;
using System.Collections.Generic;

namespace Tests.Editor.Helpers.Mapping
{
    public class CollectionPropertyMapping : Mapping
    {
        public Type ArrayType { get; set; }
        public Func<object, Array> GetValue { get; set; }
        public Action<object, object> SetValue { get; set; }
        public List<Mapping> InternalMappings { get; private set; }

        public CollectionPropertyMapping()
        { InternalMappings = new List<Mapping>(); }
    }
}