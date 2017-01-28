using System.Linq;
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
                .Subscribe(accessor =>
                {
                    var entities = accessor.Entities.ToList();
                    var entityCount = entities.Count - 1;
                    for (var i = entityCount; i >= 0; i--)
                    {
                        if (hasEntityPredicate)
                        {
                            if (system.TargetGroup.TargettedEntities(entities[i]))
                            {
                                system.Execute(entities[i]);
                            }
                            return;
                        }

                        system.Execute(entities[i]);
                    }
                });

            return new SubscriptionToken(null, subscription);
        }
    }
}
