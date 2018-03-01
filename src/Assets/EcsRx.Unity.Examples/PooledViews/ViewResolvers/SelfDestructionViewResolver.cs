using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Unity.Examples.PooledViews.Components;
using EcsRx.Unity.Systems;
using EcsRx.Views.Components;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Examples.PooledViews.ViewResolvers
{
    public class SelfDestructionViewResolver : UnityPooledViewResolverSystem
    {
        public override IGroup TargetGroup => new Group(typeof(SelfDestructComponent), typeof(ViewComponent));

        public SelfDestructionViewResolver(IInstantiator instantiator, IPoolManager poolManager) : base(instantiator, poolManager)
        {}

        protected override GameObject PrefabTemplate => Resources.Load("PooledPrefab") as GameObject;
        protected override int PoolIncrementSize => 5;

        protected override void OnPoolStarting()
        {
            ViewPool.PreAllocate(20);
        }

        protected override void OnViewAllocated(GameObject view, IEntity entity)
        {
            var selfDestructComponent = entity.GetComponent<SelfDestructComponent>();
            view.transform.position = selfDestructComponent.StartingPosition;
            Debug.Log("Created @ " + view.transform.position);
        }

        protected override void OnViewRecycled(GameObject view)
        {}
    }
}