using EcsRx.Infrastructure.Extensions;
using EcsRx.Views.Components;
using EcsRx.Zenject;

namespace EcsRx.Examples.AutoRegisterSystems
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            // You could optionally use instead of the scene based approach
            // this.BindAllSystemsWithinApplicationScope();
            this.RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {           
            var defaultPool = EntityCollectionManager.GetCollection();
            var entity = defaultPool.CreateEntity();
            entity.AddComponents(new ViewComponent());
        }
    }
}