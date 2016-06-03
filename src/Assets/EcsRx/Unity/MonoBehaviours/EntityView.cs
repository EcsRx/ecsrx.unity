namespace EcsRx.Unity.Helpers
{
    using UnityEngine;
    using System.Collections;
    using EcsRx.Entities;
    using EcsRx.Pools;
    using EcsRx.Unity.Components;
    using Zenject;
    using System.Linq;
    using System.Collections.Generic;
    using EcsRx.Components;

    [System.Serializable]
    public class EntityView : MonoBehaviour
    {
        public int g = 3;
        [Inject]
        public IPoolManager PoolManager { get; private set; }

        public string PoolName { get; set; }
        public IEntity Entity;

        public IPool Pool;

        public List<IComponent> StagedComponents = new List<IComponent>();

        [Inject]
        public void RegisterEntity()
        {
            if (string.IsNullOrEmpty(PoolName))
            { Pool = PoolManager.GetPool(); }
            else if (PoolManager.Pools.All(x => x.Name != PoolName))
            { Pool = PoolManager.CreatePool(PoolName); }
            else
            { Pool = PoolManager.GetPool(PoolName); }

            if (Entity == null)
            {
                Entity = Pool.CreateEntity();
                Debug.Log("entity created");
            }
            else
            {
                Pool.AddEntity(Entity);
                Debug.Log("entity added");
            }

            Entity.AddComponent(new ViewComponent { View = gameObject });

            Debug.Log(StagedComponents.Count() + " staged components");
            for (int i = 0; i < StagedComponents.Count(); i++)
            {
                Entity.AddComponent(StagedComponents[i]);
            }
        }
    }
}
