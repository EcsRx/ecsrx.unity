using EcsRx.Examples.GameObjectBinding.Components;
using EcsRx.Extensions;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;

namespace EcsRx.Examples.GameObjectBinding
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            this.BindAllSystemsWithinApplicationScope();
        }

        protected override void ApplicationStarted()
        {
            this.RegisterAllBoundSystems();
            var entityCollection = CollectionManager.GetCollection();

            var cubeEntity = entityCollection.CreateEntity();
            cubeEntity.AddComponent<ViewComponent>();
            cubeEntity.AddComponent<CubeComponent>();

            var sphereEntity = entityCollection.CreateEntity();
            sphereEntity.AddComponent<ViewComponent>();
            sphereEntity.AddComponent<SphereComponent>();
        }
    }
}