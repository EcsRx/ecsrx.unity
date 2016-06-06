namespace EcsRx.Unity.Helpers
{
    using UnityEngine;
    using EcsRx.Entities;
    using EcsRx.Pools;
    using EcsRx.Unity.Components;
    using Zenject;
    using System.Linq;
    using System.Collections.Generic;
    using EcsRx.Components;
    using System;

    public class EntityView : MonoBehaviour
    {
        [Inject]
        public IPoolManager PoolManager { get; private set; }

        public string PoolName { get; set; }
        public IEntity Entity;

        public IPool Pool;

        public List<string> StagedComponents = new List<string>();

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
            for (int i = 0; i < StagedComponents.Count(); i++)
            {
                var type = Type.GetType(StagedComponents[i]);
                var component = (IComponent)Activator.CreateInstance(type);
                Entity.AddComponent(component);
            }
        }
    }
}
