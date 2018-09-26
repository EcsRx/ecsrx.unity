using EcsRx.Infrastructure.Extensions;
using EcsRx.Views.Components;
using EcsRx.Zenject;

namespace EcsRx.Examples.AutoRegisterSystems
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarted()
        {           
            var defaultPool = EntityCollectionManager.GetCollection();
            var entity = defaultPool.CreateEntity();
            entity.AddComponents(new ViewComponent());
        }
    }
}