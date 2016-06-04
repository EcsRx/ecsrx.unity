using System.Collections.Generic;
using Assets.EcsRx.Framework.Blueprints;
using EcsRx.Entities;
using EcsRx.Pools.Identifiers;

namespace EcsRx.Pools
{
    public interface IPool
    {
        string Name { get; }

        IEnumerable<IEntity> Entities { get; }
        IIdentifyGenerator IdentityGenerator { get; }

        IEntity CreateEntity(IBlueprint blueprint = null);
        void RemoveEntity(IEntity entity);
    }
}
