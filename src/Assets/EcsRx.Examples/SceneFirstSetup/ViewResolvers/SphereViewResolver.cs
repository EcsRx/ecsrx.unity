using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Examples.SceneFirstSetup.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace EcsRx.Examples.SceneFirstSetup.ViewResolvers
{
    public class SphereViewResolver : PrefabViewResolverSystem
    {
        private readonly Transform ParentTrasform = GameObject.Find("Entities").transform;

        public override IGroup Group => base.Group.WithComponent<SphereComponent>();

        public SphereViewResolver(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IUnityInstantiator instantiator)
            : base(collectionManager, eventSystem, instantiator)
        {}

        protected override GameObject PrefabTemplate { get; } = Resources.Load<GameObject>("Sphere");

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.transform.position = new Vector3(2, 0, 0);
            view.transform.parent = ParentTrasform;
        }
    }
}
