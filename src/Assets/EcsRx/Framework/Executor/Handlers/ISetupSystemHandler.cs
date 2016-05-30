using EcsRx.Pools;

namespace EcsRx.Systems.Executor.Handlers
{
    public interface ISetupSystemHandler
    {
        IPoolManager PoolManager { get; }
        void Setup(ISetupSystem system);
    }
}