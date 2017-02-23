using System;
using System.Collections.Generic;

namespace EcsRx.Persistence.Data
{
    public class EntityData
    {
        public Guid EntityId { get; set; }
        public IDictionary<string, object> Data { get; set; }

        public EntityData()
        {
            Data = new Dictionary<string, object>();
        }
    }
}