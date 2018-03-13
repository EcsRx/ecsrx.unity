using EcsRx.Unity.Examples.ManuallyRegisterSystems.Systems;
using EcsRx.Views.Components;

namespace EcsRx.Unity.Examples.ManuallyRegisterSystems
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            RegisterSystem<DefaultViewResolver>();
            RegisterSystem<RandomMovementSystem>();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = CollectionManager.GetCollection();
            
            var entity = defaultPool.CreateEntity();
            entity.AddComponent(new ViewComponent());
        }
    }
}