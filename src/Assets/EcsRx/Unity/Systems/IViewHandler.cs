using System;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Pools;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Systems
{
    public interface IViewHandler
    {
        IPoolManager PoolManager { get; }
        IEventSystem EventSystem { get; }
        IInstantiator Instantiator { get; }

        GameObject InstantiateAndInject(GameObject prefab,
            Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion));

        void DestroyView(GameObject view);
        void SetupView(IEntity entity, Func<IEntity, GameObject> viewResolver);
    }
}