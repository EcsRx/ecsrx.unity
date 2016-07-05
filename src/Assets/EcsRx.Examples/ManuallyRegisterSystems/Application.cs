using Assets.EcsRx.Examples.ManuallyRegisterSystems.Systems;
using EcsRx.Unity;
using EcsRx.Unity.Components;
using Zenject;

namespace Assets.EcsRx.Examples.ManuallyRegisterSystems
{
    public class Application : EcsRxApplication
    {
        [Inject]
        public DefaultViewResolver DefaultViewResolver { get; private set; }

        [Inject]
        public RandomMovementSystem RandomMovementSystem { get; private set; }
        
        protected override void GameStarted()
        {
            var defaultPool = PoolManager.GetPool();

            SystemExecutor.AddSystem(DefaultViewResolver);
            SystemExecutor.AddSystem(RandomMovementSystem);
            
            var entity = defaultPool.CreateEntity();
            entity.AddComponent(new ViewComponent());
        }
    }
}