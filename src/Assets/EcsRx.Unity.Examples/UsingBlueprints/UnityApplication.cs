using EcsRx.Unity.Examples.UsingBlueprints.Blueprints;

namespace EcsRx.Unity.Examples.UsingBlueprints
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

            defaultPool.CreateEntity(new PlayerBlueprint("Player One"));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Two", 150.0f));
        }
    }
}