using System;
using System.Collections.Generic;
using EcsRx.Systems;
using EcsRx.Systems.Executor;
using Zenject;

namespace EcsRx.Unity.Plugins
{
    public interface IEcsRxPlugin
    {
        string Name { get; }
        Version Version { get; }

        void SetupDependencies(DiContainer container);
        IEnumerable<ISystem> GetSystemForRegistration(DiContainer container);
    }
}