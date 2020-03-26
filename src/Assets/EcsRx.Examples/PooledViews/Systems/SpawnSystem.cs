using System;
using EcsRx.Collections;
using EcsRx.Collections.Database;
using EcsRx.Collections.Entity;
using EcsRx.Entities;
using EcsRx.Examples.PooledViews.Blueprints;
using EcsRx.Examples.PooledViews.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Plugins.Views.Components;
using UniRx;
using UnityEngine;

namespace EcsRx.Examples.PooledViews.Systems
{
    public class SpawnSystem : IReactToEntitySystem
    {
        private readonly IEntityCollection _defaultCollection;

        public IGroup Group => new Group(typeof(SpawnerComponent), typeof(ViewComponent));

        public SpawnSystem(IEntityDatabase entityDatabase)
        { _defaultCollection = entityDatabase.GetCollection(); }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            var spawnComponent = entity.GetComponent<SpawnerComponent>();
            return Observable.Interval(TimeSpan.FromSeconds(spawnComponent.SpawnRate)).Select(x => entity);
        }

        public void Process(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var view = viewComponent.View as GameObject;
            var blueprint = new SelfDestructBlueprint(view.transform.position);
            _defaultCollection.CreateEntity(blueprint);
        }
    }
}