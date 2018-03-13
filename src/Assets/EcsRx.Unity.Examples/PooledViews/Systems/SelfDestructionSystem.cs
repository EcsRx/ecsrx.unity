using System;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Examples.PooledViews.Components;
using EcsRx.Views.Components;
using UniRx;

namespace EcsRx.Unity.Examples.PooledViews.Systems
{
    public class SelfDestructionSystem : IReactToEntitySystem
    {
        public IGroup TargetGroup => new Group(typeof(SelfDestructComponent), typeof(ViewComponent));
        private readonly IEntityCollection _defaultCollection;

        public SelfDestructionSystem(IEntityCollectionManager collectionManager)
        { _defaultCollection = collectionManager.GetCollection(); }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            var selfDestructComponent = entity.GetComponent<SelfDestructComponent>();
            return Observable.Interval(TimeSpan.FromSeconds(selfDestructComponent.Lifetime)).First().Select(x => entity);
        }

        public void Execute(IEntity entity)
        { _defaultCollection.RemoveEntity(entity); }
    }
}