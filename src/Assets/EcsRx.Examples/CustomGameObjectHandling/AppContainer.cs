using Assets.EcsRx.Examples.CustomGameObjectHandling.Components;
using Assets.EcsRx.Examples.CustomGameObjectHandling.Systems;
using EcsRx.Unity;
using EcsRx.Unity.Components;
using Zenject;

namespace Assets.EcsRx.Examples.CustomGameObjectHandling
{
    public class AppContainer : EcsRxContainer
    {
        [Inject]
        public ViewSetupSystem ViewSetupSystem { get; private set; }

        [Inject]
        public PlayerControlSystem PlayerControlSystem { get; private set; }

        [Inject]
        public CameraFollowSystem CameraFollowSystem { get; private set; }

        protected override void SetupSystems()
        {
            SystemExecutor.AddSystem(ViewSetupSystem);
            SystemExecutor.AddSystem(PlayerControlSystem);
            SystemExecutor.AddSystem(CameraFollowSystem);
        }

        protected override void SetupEntities()
        {
            var defaultPool = PoolManager.GetPool();
            var viewEntity = defaultPool.CreateEntity();
            viewEntity.AddComponent(new ViewComponent());
            viewEntity.AddComponent(new PlayerControlledComponent());
            viewEntity.AddComponent(new CameraFollowsComponent());
        }
    }
}