﻿using System.Collections.Generic;
using EcsRx.Components;

namespace EcsRx.UnityEditor.Data
{
    public class EntityData
    {
        public int EntityId { get; set; }
        public List<IComponent> Components { get; set; }

        public EntityData()
        {
            Components = new List<IComponent>();
        }
    }
}