using Assets.EcsRx.Examples.CustomGameObjectHandling.Components;
using Assets.EcsRx.Examples.CustomGameObjectHandling.Systems;
using EcsRx.Unity;
using Zenject;

namespace Assets.EcsRx.Examples.CustomGameObjectHandling
{
    public class AppContainer : EcsRxContainer
    {
        [Inject]
        public CustomViewSetupSystem CustomViewSetupSystem { get; private set; }

        [Inject]
        public PlayerControlSystem PlayerControlSystem { get; private set; }

        [Inject]
        public CameraFollowSystem CameraFollowSystem { get; private set; }

        protected override void GameStarted()
        {
            SystemExecutor.AddSystem(CustomViewSetupSystem);
            SystemExecutor.AddSystem(PlayerControlSystem);
            SystemExecutor.AddSystem(CameraFollowSystem);

            var defaultPool = PoolManager.GetPool();
            var viewEntity = defaultPool.CreateEntity();
            viewEntity.AddComponent(new CustomViewComponent());
            viewEntity.AddComponent(new PlayerControlledComponent());
            viewEntity.AddComponent(new CameraFollowsComponent());
        }
    }
}