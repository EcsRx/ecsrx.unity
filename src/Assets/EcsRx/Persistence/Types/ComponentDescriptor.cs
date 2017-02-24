using System;
using System.Collections.Generic;

namespace EcsRx.Persistence.Types
{
    public class ComponentDataDescriptor
    {
        public Type ComponentType { get; set; }
        public IDictionary<string, PropertyDataDescriptor> DataProperties { get; private set; }

        public ComponentDataDescriptor()
        {
            DataProperties = new Dictionary<string, PropertyDataDescriptor>();
        }
    }
}