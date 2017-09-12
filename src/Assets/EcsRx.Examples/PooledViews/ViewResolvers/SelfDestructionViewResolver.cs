using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Examples.PooledViews.Components;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Unity.Components;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace Assets.EcsRx.Examples.PooledViews.ViewResolvers
{
    public class SelfDestructionViewResolver : DefaultPooledViewResolverSystem
    {
        public override IGroup TargetGroup
        {
            get { return new Group(typeof(SelfDestructComponent), typeof(ViewComponent)); }
        }

        public SelfDestructionViewResolver(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator)
            : base(poolManager, eventSystem, instantiator)
        {
            ViewPool.PreAllocate(20);
        }

        protected override GameObject ResolvePrefabTemplate()
        { return Resources.Load("PooledPrefab") as GameObject; }

        protected override GameObject AllocateView(IEntity entity, IPool pool)
        {
            var selfDestructComponent = entity.GetComponent<SelfDestructComponent>();
            var allocatedView = base.AllocateView(entity, pool);
            allocatedView.transform.position = selfDestructComponent.StartingPosition;
            Debug.Log("Created @ " + allocatedView.transform.position);
            return allocatedView;
        }
    }
}