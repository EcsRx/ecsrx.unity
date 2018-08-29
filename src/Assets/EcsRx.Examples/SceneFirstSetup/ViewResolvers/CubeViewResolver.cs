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
    public class CubeViewResolver : PrefabViewResolverSystem
    {
        private readonly Transform _parentTransform = GameObject.Find("Entities").transform;

        public override IGroup Group => base.Group.WithComponent<CubeComponent>();

        public CubeViewResolver(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IUnityInstantiator instantiator)
            : base(collectionManager, eventSystem, instantiator)
        {}

        protected override GameObject PrefabTemplate { get; } = Resources.Load<GameObject>("Cube");
        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.transform.position = new Vector3(-2, 0, 0);
            view.transform.parent = _parentTransform;
        }
    }
}
