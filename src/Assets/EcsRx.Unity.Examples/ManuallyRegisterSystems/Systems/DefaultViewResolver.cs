using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Pools;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Examples.ManuallyRegisterSystems.Systems
{
    public class DefaultViewResolver : UnityViewResolverSystem
    {
        protected override GameObject PrefabTemplate => GameObject.CreatePrimitive(PrimitiveType.Cube);

        public DefaultViewResolver(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator) : base(poolManager, eventSystem, instantiator)
        {}

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.name = "entity-" + entity.Id;
        }
    }
}