using EcsRx.Unity;
using EcsRx.Unity.Components;
using Zenject;

namespace Assets.EcsRx.Examples.ManuallyRegisterSystems
{
    public class AppContainer : EcsRxContainer
    {
        [Inject]


        protected override void SetupSystems()
        {
            
        }

        protected override void SetupEntities()
        {
            var defaultPool = PoolManager.GetPool();

            var entity = defaultPool.CreateEntity();
            entity.AddComponent(new ViewComponent());
        }
    }
}