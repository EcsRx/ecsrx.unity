using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Pools;
using UniRx;

namespace EcsRx.Systems.Executor.Handlers
{
    public class ManualSystemHandler : IManualSystemHandler
    {
        public IPoolManager PoolManager { get; private set; }

        public ManualSystemHandler(IPoolManager poolManager)
        {
            PoolManager = poolManager;
        }

        public void Start(IManualSystem system)
        {
            var groupAccessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
            system.StartSystem(groupAccessor);
        }

        public void Stop(IManualSystem system)
        {
            var groupAccessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
            system.StopSystem(groupAccessor);
        }
    }
}