using System;
using System.Collections.Generic;

namespace Tests.Editor.Helpers.Mapping
{
    public class NestedMapping : Mapping
    {
        public Func<object, object> GetValue { get; set; }
        public List<Mapping> InternalMappings { get; private set; }

        public NestedMapping()
        { InternalMappings = new List<Mapping>(); }
    }
}