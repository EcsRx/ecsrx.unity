using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.EventHandlers;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Pools.Identifiers;

namespace EcsRx.Pools
{
    public interface IPool
    {
        event EntityHandler OnEntityAdded;
        event EntityHandler OnEntityRemoved;

        event EntityComponentHandler OnEntityComponentAdded;
        event EntityComponentHandler OnEntityComponentRemoved;

        string Name { get; }

        IEnumerable<IEntity> Entities { get; }
        IIdentifyGenerator IdentityGenerator { get; }

        IEntity CreateEntity();
        void RemoveEntity(IEntity entity);
    }
}