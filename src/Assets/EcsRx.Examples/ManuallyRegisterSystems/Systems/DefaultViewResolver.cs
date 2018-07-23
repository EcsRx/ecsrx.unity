using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace EcsRx.Examples.ManuallyRegisterSystems.Systems
{
    public class DefaultViewResolver : UnityViewResolverSystem
    {
        protected override GameObject PrefabTemplate => GameObject.CreatePrimitive(PrimitiveType.Cube);

        public DefaultViewResolver(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IInstantiator instantiator) : base(collectionManager, eventSystem, instantiator)
        {}

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.name = "entity-" + entity.Id;
        }
    }
}