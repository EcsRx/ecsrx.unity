﻿using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Groups.Accessors;
using EcsRx.Groups.Computed;
using EcsRx.Unity.Examples.GroupFilters.Components;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Examples.GroupFilters.Filters
{
    public class CacheableLeaderboardFilter : CacheableComputedGroup<HasScoreComponent>
    {
        public CacheableLeaderboardFilter(IObservableGroup groupAccessor) : base(groupAccessor)
        {}

        protected override IObservable<bool> TriggerOnChange()
        { return Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => true); }

        protected override IEnumerable<HasScoreComponent> FilterQuery()
        {
            Debug.Log("Updating");
            return ObservableGroup.Entities
                .Select(x => x.GetComponent<HasScoreComponent>())
                .OrderByDescending(x => x.Score.Value)
                .Take(5)
                .ToList();
        }
    }
}