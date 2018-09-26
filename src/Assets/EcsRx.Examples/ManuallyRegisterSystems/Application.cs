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
        protected override void StartSystems()
        {
            // This one we are manually binding and starting at the same time
            this.BindAndStartSystem<DefaultViewResolver>();
            
            // This one we are manually starting from the installer which has already bound it
            this.StartSystem<RandomMovementSystem>();
        }

        protected override void ApplicationStarted()
        {
            
            
            var defaultPool = EntityCollectionManager.GetCollection();
            
            var entity = defaultPool.CreateEntity();
            entity.AddComponents(new ViewComponent());
        }
    }
}