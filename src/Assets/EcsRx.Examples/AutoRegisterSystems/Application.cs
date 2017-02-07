using EcsRx.Unity;
using EcsRx.Unity.Components;

namespace Assets.EcsRx.Examples.AutoRegisterSystems
{
    public class Application : EcsRxApplication
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();
            var entity = defaultPool.CreateEntity();
            entity.AddComponent(new ViewComponent());
        }
    }
}