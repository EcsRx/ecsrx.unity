using EcsRx.Examples.ManuallyRegisterSystems.Systems;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;

namespace EcsRx.Examples.ManuallyRegisterSystems
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            this.RegisterSystem<DefaultViewResolver>();
            this.RegisterSystem<RandomMovementSystem>();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = CollectionManager.GetCollection();
            
            var entity = defaultPool.CreateEntity();
            entity.AddComponent(new ViewComponent());
        }
    }
}