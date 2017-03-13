using System;
using System.Collections.Generic;

namespace Tests.Editor.Helpers.Mapping
{
    public class CollectionPropertyMapping : Mapping
    {
        public Func<object, Array> GetValue { get; set; }
        public List<Mapping> InternalMappings { get; private set; }

        public CollectionPropertyMapping()
        { InternalMappings = new List<Mapping>(); }
    }
}