using EcsRx.Unity.Examples.PluginExample.HelloWorldPlugin.components;

namespace EcsRx.Unity.Examples.PluginExample
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
            RegisterPlugin(new HelloWorldPlugin.HelloWorldPlugin());
        }
        
        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();
            var entity = defaultPool.CreateEntity();
            entity.AddComponent<SayHelloWorldComponent>();
        }
    }
}