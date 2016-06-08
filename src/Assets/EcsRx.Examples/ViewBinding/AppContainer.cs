using Assets.EcsRx.Examples.ViewBinding.Components;
using EcsRx.Unity;
using EcsRx.Unity.Components;

namespace Assets.EcsRx.Examples.ViewBinding
{
    public class AppContainer : EcsRxContainer
    {
        protected override void SetupSystems() { }

        protected override void SetupEntities()
        {
            var defaultPool = PoolManager.GetPool();

            var cubeEntity = defaultPool.CreateEntity();
            cubeEntity.AddComponent<ViewComponent>();
            cubeEntity.AddComponent<CubeComponent>();

            var sphereEntity = defaultPool.CreateEntity();
            sphereEntity.AddComponent<ViewComponent>();
            sphereEntity.AddComponent<SphereComponent>();
        }
    }
}
