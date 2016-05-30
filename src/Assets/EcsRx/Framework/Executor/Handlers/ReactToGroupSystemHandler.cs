using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Pools;
using UniRx;

namespace EcsRx.Systems.Executor.Handlers
{
    public class ReactToGroupSystemHandler : IReactToGroupSystemHandler
    {
        public IPoolManager PoolManager { get; private set; }

        public ReactToGroupSystemHandler(IPoolManager poolManager)
        {
            PoolManager = poolManager;
        }

        public SubscriptionToken Setup(IReactToGroupSystem system)
        {
            var hasEntityPredicate = system.TargetGroup.TargettedEntities != null;
            var groupAccessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
            var subscription = system.ReactToGroup(groupAccessor)
                .Subscribe(accessor => accessor.Entities.ForEachRun(entity =>
                {
                    if (hasEntityPredicate)
                    {
                        if(system.TargetGroup.TargettedEntities(entity))
                        { system.Execute(entity); }
                        return;
                    }

                    system.Execute(entity);
                }));

            return new SubscriptionToken(null, subscription);
        }
    }
}