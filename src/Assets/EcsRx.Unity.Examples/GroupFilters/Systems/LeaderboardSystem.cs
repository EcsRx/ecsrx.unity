using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Collections;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Systems;
using EcsRx.Unity.Examples.GroupFilters.Components;
using EcsRx.Unity.Examples.GroupFilters.Filters;
using EcsRx.Unity.Examples.GroupFilters.Groups;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace EcsRx.Unity.Examples.GroupFilters.Systems
{
    public class LeaderboardSystem : IManualSystem
    {
        private LeaderboardFilter _leaderboardFilter;
        private CacheableLeaderboardFilter _cacheableLeaderboardFilter;

        private IDisposable _subscription;
        private Text _leaderBoardText;
        private Toggle _useCacheToggle;

        public IGroup TargetGroup => new EmptyGroup();

        public LeaderboardSystem(IEntityCollectionManager collectionManager)
        {
            var accessor = collectionManager.CreateObservableGroup(new HasScoreGroup());
            _leaderboardFilter = new LeaderboardFilter(accessor);
            _cacheableLeaderboardFilter = new CacheableLeaderboardFilter(accessor);
            _leaderBoardText = GameObject.Find("Leaderboard").GetComponent<Text>();
            _useCacheToggle = GameObject.Find("UseCache").GetComponent<Toggle>();
        }

        public void StartSystem(IObservableGroup group)
        { _subscription = Observable.EveryUpdate().Subscribe(x => UpdateLeaderboard()); }

        public void UpdateLeaderboard()
        {
            IList<HasScoreComponent> leaderboardEntities;

            if(_useCacheToggle.isOn)
            { leaderboardEntities = _cacheableLeaderboardFilter.Filter().ToList(); }
            else
            { leaderboardEntities = _leaderboardFilter.Filter().ToList(); }

            var leaderboardString = new StringBuilder();
            for (var i = 0; i < leaderboardEntities.Count; i++)
            {
                leaderboardString.AppendLine($"{i} - {leaderboardEntities[i].Name} [{leaderboardEntities[i].Score}]");
            }
            _leaderBoardText.text = leaderboardString.ToString();
        }

        public void StopSystem(IObservableGroup @group)
        { _subscription.Dispose(); }
    }
}