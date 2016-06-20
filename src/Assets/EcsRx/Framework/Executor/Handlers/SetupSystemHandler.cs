using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Pools;
using UniRx;

namespace EcsRx.Systems.Executor.Handlers
{
    public class SetupSystemHandler : ISetupSystemHandler
    {
        public IPoolManager PoolManager { get; private set; }

        public SetupSystemHandler(IPoolManager poolManager)
        {
            PoolManager = poolManager;
        }

        public IEnumerable<SubscriptionToken> Setup(ISetupSystem system)
        {
            var subscriptions = new List<SubscriptionToken>();
            var groupAccessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
            groupAccessor.Entities.ForEachRun(x =>
            {
                var possibleSubscription = ProcessEntity(system, x);
                if(possibleSubscription != null) { subscriptions.Add(possibleSubscription); }
            });

            return subscriptions;
        }

        public SubscriptionToken ProcessEntity(ISetupSystem system, IEntity entity)
        {
            var hasEntityPredicate = system.TargetGroup.TargettedEntities != null;
            if (!hasEntityPredicate)
            {
                system.Setup(entity);
                return null;
            }

            if (system.TargetGroup.TargettedEntities(entity))
            {
                system.Setup(entity);
                return null;
            }

            var subscription = entity.WaitForPredicateMet(system.TargetGroup.TargettedEntities)
                .Subscribe(system.Setup);

            var subscriptionToken = new SubscriptionToken(entity, subscription);
            return subscriptionToken;
        }
    }
}