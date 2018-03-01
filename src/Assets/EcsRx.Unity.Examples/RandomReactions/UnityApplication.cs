using EcsRx.Unity.Examples.RandomReactions.Components;
using EcsRx.Views.Components;

namespace EcsRx.Unity.Examples.RandomReactions
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        private readonly int _cubeCount = 500;
        
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();

            for (var i = 0; i < _cubeCount; i++)
            {
                var viewEntity = defaultPool.CreateEntity();
                viewEntity.AddComponent(new ViewComponent());
                viewEntity.AddComponent(new RandomColorComponent());
            }
        }
    }
}