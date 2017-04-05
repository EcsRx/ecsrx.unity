using System.Linq;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Persistence.Data;
using EcsRx.Pools;

namespace EcsRx.Persistence.Transformers
{
    public class PoolDataTransformer : IPoolDataTransformer
    {
        public IEntityDataTransformer EntityTransformer { get; private set; }
        public IEventSystem EventSystem { get; private set; }
        public IEntityFactory EntityFactory { get; private set; }

        public PoolDataTransformer(IEntityDataTransformer entityTransformer, IEventSystem eventSystem, IEntityFactory entityFactory)
        {
            EntityTransformer = entityTransformer;
            EventSystem = eventSystem;
            EntityFactory = entityFactory;
        }

        public object TransformTo(object original)
        {
            var pool = (IPool)original;

            var entityData = pool.Entities
                .Select(EntityTransformer.TransformTo)
                .Cast<EntityData>()
                .ToList();

            return new PoolData
            {
                PoolName = pool.Name,
                Entities = entityData
            };
        }

        public object TransformFrom(object converted)
        {
            var poolData = (PoolData) converted;
            var pool = new Pool(poolData.PoolName, EntityFactory, EventSystem);
            var entities = poolData.Entities
                .Select(EntityTransformer.TransformFrom)
                .Cast<IEntity>()
                .ToList();

            foreach (var entity in entities)
            { pool.AddEntity(entity); }

            return pool;
        }
    }
}