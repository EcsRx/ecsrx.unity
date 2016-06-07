using Assets.Examples.PluginExample.HelloWorldPlugin.components;
using EcsRx.Unity;

namespace Assets.Examples.PluginExample
{
    public class AppContainer : EcsRxContainer
    {
        public AppContainer()
        {
            RegisterPlugin(new HelloWorldPlugin.HelloWorldPlugin());
        }

        protected override void SetupSystems() { }

        protected override void SetupEntities()
        {
            var defaultPool = PoolManager.GetPool();
            var entity = defaultPool.CreateEntity();
            entity.AddComponent<SayHelloWorldComponent>();
        }
    }
}