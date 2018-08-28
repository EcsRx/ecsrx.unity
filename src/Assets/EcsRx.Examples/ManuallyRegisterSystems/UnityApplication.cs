using EcsRx.Examples.ManuallyRegisterSystems.Systems;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;

namespace EcsRx.Examples.ManuallyRegisterSystems
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarted()
        {
            this.RegisterSystem<DefaultViewResolver>();
            this.RegisterSystem<RandomMovementSystem>();
            
            var defaultPool = CollectionManager.GetCollection();
            
            var entity = defaultPool.CreateEntity();
            entity.AddComponents(new ViewComponent());
        }
    }
}