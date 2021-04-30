using SystemsRx.Events;
using EcsRx.Collections.Database;
using EcsRx.Entities;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Systems;
using UnityEngine;

namespace EcsRx.Examples.SimpleMovement.ViewResolvers
{
    public class DefaultViewResolver : PrefabViewResolverSystem
    {
        public DefaultViewResolver(IEntityDatabase entityDatabase, IEventSystem eventSystem, IUnityInstantiator instantiator)
            : base(entityDatabase, eventSystem, instantiator)
        {}
        
        protected override GameObject PrefabTemplate {get;} = Resources.Load<GameObject>("Cube");

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.name = $"entity-{entity.Id}";
            var rigidBody = view.AddComponent<Rigidbody>();
            rigidBody.freezeRotation = true;
        }
    }
}