using Assets.Tests.Scenes.GroupedPerformance.Components;
using Assets.Tests.Scenes.GroupedPerformance.Systems;
using Assets.Tests.Scenes.GroupedPerformance.ViewResolvers;
using EcsRx.Unity;
using EcsRx.Unity.Components;

namespace Assets.Tests.Scenes.GroupedPerformance
{
    public class Application : EcsRxApplication
    {
        private readonly int _cubeCount = 500;

        protected override void GameStarting()
        {
            RegisterBoundSystem<CubeViewResolver>();

            // Enable one of the below to see impact
            RegisterBoundSystem<GroupRotationSystem>();
            //RegisterBoundSystem<EntityRotationSystem>();
        }

        protected override void GameStarted()
        {
            var defaultPool = PoolManager.GetPool();

            for (var i = 0; i < _cubeCount; i++)
            {
                var viewEntity = defaultPool.CreateEntity();
                viewEntity.AddComponent(new ViewComponent());
                viewEntity.AddComponent(new RotationComponent { RotationSpeed = 10.0f });
            }
        }
    }
}