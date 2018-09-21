using EcsRx.Examples.UsingBlueprints.Blueprints;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Zenject;
using EcsRx.Zenject.Extensions;

namespace EcsRx.Examples.UsingBlueprints
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            this.BindAllSystemsWithinApplicationScope();
            this.RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = EntityCollectionManager.GetCollection();

            defaultPool.CreateEntity(new PlayerBlueprint("Player One"));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Two", 150.0f));
        }
    }
}