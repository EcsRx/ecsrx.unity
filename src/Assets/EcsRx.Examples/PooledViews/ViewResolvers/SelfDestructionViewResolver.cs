using SystemsRx.Events;
using EcsRx.Collections;
using EcsRx.Collections.Database;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Examples.PooledViews.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Systems;
using EcsRx.Plugins.Views.Components;
using EcsRx.Plugins.Views.Pooling;
using UnityEngine;

namespace EcsRx.Examples.PooledViews.ViewResolvers
{
    public class SelfDestructionViewResolver : PooledPrefabViewResolverSystem
    {
        public override IGroup Group { get; } = new Group(typeof(SelfDestructComponent), typeof(ViewComponent));

        public SelfDestructionViewResolver(IUnityInstantiator instantiator, IEntityDatabase entityDatabase, IEventSystem eventSystem)
            : base(instantiator, entityDatabase, eventSystem)
        {}

        protected override GameObject PrefabTemplate { get; } = Resources.Load("PooledPrefab") as GameObject;
        protected override int PoolIncrementSize => 5;

        protected override void OnPoolStarting()
        { ViewPool.PreAllocate(30); }

        protected override void OnViewAllocated(GameObject view, IEntity entity)
        {
            view.name = $"pooled-active-{entity.Id}";
            
            var selfDestructComponent = entity.GetComponent<SelfDestructComponent>();
            view.transform.position = selfDestructComponent.StartingPosition;
        }

        protected override void OnViewRecycled(GameObject view, IEntity entity)
        {
            view.name = "pooled-inactive";
        }
    }
}