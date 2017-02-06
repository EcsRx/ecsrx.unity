using System.Collections.Generic;
using System.Linq;
using Assets.EcsRx.Examples.GroupFilters.Components;
using Assets.EcsRx.Framework.Groups.Filtration;
using EcsRx.Extensions;
using EcsRx.Groups;
using UniRx;

namespace Assets.EcsRx.Examples.GroupFilters.Filters
{
    public class CacheableLeaderboardFilter : CacheableGroupAccessorFilter<HasScoreComponent>
    {
        public CacheableLeaderboardFilter(IGroupAccessor groupAccessor) : base(groupAccessor)
        {}

        protected override IEnumerable<HasScoreComponent> FilterQuery()
        {
            return GroupAccessor.Entities
                .Select(x => x.GetComponent<HasScoreComponent>())
                .OrderBy(x => x.Score.Value)
                .Take(5);
        }

        protected override IObservable<Unit> TriggerOnChange()
        {
            var scoreList = GroupAccessor.Entities
                .Select(x => x.GetComponent<HasScoreComponent>().Score)
                .ToReactiveCollection();
            
            var entityAdded = scoreList.ObserveAdd().AsTrigger();
            var entityRemoved = scoreList.ObserveRemove().AsTrigger();
            var valuesChanges = scoreList.ToObservable().DistinctUntilChanged().AsTrigger();

            return Observable.Merge(entityAdded, entityRemoved, valuesChanges);
        }
    }
}