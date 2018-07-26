using EcsRx.Examples.PluginExample.HelloWorldPlugin.components;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;

namespace EcsRx.Examples.PluginExample
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            RegisterPlugin(new HelloWorldPlugin.HelloWorldPlugin());
        }
        
        protected override void ApplicationStarted()
        {
            var defaultPool = CollectionManager.GetCollection();
            var entity = defaultPool.CreateEntity();
            entity.AddComponent<SayHelloWorldComponent>();
        }
    }
}