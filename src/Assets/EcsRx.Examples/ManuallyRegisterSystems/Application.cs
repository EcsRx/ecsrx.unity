using EcsRx.Examples.ManuallyRegisterSystems.Systems;
using EcsRx.Infrastructure.Extensions;
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
            // This one we are manually binding and registering at same time
            this.BindAndRegisterSystem<DefaultViewResolver>();
            
            // This one we are manually registering from the installer which has already bound it
            this.RegisterSystem<RandomMovementSystem>();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = EntityCollectionManager.GetCollection();
            
            var entity = defaultPool.CreateEntity();
            entity.AddComponents(new ViewComponent());
        }
    }
}