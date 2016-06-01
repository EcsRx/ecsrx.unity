using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Pools.Identifiers;

namespace EcsRx.Pools
{
    public interface IPool
    {
        string Name { get; }

        IEnumerable<IEntity> Entities { get; }
        IIdentifyGenerator IdentityGenerator { get; }

        IEntity CreateEntity();
        void RemoveEntity(IEntity entity);
    }
}