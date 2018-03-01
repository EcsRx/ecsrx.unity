using EcsRx.Unity.Examples.SimpleMovement.Components;
using EcsRx.Views.Components;

namespace EcsRx.Unity.Examples.SimpleMovement
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();
            var viewEntity = defaultPool.CreateEntity();
            viewEntity.AddComponent(new ViewComponent());
            viewEntity.AddComponent(new PlayerControlledComponent());
            viewEntity.AddComponent(new CameraFollowsComponent());
        }
    }
}