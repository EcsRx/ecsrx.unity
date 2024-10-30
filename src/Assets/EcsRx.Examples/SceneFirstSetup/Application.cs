using EcsRx.Examples.SceneFirstSetup.Components;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.Zenject;

namespace EcsRx.Examples.SceneFirstSetup
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarted()
        {
            var defaultPool = EntityDatabase.GetCollection();

            var cubeEntity = defaultPool.CreateEntity();
            cubeEntity.AddComponent<ViewComponent>();
            cubeEntity.AddComponent<CubeComponent>();

            var sphereEntity = defaultPool.CreateEntity();
            sphereEntity.AddComponent<ViewComponent>();
            sphereEntity.AddComponent<SphereComponent>();
        }
    }
}
