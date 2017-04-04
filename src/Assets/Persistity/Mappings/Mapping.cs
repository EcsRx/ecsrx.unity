using System;
using System.Collections.Generic;

namespace Persistity.Mappings
{
    public class Mapping
    {
        public string LocalName { get; set; }
        public string ScopedName { get; set; }
        public Type Type { get; set; }
        public IDictionary<string, object> MetaData { get; private set; }

        public Mapping()
        {
            MetaData = new Dictionary<string, object>();
        }
    }
}