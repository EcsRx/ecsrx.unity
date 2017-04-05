using System.Linq;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Persistence.Data;

namespace EcsRx.Persistence.Transformers
{
    public class EntityDataTransformer : IEntityDataTransformer
    {
        public IEventSystem EventSystem { get; private set; }

        public EntityDataTransformer(IEventSystem eventSystem)
        {
            EventSystem = eventSystem;
        }
        
        public object TransformTo(object original)
        {
            var entity = (IEntity)original;
            return new EntityData
            {
                EntityId = entity.Id,
                Components = entity.Components.ToList()
            };
        }

        public object TransformFrom(object converted)
        {
            var entityData = (EntityData) converted;
            var entity = new Entity(entityData.EntityId, EventSystem);
            foreach (var component in entityData.Components)
            { entity.AddComponent(component); }
            return entity;
        }
    }
}