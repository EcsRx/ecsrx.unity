using Assets.EcsRx.Examples.UsingBlueprints.Blueprints;
using EcsRx.Unity;

namespace Assets.EcsRx.Examples.UsingBlueprints
{
    public class AppContainer : EcsRxContainer
    {
        protected override void SetupSystems() { }

        protected override void SetupEntities()
        {
            var defaultPool = PoolManager.GetPool();

            defaultPool.CreateEntity(new PlayerBlueprint("Player One"));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Two", 150.0f));
        }
    }
}