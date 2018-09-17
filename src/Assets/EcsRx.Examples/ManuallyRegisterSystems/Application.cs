using EcsRx.Examples.ManuallyRegisterSystems.Systems;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;
using EcsRx.Zenject;
using EcsRx.Zenject.Extensions;

namespace EcsRx.Examples.ManuallyRegisterSystems
{
    public class Application : EcsRxApplicationBehaviour
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
            entity.AddComponents(new ViewComponent());
        }
    }
}