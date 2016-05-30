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

        public void Setup(ISetupSystem system)
        {
            var hasEntityPredicate = system.TargetGroup.TargettedEntities != null;
            var groupAccessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
            groupAccessor.Entities.ForEachRun(x =>
            {
                if (hasEntityPredicate)
                {
                    if(system.TargetGroup.TargettedEntities(x))
                    { system.Setup(x); }
                    return;
                }

                system.Setup(x);
            });
        }
    }
}