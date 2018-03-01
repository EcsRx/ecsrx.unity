using EcsRx.Unity.Examples.GroupFilters.Blueprints;

namespace EcsRx.Unity.Examples.GroupFilters
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();

            var entityCount = 1000;

            for (var i = 0; i < entityCount; i++)
            { defaultPool.CreateEntity(new PlayerBlueprint("Player " + i, 0)); }
        }
    }
}