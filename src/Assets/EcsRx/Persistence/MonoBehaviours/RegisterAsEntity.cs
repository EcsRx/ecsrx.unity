using System.Linq;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Persistence.Data;
using EcsRx.Views.Components;
using LazyData;
using LazyData.Serialization.Binary;
using UnityEngine;
using Zenject;

namespace EcsRx.Persistence.MonoBehaviours
{
    public class RegisterAsEntity : MonoBehaviour
    {
        [Inject]
        public IEntityCollectionManager CollectionManager { get; private set; }

        [Inject]
        public IBinarySerializer Serializer;
        
        [Inject]
        public IBinaryDeserializer Deserializer;
        
        [SerializeField]
        public string CollectionName;

        [SerializeField]
        public int EntityId;
        
        [SerializeField]
        public byte[] EntityState;

        public bool HasDeserialized = false;
        public EntityData EntityData = new EntityData();
        
        [Inject]
        public void RegisterEntity()
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf) { return; }

            DeserializeState();

            var collectionToUse = GetCollectionManager();
            var createdEntity = collectionToUse.CreateEntity();
            createdEntity.AddComponents(new ViewComponent { View = gameObject });
            SetupEntityBinding(createdEntity, collectionToUse);

            Destroy(this);
        }
        
        private IEntityCollection GetCollectionManager()
        {
            if (string.IsNullOrEmpty(CollectionName))
            { return CollectionManager.GetCollection(); }

            if (CollectionManager.Collections.All(x => x.Name != CollectionName))
            { return CollectionManager.CreateCollection(CollectionName); }

            return CollectionManager.GetCollection(CollectionName);
        }
        
        public void SerializeState()
        {
            if (Serializer == null) { return; }
            EntityData.EntityId = EntityId;
            var data = Serializer.Serialize(EntityData);
            EntityState = data.AsBytes;
        }

        public void DeserializeState()
        {
            if (Deserializer == null) { return; }

            HasDeserialized = true;
            if (EntityState == null || EntityState.Length == 0) 
            { return; }
            
            var data = new DataObject(EntityState);
            Deserializer.DeserializeInto(data, EntityData);
            EntityId = EntityData.EntityId;
        }

        private void SetupEntityBinding(IEntity entity, IEntityCollection entityCollection)
        {
            var entityBinding = gameObject.AddComponent<EntityView>();
            entityBinding.Entity = entity;
            entityBinding.EntityCollection = entityCollection;
        }
        
        public IEntityCollection GetCollection()
        {
            return CollectionManager.GetCollection(CollectionName);
        }
    }
}