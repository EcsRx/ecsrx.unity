using EcsRx.Examples.SceneFirstSetup.Components;
using EcsRx.Extensions;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;
using EcsRx.Zenject;
using EcsRx.Zenject.Extensions;

namespace EcsRx.Examples.SceneFirstSetup
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            this.BindAllSystemsWithinApplicationScope();
            this.RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = CollectionManager.GetCollection();

            var cubeEntity = defaultPool.CreateEntity();
            cubeEntity.AddComponent<ViewComponent>();
            cubeEntity.AddComponent<CubeComponent>();

            var sphereEntity = defaultPool.CreateEntity();
            sphereEntity.AddComponent<ViewComponent>();
            sphereEntity.AddComponent<SphereComponent>();
        }
    }
}
