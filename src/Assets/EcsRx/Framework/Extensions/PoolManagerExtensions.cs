using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Entities;
using EcsRx.Pools;

namespace EcsRx.Extensions
{
    public static class PoolManagerExtensions
    {
        public static IEnumerable<IEntity> GetAllEntities(this IEnumerable<IPool> pools)
        {
            return pools.SelectMany(x => x.Entities);
        }

        public static IPool GetContainingPoolFor(this IPoolManager poolManager, IEntity entity)
        {
            return poolManager.Pools.SingleOrDefault(x => x.Entities.Contains(entity));
        }

        public static void RemoveEntitiesContaining(this IPoolManager poolManager, params Type[] components)
        {
            foreach (var pool in poolManager.Pools)
            { pool.RemoveEntitiesContaining(components); }
        }

        public static void RemoveEntity(this IPoolManager poolManager, IEntity entity)
        {
            var containingPool = poolManager.GetContainingPoolFor(entity);
            containingPool.RemoveEntity(entity);
        }
    }
}