using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Examples.GameObjectBinding.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace EcsRx.Examples.GameObjectBinding.ViewResolvers
{
    public class SphereViewResolver : UnityViewResolverSystem
    {
        public override IGroup Group => base.Group.WithComponent<SphereComponent>();
        protected override GameObject PrefabTemplate => GameObject.CreatePrimitive(PrimitiveType.Sphere);

        public SphereViewResolver(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IInstantiator instantiator) : base(collectionManager, eventSystem, instantiator)
        {
        }

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.transform.position = new Vector3(2,0,0);
        }
    }
}