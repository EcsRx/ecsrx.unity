using EcsRx.Examples.PluginExample.HelloWorldPlugin.components;
using EcsRx.Extensions;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Zenject;

namespace EcsRx.Examples.PluginExample
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void LoadPlugins()
        {
            base.LoadPlugins();
            RegisterPlugin(new HelloWorldPlugin.HelloWorldPlugin());
        }
        
        protected override void ApplicationStarted()
        {
            var defaultPool = EntityDatabase.GetCollection();
            var entity = defaultPool.CreateEntity();
            entity.AddComponent<SayHelloWorldComponent>();
        }
    }
}