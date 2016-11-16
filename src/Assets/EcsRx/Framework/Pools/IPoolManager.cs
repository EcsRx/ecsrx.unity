using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Pools.Identifiers;

namespace EcsRx.Pools
{
    public interface IPoolManager
    {
        IEnumerable<IPool> Pools { get; }
        IIdentityGenerator IdentityGenerator { get; }

        IEnumerable<IEntity> GetEntitiesFor(IGroup group, string poolName = null);
        IGroupAccessor CreateGroupAccessor(IGroup group, string poolName = null);

        IPool CreatePool(string name);
        IPool GetPool(string name = null);
        void RemovePool(string name);
    }
}