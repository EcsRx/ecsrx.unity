using System;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Unity.Examples.PooledViews.Blueprints;
using EcsRx.Unity.Examples.PooledViews.Components;
using EcsRx.Views.Components;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Examples.PooledViews.Systems
{
    public class SpawnSystem : IReactToEntitySystem
    {
        private readonly IPool _defaultPool;

        public IGroup TargetGroup { get { return new Group(typeof(SpawnerComponent), typeof(ViewComponent));} }

        public SpawnSystem(IPoolManager poolManager)
        { _defaultPool = poolManager.GetPool(); }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            var spawnComponent = entity.GetComponent<SpawnerComponent>();
            return Observable.Interval(TimeSpan.FromSeconds(spawnComponent.SpawnRate)).Select(x => entity);
        }

        public void Execute(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var view = viewComponent.View as GameObject;
            var blueprint = new SelfDestructBlueprint(view.transform.position);
            _defaultPool.CreateEntity(blueprint);
        }
    }
}