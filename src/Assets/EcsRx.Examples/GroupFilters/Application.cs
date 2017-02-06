
using Assets.EcsRx.Examples.GroupFilters.Blueprints;
using EcsRx.Unity;

namespace Assets.EcsRx.Examples.GroupFilters
{
    public class Application : EcsRxApplication
    {
        protected override void GameStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void GameStarted()
        {
            var defaultPool = PoolManager.GetPool();
            
            defaultPool.CreateEntity(new PlayerBlueprint("Player One", 0));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Two", 0));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Three", 0));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Four", 0));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Five", 0));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Six", 0));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Seven", 0));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Eight", 0));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Nine", 0));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Ten", 0));
        }
    }
}