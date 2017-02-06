using System;
using System.Linq;
using Assets.EcsRx.Examples.GroupFilters.Components;
using Assets.EcsRx.Examples.GroupFilters.Filters;
using Assets.EcsRx.Examples.GroupFilters.Groups;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using UniRx;

namespace Assets.EcsRx.Examples.GroupFilters.Systems
{
    public class LeaderboardSystem : IManualSystem
    {
        private LeaderboardFilter _leaderboardFilter;
        private IDisposable _subscription;

        public IGroup TargetGroup { get { return new EmptyGroup(); } }

        public LeaderboardSystem(IPoolManager poolManager)
        {
            var accessor = poolManager.CreateGroupAccessor(new HasScoreGroup());
            _leaderboardFilter = new LeaderboardFilter(accessor);
        }

        public void StartSystem(IGroupAccessor @group)
        { _subscription = Observable.EveryUpdate().Subscribe(x => UpdateLeaderboard()); }

        public void UpdateLeaderboard()
        {
            var leaderboardEntities = _leaderboardFilter.Filter().ToList();
            for (var i = 0; i < leaderboardEntities.Count; i++)
            {
                
            }
        }

        public void StopSystem(IGroupAccessor @group)
        { _subscription.Dispose(); }
    }
}