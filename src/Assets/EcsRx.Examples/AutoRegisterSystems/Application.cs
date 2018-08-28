using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;

namespace EcsRx.Examples.AutoRegisterSystems
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            // You could optionally use instead of the scene based approach
            // this.BindAllSystemsWithinApplicationScope();
        }

        protected override void ApplicationStarted()
        {
            this.RegisterAllBoundSystems();
            
            var defaultPool = CollectionManager.GetCollection();
            var entity = defaultPool.CreateEntity();
            entity.AddComponents(new ViewComponent());
        }
    }
}