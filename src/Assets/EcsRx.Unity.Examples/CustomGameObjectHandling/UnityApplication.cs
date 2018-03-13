using EcsRx.Unity.Examples.CustomGameObjectHandling.Components;
using EcsRx.Unity.Examples.CustomGameObjectHandling.Systems;
using Zenject;

namespace EcsRx.Unity.Examples.CustomGameObjectHandling
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