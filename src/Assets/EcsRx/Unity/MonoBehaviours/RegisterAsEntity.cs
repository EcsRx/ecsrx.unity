using System.Linq;
using Assets.Examples.AutoRegisterSystems.Components;
using EcsRx.Entities;
using EcsRx.Pools;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public class RegisterAsEntity : MonoBehaviour
    {
        [Inject]
        public IPoolManager PoolManager { get; private set; }

        public string PoolName;

        [Inject]
        public void RegisterEntity()
        {
            IPool poolToUse;

            if (string.IsNullOrEmpty(PoolName))
            { poolToUse = PoolManager.GetPool(); }
            else if (PoolManager.Pools.All(x => x.Name != PoolName))
            { poolToUse = PoolManager.CreatePool(PoolName); }
            else
            { poolToUse = PoolManager.GetPool(PoolName); }

            var createdEntity = poolToUse.CreateEntity();
            createdEntity.AddComponent(new ViewComponent { GameObject = gameObject });
            SetupEntityBinding(createdEntity);
        }

        private void SetupEntityBinding(IEntity entity)
        {
            var entityBinding = gameObject.AddComponent<EntityBinding>();
            entityBinding.Entity = entity;
            entityBinding.PoolName = PoolName;
            Destroy(this);
        }
    }
}