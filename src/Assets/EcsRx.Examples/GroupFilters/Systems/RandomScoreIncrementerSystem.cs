using System;
using Assets.EcsRx.Examples.GroupFilters.Components;
using Assets.EcsRx.Examples.GroupFilters.Groups;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using Random = UnityEngine.Random;

namespace Assets.EcsRx.Examples.GroupFilters.Systems
{
    public class RandomScoreIncrementerSystem : IReactToEntitySystem
    {
        public IGroup TargetGroup { get { return new HasScoreGroup(); } }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        { return Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => entity); }

        public void Execute(IEntity entity)
        {
            var scoreComponent = entity.GetComponent<HasScoreComponent>();
            var randomIncrement = Random.Range(1, 5);
            scoreComponent.Score.Value += randomIncrement;
        }
    }
}