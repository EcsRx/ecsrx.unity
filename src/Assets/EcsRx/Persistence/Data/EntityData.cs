using System;
using System.Collections.Generic;
using EcsRx.Components;

namespace EcsRx.Persistence.Data
{
    public class EntityData
    {
        public Guid EntityId { get; set; }
        public IList<IComponent> Components { get; set; }

        public EntityData()
        {
            Components = new List<IComponent>();
        }
    }
}