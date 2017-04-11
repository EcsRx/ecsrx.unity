using System;
using System.Linq;
using EcsRx.Entities;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Transformers;
using EcsRx.Pools;
using EcsRx.Unity.Components;
using Persistity;
using Persistity.Serialization.Binary;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public class RegisterAsEntity : MonoBehaviour
    {
        [Inject]
        public IPoolManager PoolManager { get; private set; }

        [Inject]
        public IBinarySerializer Serializer;

        [Inject]
        public IBinaryDeserializer Deserializer;

        [Inject]
        public IEntityDataTransformer Transformer;
        
        [SerializeField]
        public string PoolName;

        [SerializeField]
        public Guid EntityId = Guid.NewGuid();

        [SerializeField]
        private byte[] EntityState;

        public bool HasDeserialized = false;
        public IEntity Entity = new Entity(Guid.Empty, null);
        public EntityData EntityData = new EntityData();

        [Inject]
        public void RegisterEntity()
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf) { return; }

            var poolToUse = GetPool();
            var entity = (IEntity)Transformer.TransformFrom(EntityData);

            poolToUse.AddEntity(entity);

            if (!entity.HasComponent<ViewComponent>())
            { entity.AddComponent(new ViewComponent { View = gameObject }); }

            SetupEntityBinding(entity, poolToUse);
            Destroy(this);
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
            if(Deserializer == null) { return; }

            HasDeserialized = true;
            if (EntityState == null || EntityState.Length == 0) { return; }
            var data = new DataObject(EntityState);
            EntityData = Deserializer.Deserialize<EntityData>(data);
            EntityId = EntityData.EntityId;
        }

        private void SetupEntityBinding(IEntity entity, IPool pool)
        {
            var entityBinding = gameObject.AddComponent<EntityView>();
            entityBinding.Entity = entity;
            entityBinding.Pool = pool;
        }
        
        public IPool GetPool()
        {
            if (string.IsNullOrEmpty(PoolName))
            { return PoolManager.GetPool(); }

            if (PoolManager.Pools.All(x => x.Name != PoolName))
            { return PoolManager.CreatePool(PoolName); }

            return PoolManager.GetPool(PoolName);
        }
    }
}