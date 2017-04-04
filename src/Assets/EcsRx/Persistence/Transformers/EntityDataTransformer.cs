using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Persistence.Data;
using Persistity;
using Persistity.Serialization;

namespace EcsRx.Persistence.Transformers
{
    public class EntityDataTransformer : IEntityDataTransformer
    {
        public ISerializer Serializer { get; private set; }
        public IDeserializer Deserializer { get; private set; }
        public IEventSystem EventSystem { get; private set; }

        public EntityDataTransformer(ISerializer serializer, IDeserializer deserializer, IEventSystem eventSystem)
        {
            Serializer = serializer;
            Deserializer = deserializer;
            EventSystem = eventSystem;
        }
        
        public object TransformTo(object original)
        {
            var entity = (IEntity)original;
            var entityData = new EntityData { EntityId = entity.Id };
            foreach (var component in entity.Components)
            {
                var componentData = new ComponentData();
                var componentType = component.GetType();

                var serializedData = Serializer.Serialize(component);
                componentData.ComponentState = new StateData(serializedData.AsString);
                componentData.ComponentName = componentType.FullName;
                entityData.ComponentData.Add(componentData);
            }
            return entityData;
        }

        public object TransformFrom(object converted)
        {
            var entityData = (EntityData) converted;
            var entity = new Entity(entityData.EntityId, EventSystem);
            foreach (var componentData in entityData.ComponentData)
            {
                var dataObject = new DataObject(componentData.ComponentState.State);
                var component = (IComponent)Deserializer.Deserialize(dataObject);
                entity.AddComponent(component);
            }
            return entity;
        }
    }
}