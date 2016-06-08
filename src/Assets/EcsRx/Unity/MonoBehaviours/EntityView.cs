using UnityEngine;
using EcsRx.Entities;
using EcsRx.Pools;
using EcsRx.Unity.Components;
using Zenject;
using System.Linq;
using System.Collections.Generic;
using EcsRx.Components;
using System;
using EcsRx.Json;

namespace EcsRx.Unity.Helpers
{
    public class EntityView : MonoBehaviour
    {
        [Inject]
        public IPoolManager PoolManager { get; private set; }

        public string PoolName { get; set; }
        public IEntity Entity;

        public IPool Pool;

		[SerializeField]
        public List<string> Components = new List<string>();

		[SerializeField]
        public List<string> Properties = new List<string>();

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
            for (int i = 0; i < Components.Count(); i++)
            {
                var type = Type.GetType(Components[i]);
                var component = (IComponent)Activator.CreateInstance(type);

								var node = JSON.Parse(Properties[i]);
								component.DeserializeComponent(node);
                Entity.AddComponent(component);
            }
        }
    }
}
