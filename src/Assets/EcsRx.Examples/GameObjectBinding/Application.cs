using Assets.EcsRx.Examples.GameObjectBinding.components;
using EcsRx.Unity;
using EcsRx.Unity.Components;

namespace Assets.EcsRx.Examples.GameObjectBinding
{
    public class Application : EcsRxApplication
    {
        protected override void GameStarting()
        {
            RegisterAllBoundSystems();
        }

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