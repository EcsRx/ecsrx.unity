using System;
using EcsRx.Entities;

namespace EcsRx.Events
{
    public class EntityEvent : EventArgs
    {
        public IEntity Entity { get; set; }

        public EntityEvent(IEntity entity)
        {
            Entity = entity;
        }
    }
}