using System.Collections.Generic;
using System.Linq;
using EcsRx.Entities;
using EcsRx.Pools;
using EcsRx.Unity.Components;
using EcsRx.Unity.MonoBehaviours.Helpers;
using EcsRx.Unity.MonoBehaviours.Models;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public class RegisterAsEntity : MonoBehaviour
    {
        [Inject]
        public IPoolManager PoolManager { get; private set; }

        [SerializeField]
        public string PoolName;

        [SerializeField]
        public List<ComponentCache> ComponentCache = new List<ComponentCache>();

        [Inject]
        public void RegisterEntity()
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf) { return; }

            IPool poolToUse;

            if (string.IsNullOrEmpty(PoolName))
            { poolToUse = PoolManager.GetPool(); }
            else if (PoolManager.Pools.All(x => x.Name != PoolName))
            { poolToUse = PoolManager.CreatePool(PoolName); }
            else
            { poolToUse = PoolManager.GetPool(PoolName); }

            var createdEntity = poolToUse.CreateEntity();
            createdEntity.AddComponent(new ViewComponent { View = gameObject });
            SetupEntityBinding(createdEntity, poolToUse);
            SetupEntityComponents(createdEntity);

            Destroy(this);
        }

        private void SetupEntityBinding(IEntity entity, IPool pool)
        {
            var entityBinding = gameObject.AddComponent<EntityView>();
            entityBinding.Entity = entity;
            entityBinding.Pool = pool;
        }

        private void SetupEntityComponents(IEntity entity)
        {
            for (var i = 0; i < ComponentCache.Count; i++)
            {
                var component = EntityTransformer.DeserializeComponent(ComponentCache[i]);
                entity.AddComponent(component);
            }
        }
        
        public IPool GetPool()
        {
            return PoolManager.GetPool(PoolName);
        }
    }
}