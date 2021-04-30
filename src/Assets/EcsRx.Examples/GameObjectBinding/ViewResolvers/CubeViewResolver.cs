using SystemsRx.Events;
using EcsRx.Collections.Database;
using EcsRx.Entities;
using EcsRx.Examples.GameObjectBinding.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Systems;
using UnityEngine;

namespace EcsRx.Examples.GameObjectBinding.ViewResolvers
{
    public class CubeViewResolver : PrefabViewResolverSystem
    {
        public override IGroup Group => base.Group.WithComponent<CubeComponent>();

        protected override GameObject PrefabTemplate { get; } = Resources.Load<GameObject>("Cube");

        public CubeViewResolver(IEntityDatabase entityDatabase, IEventSystem eventSystem, IUnityInstantiator instantiator) 
            : base(entityDatabase, eventSystem, instantiator)
        {}

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.transform.position = new Vector3(-2, 0, 0);
        }
    }
}