using System;
using System.Linq;
using EcsRx.Entities;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Database;
using EcsRx.Persistence.Transformers;
using Persistity;
using Persistity.Serialization;
using UnityEditor;

namespace EcsRx.Persistence.Repositories
{
    public class ApplicationRepository
    {
        public ApplicationDatabase ApplicationDatabase { get; private set; }
        public ISerializer Serializer { get; private set; }
        public IDeserializer Deserializer { get; private set; }
        public IEntityDataTransformer Transformer { get; private set; }

        public ApplicationRepository(ApplicationDatabase applicationDatabase, ISerializer serializer, IDeserializer deserializer, IEntityDataTransformer transformer)
        {
            ApplicationDatabase = applicationDatabase;
            Serializer = serializer;
            Deserializer = deserializer;
            Transformer = transformer;
        }
        /*
        public bool HasEntity(Guid entityId)
        { return ApplicationDatabase.Pools.Any(x => x.EntityId == entityId); }

        public IEntity Retrieve(Guid entityId)
        {
            var entityStateData = ApplicationDatabase.EntityData.SingleOrDefault(x => x.EntityId == entityId);
            if(entityStateData == null) { return null; }

            var dataObject = new DataObject(entityStateData.EntityData);
            var entityData = Deserializer.Deserialize<EntityData>(dataObject);

            return (IEntity)Transformer.TransformFrom(entityData);
        }

        public void Update(IEntity entity)
        {
            if (!HasEntity(entity.Id))
            { Create(entity); }

            var entityData = (EntityData)Transformer.TransformTo(entity);
            var entityStateData = Serializer.Serialize(entityData);

            var currentEntityLink = ApplicationDatabase.EntityData.Single(x => x.EntityId == entity.Id);
            currentEntityLink.EntityData = entityStateData.AsBytes;
        }


        public void Delete(IEntity entity)
        {
            if (!HasEntity(entity.Id))
            { return; }

            var currentEntityLink = ApplicationDatabase.EntityData.Single(x => x.EntityId == entity.Id);
            ApplicationDatabase.EntityData.Remove(currentEntityLink);
        }

        public void Create(IEntity entity)
        {
            var entityData = (EntityData)Transformer.TransformTo(entity);
            var entityStateData = Serializer.Serialize(entityData);

            var entityLink = new ApplicationEntityLink { EntityId = entity.Id, EntityData = entityStateData.AsBytes };
            ApplicationDatabase.EntityData.Add(entityLink);
        }
        */
    }
}