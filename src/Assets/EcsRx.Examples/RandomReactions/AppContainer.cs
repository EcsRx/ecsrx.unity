using Assets.Examples.RandomReactions.Components;
using Assets.Examples.RandomReactions.Systems;
using EcsRx.Unity;
using Zenject;

namespace Assets.Examples.RandomReactions
{
    public class AppContainer : EcsRxContainer
    {
        private readonly int _cubeCount = 500;

        [Inject]
        public ColorChangingSystem ColorChangingSystem { get; private set; }

        [Inject]
        public CubeSetupSystem CubeSetupSystem { get; private set; }

        [Inject]
        public CubeColourChangerSystem CubeColourChangerSystem { get; private set; }

        protected override void SetupSystems()
        {
            SystemExecutor.AddSystem(CubeSetupSystem);
            SystemExecutor.AddSystem(ColorChangingSystem);
            SystemExecutor.AddSystem(CubeColourChangerSystem);
        }

        protected override void SetupEntities()
        {
            var defaultPool = PoolManager.GetPool();

            for (var i = 0; i < _cubeCount; i++)
            {
                var viewEntity = defaultPool.CreateEntity();
                viewEntity.AddComponent(new HasCubeComponent());
                viewEntity.AddComponent(new RandomColorComponent());
            }
        }
    }
}