
using Assets.EcsRx.Examples.GroupFilters.Blueprints;
using EcsRx.Unity;

namespace Assets.EcsRx.Examples.GroupFilters
{
    public class Application : EcsRxApplication
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