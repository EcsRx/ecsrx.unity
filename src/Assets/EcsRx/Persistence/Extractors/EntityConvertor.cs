using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Persistence.Data;
using EcsRx.Unity.MonoBehaviours.Helpers;
using Persistity.Transformers;

namespace EcsRx.Persistence.Extractors
{
    public class EntityConvertor : IEntityConvertor
    {
        public ITransformer Transformer { get; private set; }
        public IEventSystem EventSystem { get; private set; }

        public EntityConvertor(ITransformer transformer, IEventSystem eventSystem)
        {
            Transformer = transformer;
            EventSystem = eventSystem;
        }

        public EntityData ConvertToData(IEntity entity)
        {
            var entityData = new EntityData { EntityId = entity.Id };
            foreach (var component in entity.Components)
            {
                var componentData = new ComponentData();
                var componentType = component.GetType();

                componentData.ComponentTypeReference = componentType.FullName;
                componentData.ComponentState = Transformer.Transform(componentType, component);
                entityData.ComponentData.Add(componentData);
            }
            return entityData;
        }

        public IEntity ConvertFromData(EntityData entityData)
        {
            var entity = new Entity(entityData.EntityId, EventSystem);
            foreach (var componentData in entityData.ComponentData)
            {
                var componentType = TypeHelper.GetTypeWithAssembly(componentData.ComponentTypeReference);
                var component = (IComponent)Transformer.Transform(componentType, componentData.ComponentState);
                entity.AddComponent(component);
            }
            return entity;
        }
    }
}