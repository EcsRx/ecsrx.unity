using Assets.EcsRx.Examples.PluginExample.HelloWorldPlugin.components;
using EcsRx.Unity;

namespace Assets.EcsRx.Examples.PluginExample
{
    public class Application : EcsRxApplication
    {
        protected override void GameStarting()
        {
            RegisterAllBoundSystems();
            RegisterPlugin(new HelloWorldPlugin.HelloWorldPlugin());
        }
        
        protected override void GameStarted()
        {
            var defaultPool = PoolManager.GetPool();
            var entity = defaultPool.CreateEntity();
            entity.AddComponent<SayHelloWorldComponent>();
        }
    }
}