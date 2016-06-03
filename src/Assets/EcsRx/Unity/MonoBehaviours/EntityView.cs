namespace EcsRx.Unity.Helpers
{
    using UnityEngine;
    using System.Collections;
    using EcsRx.Entities;
    using EcsRx.Pools;
    using EcsRx.Unity.Components;
    using Zenject;
    using System.Linq;

    public class EntityView : MonoBehaviour
    {
        [Inject]
        public IPoolManager PoolManager { get; private set; }

        public string PoolName { get; set; }
        public IEntity Entity { get; set; }

        public IPool Pool;

        [Inject]
        public void RegisterEntity()
        {
            if (string.IsNullOrEmpty(PoolName))
            { Pool = PoolManager.GetPool(); }
            else if (PoolManager.Pools.All(x => x.Name != PoolName))
            { Pool = PoolManager.CreatePool(PoolName); }
            else
            { Pool = PoolManager.GetPool(PoolName); }

            Entity = Pool.CreateEntity();
            Entity.AddComponent(new ViewComponent { View = gameObject });
        }
    }
}
