using System;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;

namespace Assets.EcsRx.Examples.PooledViews.Systems
{
    public class SelfDestructionSystem : IReactToEntitySystem
    {
        public Group TargetGroup { get { return new Group(typeof(SelfDestructComponent), typeof(ViewComponent));} }
        private readonly IPool _defaultPool;

        public SelfDestructionSystem(IPoolManager poolManager)
        { _defaultPool = poolManager.GetPool(); }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            var selfDestructComponent = entity.GetComponent<SelfDestructComponent>();
            return Observable.Interval(TimeSpan.FromSeconds(selfDestructComponent.Lifetime)).First().Select(x => entity);
        }

        public void Execute(IEntity entity)
        { _defaultPool.RemoveEntity(entity); }
    }
}