using EcsRx.Entities;
using EcsRx.Pools;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace Assets.EcsRx.Examples.SimpleMovement.ViewResolvers
{
    public class DefaultViewResolver : ViewResolverSystem
    {
        public DefaultViewResolver(IPoolManager poolManager, IInstantiator instantiator) : base(poolManager, instantiator)
        {}

        public override GameObject ResolveView(IEntity entity)
        {
            var view = GameObject.CreatePrimitive(PrimitiveType.Cube);
            view.name = "entity-" + entity.Id;
            var rigidBody = view.AddComponent<Rigidbody>();
            rigidBody.freezeRotation = true;
            return view;
        }
    }
}