using SystemsRx.Events;
using EcsRx.Collections.Database;
using EcsRx.Entities;
using EcsRx.Examples.SceneFirstSetup.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Systems;
using UnityEngine;

namespace EcsRx.Examples.SceneFirstSetup.ViewResolvers
{
    public class SphereViewResolver : PrefabViewResolverSystem
    {
        private readonly Transform ParentTrasform = GameObject.Find("Entities").transform;

        public override IGroup Group => base.Group.WithComponent<SphereComponent>();

        public SphereViewResolver(IEntityDatabase entityDatabase, IEventSystem eventSystem, IUnityInstantiator instantiator)
            : base(entityDatabase, eventSystem, instantiator)
        {}

        protected override GameObject PrefabTemplate { get; } = Resources.Load<GameObject>("Sphere");

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.transform.position = new Vector3(2, 0, 0);
            view.transform.parent = ParentTrasform;
        }
    }
}
