using EcsRx.Views.Components;

namespace EcsRx.Unity.Examples.AutoRegisterSystems
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = CollectionManager.GetCollection();
            var entity = defaultPool.CreateEntity();
            entity.AddComponent(new ViewComponent());
        }
    }
}