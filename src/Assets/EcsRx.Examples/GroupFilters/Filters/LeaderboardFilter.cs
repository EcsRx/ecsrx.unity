using System.Collections.Generic;
using System.Linq;
using Assets.EcsRx.Examples.GroupFilters.Components;
using Assets.EcsRx.Framework.Groups.Filtration;
using EcsRx.Groups;

namespace Assets.EcsRx.Examples.GroupFilters.Filters
{
    public class LeaderboardFilter : IGroupAccessorFilter<HasScoreComponent>
    {
        public IGroupAccessor GroupAccessor { get; private set; }

        public LeaderboardFilter(IGroupAccessor groupAccessor)
        { GroupAccessor = groupAccessor; }

        public IEnumerable<HasScoreComponent> Filter()
        {
            return GroupAccessor.Entities
                .Select(x => x.GetComponent<HasScoreComponent>())
                .OrderBy(x => x.Score.Value)
                .Take(5);
        }
    }
}