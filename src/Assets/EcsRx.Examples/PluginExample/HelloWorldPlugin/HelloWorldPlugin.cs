using System;
using System.Collections.Generic;
using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.Extensions;
using SystemsRx.Infrastructure.Plugins;
using SystemsRx.Systems;
using EcsRx.Examples.PluginExample.HelloWorldPlugin.systems;

namespace EcsRx.Examples.PluginExample.HelloWorldPlugin
{
    public class HelloWorldPlugin : ISystemsRxPlugin
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