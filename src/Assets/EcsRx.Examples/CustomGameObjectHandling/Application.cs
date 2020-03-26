using EcsRx.Examples.CustomGameObjectHandling.Components;
using EcsRx.Extensions;
using EcsRx.Zenject;

namespace EcsRx.Examples.CustomGameObjectHandling
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarted()
        {
            var defaultPool = EntityDatabase.GetCollection();
            var viewEntity = defaultPool.CreateEntity();
            viewEntity.AddComponents(new CustomViewComponent(), new PlayerControlledComponent(), new CameraFollowsComponent());
        }
    }
}