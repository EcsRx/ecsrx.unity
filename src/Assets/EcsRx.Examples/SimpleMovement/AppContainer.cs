using Assets.Examples.SimpleMovement.Components;
using Assets.Examples.SimpleMovement.Systems;
using EcsRx.Unity;
using Zenject;

namespace Assets.Examples.SimpleMovement
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