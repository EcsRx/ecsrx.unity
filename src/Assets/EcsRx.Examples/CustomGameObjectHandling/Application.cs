using EcsRx.Examples.CustomGameObjectHandling.Components;
using EcsRx.Unity.Extensions;
using EcsRx.Zenject;
using EcsRx.Zenject.Extensions;

namespace EcsRx.Examples.CustomGameObjectHandling
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            this.BindAllSystemsWithinApplicationScope();
            this.RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = CollectionManager.GetCollection();
            var viewEntity = defaultPool.CreateEntity();
            viewEntity.AddComponents(new CustomViewComponent(), new PlayerControlledComponent(), new CameraFollowsComponent());
        }
    }
}