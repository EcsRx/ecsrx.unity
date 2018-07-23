using EcsRx.Examples.CustomGameObjectHandling.Components;
using EcsRx.Examples.CustomGameObjectHandling.Systems;
using EcsRx.Unity;
using Zenject;

namespace EcsRx.Examples.CustomGameObjectHandling
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        [Inject]
        public CustomViewSetupSystem CustomViewSetupSystem { get; private set; }

        [Inject]
        public PlayerControlSystem PlayerControlSystem { get; private set; }

        [Inject]
        public CameraFollowSystem CameraFollowSystem { get; private set; }
        
        protected override void ApplicationStarted()
        {
            SystemExecutor.AddSystem(CustomViewSetupSystem);
            SystemExecutor.AddSystem(PlayerControlSystem);
            SystemExecutor.AddSystem(CameraFollowSystem);

            var defaultPool = CollectionManager.GetCollection();
            var viewEntity = defaultPool.CreateEntity();
            viewEntity.AddComponent(new CustomViewComponent());
            viewEntity.AddComponent(new PlayerControlledComponent());
            viewEntity.AddComponent(new CameraFollowsComponent());
        }
    }
}