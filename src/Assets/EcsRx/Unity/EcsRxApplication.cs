using System.Collections.Generic;
using System.Linq;
using EcsRx.Extensions;
using UnityEngine;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Systems.Executor;
using EcsRx.Unity.Plugins;
using EcsRx.Unity.Systems;
using Zenject;

namespace EcsRx.Unity
{
    public abstract class EcsRxApplication : MonoBehaviour
    {
        [Inject]
        public ISystemExecutor SystemExecutor { get; private set; }

        [Inject]
        public IPoolManager PoolManager { get; private set; }

        protected List<IEcsRxPlugin> Plugins { get; private set; }
        protected DiContainer Container { get; private set; }

        [Inject]
        private void Init(DiContainer container)
        {
            Plugins = new List<IEcsRxPlugin>();
            Container = container;
            ApplicationStarting();
            RegisterAllPluginDependencies();
            SetupAllPluginSystems();
            ApplicationStarted();
        }

        protected virtual void ApplicationStarting() { }
        protected abstract void ApplicationStarted();

        protected virtual void RegisterAllPluginDependencies()
        { Plugins.ForEachRun(x => x.SetupDependencies(Container)); }

        protected virtual void SetupAllPluginSystems()
        {
            Plugins.SelectMany(x => x.GetSystemForRegistration(Container))
                .ForEachRun(x => SystemExecutor.AddSystem(x));
        }

        protected void RegisterPlugin(IEcsRxPlugin plugin)
        { Plugins.Add(plugin); }
        
        protected virtual void RegisterAllBoundSystems()
        {
            var allSystems = Container.ResolveAll<ISystem>();

            var orderedSystems = allSystems
                .OrderByDescending(x => x is ViewResolverSystem)
                .ThenByDescending(x => x is ISetupSystem);
            orderedSystems.ForEachRun(SystemExecutor.AddSystem);
        }

        protected virtual void RegisterBoundSystem<T>() where T : ISystem
        {
            var system = Container.Resolve<T>();
            SystemExecutor.AddSystem(system);
        }
    }
}
