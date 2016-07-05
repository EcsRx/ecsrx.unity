using Assets.EcsRx.Examples.SceneFirstSetup.Components;
using EcsRx.Unity;
using EcsRx.Unity.Components;

namespace Assets.EcsRx.Examples.SceneFirstSetup
{
    public class AppContainer : EcsRxContainer
    {
        protected override void GameStarted()
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
