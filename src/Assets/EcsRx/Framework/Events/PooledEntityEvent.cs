using System;
using EcsRx.Entities;

namespace EcsRx.Events
{
    public class PooledEntityEvent : EventArgs
    {
        public string PoolName { get; set; }
        public IEntity Entity { get; set; }

        public PooledEntityEvent(string poolName, IEntity entity)
        {
            PoolName = poolName;
            Entity = entity;
        }
    }
}