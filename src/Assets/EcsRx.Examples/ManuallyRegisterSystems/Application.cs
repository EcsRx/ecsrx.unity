using SystemsRx.Infrastructure.Extensions;
using EcsRx.Examples.ManuallyRegisterSystems.Systems;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.Zenject;

namespace EcsRx.Examples.ManuallyRegisterSystems
{
    public class Application : EcsRxApplicationBehaviour
    {
        // We override this to stop auto bindings
        protected override void BindSystems()
        {
            Container.Bind<RandomMovementSystem>();
        }

        // We override this to manually control how systems start
        protected override void StartSystems()
        {
            // This one we are manually binding and starting at the same time
            this.BindAndStartSystem<DefaultViewResolver>();
            
            // This one we are manually starting from the installer which has already bound it
            this.StartSystem<RandomMovementSystem>();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = EntityDatabase.GetCollection();
            
            var entity = defaultPool.CreateEntity();
            entity.AddComponents(new ViewComponent());
        }
    }
}