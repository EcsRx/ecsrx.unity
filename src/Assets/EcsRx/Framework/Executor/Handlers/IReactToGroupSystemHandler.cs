using EcsRx.Pools;

namespace EcsRx.Systems.Executor.Handlers
{
    public interface IReactToGroupSystemHandler
    {
        IPoolManager PoolManager { get; }
        SubscriptionToken Setup(IReactToGroupSystem system);
    }
}