using System;
using EcsRx.Components;
using EcsRx.Entities;

namespace EcsRx.Events
{
    public class EntityComponentEvent : EventArgs
    {
        public IEntity Entity { get; set; }
        public IComponent Component { get; set; }

        public EntityComponentEvent(IEntity entity, IComponent component)
        {
            Entity = entity;
            Component = component;
        }
    }
}