using System.Collections.Generic;
using EcsRx.Pools;

namespace EcsRx.Systems.Executor
{
    public interface ISystemExecutor
    {
        IPoolManager PoolManager { get; }
        IEnumerable<ISystem> Systems { get; }

        void RemoveSystem(ISystem system);
        void AddSystem(ISystem system);
    }
}