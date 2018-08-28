using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Examples.PooledViews.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Unity.Systems;
using EcsRx.Views.Components;
using UnityEngine;
using Zenject;

namespace EcsRx.Examples.PooledViews.ViewResolvers
{
    public class SelfDestructionViewResolver : PooledPrefabViewResolverSystem
    {
        public override IGroup Group { get; } = new Group(typeof(SelfDestructComponent), typeof(ViewComponent));

        public SelfDestructionViewResolver(IInstantiator instantiator, IEntityCollectionManager collectionManager, IEventSystem eventSystem)
            : base(instantiator, collectionManager, eventSystem)
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