using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Examples.SimpleMovement.ViewResolvers
{
    public class DefaultViewResolver : UnityViewResolverSystem
    {
        public DefaultViewResolver(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IInstantiator instantiator) : base(collectionManager, eventSystem, instantiator)
        {}
        
        protected override GameObject PrefabTemplate => GameObject.CreatePrimitive(PrimitiveType.Cube);

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.name = "entity-" + entity.Id;
            var rigidBody = view.AddComponent<Rigidbody>();
            rigidBody.freezeRotation = true;
        }
    }
}