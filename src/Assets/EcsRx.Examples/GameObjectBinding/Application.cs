using EcsRx.Examples.GameObjectBinding.Components;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.Zenject;

namespace EcsRx.Examples.GameObjectBinding
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarted()
        {
            var entityCollection = EntityDatabase.GetCollection();

            var cubeEntity = entityCollection.CreateEntity();
            cubeEntity.AddComponent<ViewComponent>();
            cubeEntity.AddComponent<CubeComponent>();

            var sphereEntity = entityCollection.CreateEntity();
            sphereEntity.AddComponent<ViewComponent>();
            sphereEntity.AddComponent<SphereComponent>();
        }
    }
}