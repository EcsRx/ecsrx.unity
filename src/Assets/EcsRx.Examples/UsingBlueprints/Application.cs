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
        protected override void ApplicationStarted()
        {
            var defaultPool = EntityDatabase.GetCollection();

            defaultPool.CreateEntity(new PlayerBlueprint("Player One"));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Two", 150.0f));
        }
    }
}