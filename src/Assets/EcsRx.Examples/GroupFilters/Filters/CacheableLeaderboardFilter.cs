using System;
using System.Collections.Generic;
using System.Linq;
using Assets.EcsRx.Examples.GroupFilters.Components;
using Assets.EcsRx.Framework.Groups.Filtration;
using EcsRx.Extensions;
using EcsRx.Groups;
using UniRx;
using UnityEngine;

namespace Assets.EcsRx.Examples.GroupFilters.Filters
{
    public class CacheableLeaderboardFilter : CacheableGroupAccessorFilter<HasScoreComponent>
    {
        public CacheableLeaderboardFilter(IGroupAccessor groupAccessor) : base(groupAccessor)
        {}

        protected override IEnumerable<HasScoreComponent> FilterQuery()
        {
            Debug.Log("Updating");
            return GroupAccessor.Entities
                .Select(x => x.GetComponent<HasScoreComponent>())
                .OrderByDescending(x => x.Score.Value)
                .Take(5)
                .ToList();
        }

        protected override IObservable<Unit> TriggerOnChange()
        {
            return Observable.Interval(TimeSpan.FromSeconds(1)).AsTrigger();
        }
    }
}