using System.Collections.Generic;
using System.Linq;
using EcsRx.Groups.Accessors;
using EcsRx.Groups.Computed;
using EcsRx.Unity.Examples.GroupFilters.Components;

namespace EcsRx.Unity.Examples.GroupFilters.Filters
{
    public class LeaderboardFilter : IComputedGroup<HasScoreComponent>
    {
        public IObservableGroup ObservableGroup { get; }

        public LeaderboardFilter(IObservableGroup observableGroup)
        { ObservableGroup = observableGroup; }

        public IEnumerable<HasScoreComponent> Filter()
        {
            return ObservableGroup.Entities
                .Select(x => x.GetComponent<HasScoreComponent>())
                .OrderByDescending(x => x.Score.Value)
                .Take(5);
        }
    }
}