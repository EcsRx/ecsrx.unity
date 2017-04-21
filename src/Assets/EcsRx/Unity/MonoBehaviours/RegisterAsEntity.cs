using System;
using System.Linq;
using EcsRx.Entities;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Transformers;
using EcsRx.Pools;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public class RegisterAsEntity : MonoBehaviour
    {
        [Inject]
        public IPoolManager PoolManager { get; private set; }

        [Inject]
        public IEntityDataTransformer Transformer;
        
        [SerializeField]
        public string PoolName;

        [SerializeField]
        public Guid EntityId = Guid.NewGuid();

        /*
        [Inject]
        public void RegisterEntity()
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf) { return; }

            DeserializeState();
            var poolToUse = GetPool();           
            var entity = (IEntity)Transformer.TransformFrom(EntityData);

            poolToUse.AddEntity(entity);

            if (!entity.HasComponent<ViewComponent>())
            { entity.AddComponent(new ViewComponent { View = gameObject }); }

            SetupEntityBinding(entity, poolToUse);
            Destroy(this);
        }

        public void DeserializeState()
        {
            if(Deserializer == null) { return; }

            //HasDeserialized = true;
            //if (EntityState == null || EntityState.Length == 0) { return; }
            var data = new DataObject(EntityState);
            Deserializer.DeserializeInto(data, EntityData);
            EntityId = EntityData.EntityId;
        }
        */
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