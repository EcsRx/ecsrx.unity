using Assets.EcsRx.Examples.SimpleMovement.Components;
using EcsRx.Unity;
using EcsRx.Unity.Components;

namespace Assets.EcsRx.Examples.SimpleMovement
{
    public class AppContainer : EcsRxContainer
    {
        protected override void GameStarted()
        {
            var defaultPool = PoolManager.GetPool();
            var viewEntity = defaultPool.CreateEntity();
            viewEntity.AddComponent(new ViewComponent());
            viewEntity.AddComponent(new PlayerControlledComponent());
            viewEntity.AddComponent(new CameraFollowsComponent());
        }
    }
}