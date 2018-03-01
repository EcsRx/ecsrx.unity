using System;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Examples.GroupFilters.Components;
using EcsRx.Unity.Examples.GroupFilters.Groups;
using UniRx;
using Random = UnityEngine.Random;

namespace EcsRx.Unity.Examples.GroupFilters.Systems
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