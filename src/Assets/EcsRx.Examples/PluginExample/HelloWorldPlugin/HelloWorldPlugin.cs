using System;
using System.Collections.Generic;
using EcsRx.Examples.PluginExample.HelloWorldPlugin.systems;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Infrastructure.Plugins;
using EcsRx.Systems;

namespace EcsRx.Examples.PluginExample.HelloWorldPlugin
{
    public class HelloWorldPlugin : IEcsRxPlugin
    {
        public string Name => "Hello World Plugin";
        public Version Version => new Version(1, 0, 0);

        public void SetupDependencies(IDependencyContainer container)
        {
            container.Bind<OutputHelloWorldSystem>();
        }

        public IEnumerable<ISystem> GetSystemsForRegistration(IDependencyContainer container)
        {
            return new[]
            {
                container.Resolve<OutputHelloWorldSystem>()
            };
        }
    }
}