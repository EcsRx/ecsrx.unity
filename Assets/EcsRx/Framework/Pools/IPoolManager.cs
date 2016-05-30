using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.EventHandlers;
using EcsRx.Groups;
using EcsRx.Pools.Identifiers;

namespace EcsRx.Pools
{
    public interface IPoolManager
    {
        event PooledEntityHandler OnEntityAdded;
        event PooledEntityHandler OnEntityRemoved;
        
        event EntityComponentHandler OnEntityComponentAdded;
        event EntityComponentHandler OnEntityComponentRemoved;

        IEnumerable<IPool> Pools { get; }
        IIdentifyGenerator IdentityGenerator { get; }

        IEnumerable<IEntity> GetEntitiesFor(IGroup group, string poolName = null);
        GroupAccessor CreateGroupAccessor(IGroup group, string poolName = null);

        IPool CreatePool(string name);
        IPool GetPool(string name = null);
        void RemovePool(string name);
    }
}