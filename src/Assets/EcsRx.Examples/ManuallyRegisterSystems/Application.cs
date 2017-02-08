using Assets.EcsRx.Examples.ManuallyRegisterSystems.Systems;
using EcsRx.Unity;
using EcsRx.Unity.Components;
using Zenject;

namespace Assets.EcsRx.Examples.ManuallyRegisterSystems
{
    public class Application : EcsRxApplication
    {
        protected override void ApplicationStarting()
        {
            RegisterBoundSystem<DefaultViewResolver>();
            RegisterBoundSystem<RandomMovementSystem>();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();
            
            var entity = defaultPool.CreateEntity();
            entity.AddComponent(new ViewComponent());
        }
    }
}