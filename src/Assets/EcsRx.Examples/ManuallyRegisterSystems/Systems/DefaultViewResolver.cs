using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace EcsRx.Examples.ManuallyRegisterSystems.Systems
{
    public class DefaultViewResolver : PrefabViewResolverSystem
    {
        protected override GameObject PrefabTemplate { get; } = Resources.Load<GameObject>("Cube");

        public DefaultViewResolver(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IUnityInstantiator instantiator) 
            : base(collectionManager, eventSystem, instantiator)
        {}

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.name = "entity-" + entity.Id;
        }
    }
}