using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Unity.Examples.SceneFirstSetup.Components;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Examples.SceneFirstSetup.ViewResolvers
{
    public class SphereViewResolver : UnityViewResolverSystem
    {
        private readonly Transform ParentTrasform = GameObject.Find("Entities").transform;

        public override IGroup TargetGroup => base.TargetGroup.WithComponent<SphereComponent>();

        public SphereViewResolver(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator) : base(poolManager, eventSystem, instantiator)
        {}

        protected override GameObject PrefabTemplate => GameObject.CreatePrimitive(PrimitiveType.Sphere);

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.transform.position = new Vector3(2, 0, 0);
            view.transform.parent = ParentTrasform;
        }
    }
}
