using Assets.EcsRx.Examples.RandomReactions.Components;
using EcsRx.Unity;
using EcsRx.Unity.Components;

namespace Assets.EcsRx.Examples.RandomReactions
{
    public class AppContainer : EcsRxContainer
    {
        private readonly int _cubeCount = 500;
        
        protected override void GameStarted()
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