using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Pools;
using UniRx;

namespace EcsRx.Systems.Executor.Handlers
{
    public class ReactToEntitySystemHandler : IReactToEntitySystemHandler
    {
        public IPoolManager PoolManager { get; private set; }

        public ReactToEntitySystemHandler(IPoolManager poolManager)
        {
            PoolManager = poolManager;
        }

        public IEnumerable<SubscriptionToken> Setup(IReactToEntitySystem system)
        {
            var accessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
            return accessor.Entities.ForEachRun(x => ProcessEntity(system, x));
        }

        public SubscriptionToken ProcessEntity(IReactToEntitySystem system, IEntity entity)
        {
            var hasEntityPredicate = system.TargetGroup.TargettedEntities != null;
            var subscription = system.ReactToEntity(entity)
                .Subscribe(x =>
                {
                    if (hasEntityPredicate)
                    {
                        if(system.TargetGroup.TargettedEntities(x))
                        {  system.Execute(x); }
                        return;
                    }

                    system.Execute(x);
                });

            return new SubscriptionToken(entity, subscription);
        }
    }
}