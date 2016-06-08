using System;
using System.Collections.Generic;
using Assets.EcsRx.Examples.PluginExample.HelloWorldPlugin.systems;
using EcsRx.Systems;
using EcsRx.Unity.Plugins;
using Zenject;

namespace Assets.EcsRx.Examples.PluginExample.HelloWorldPlugin
{
    public class HelloWorldPlugin : IEcsRxPlugin
    {
        public string Name { get { return "Hello World Plugin";  } }
        public Version Version { get { return new Version(1,0,0); } }

        public void SetupDependencies(DiContainer container)
        {
            container.Bind<OutputHelloWorldSystem>().AsSingle();
        }

        public IEnumerable<ISystem> GetSystemForRegistration(DiContainer container)
        {
            return new[]
            {
                container.Resolve<OutputHelloWorldSystem>()
            };
        }
    }
}