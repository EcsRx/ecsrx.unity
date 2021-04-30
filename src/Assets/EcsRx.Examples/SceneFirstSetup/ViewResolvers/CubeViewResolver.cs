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
    public class CubeViewResolver : PrefabViewResolverSystem
    {
        private readonly Transform _parentTransform = GameObject.Find("Entities").transform;

        public override IGroup Group => base.Group.WithComponent<CubeComponent>();

        public CubeViewResolver(IEntityDatabase entityDatabase, IEventSystem eventSystem, IUnityInstantiator instantiator)
            : base(entityDatabase, eventSystem, instantiator)
        {}

        protected override GameObject PrefabTemplate { get; } = Resources.Load<GameObject>("Cube");
        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.transform.position = new Vector3(-2, 0, 0);
            view.transform.parent = _parentTransform;
        }
    }
}
