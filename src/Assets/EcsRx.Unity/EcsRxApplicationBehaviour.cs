using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Collections;
using EcsRx.Events;
using EcsRx.Executor;
using EcsRx.Executor.Handlers;
using EcsRx.Extensions;
using EcsRx.Infrastructure;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Modules;
using EcsRx.Infrastructure.Plugins;
using EcsRx.Systems;
using EcsRx.Unity.Dependencies;
using EcsRx.Views.Systems;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity
{
    public abstract class EcsRxApplicationBehaviour : MonoBehaviour, IEcsRxApplication
    {
        protected IDependencyContainer DependencyContainer { get; private set; }
        
        public ISystemExecutor SystemExecutor { get; private set; }
        public IEventSystem EventSystem { get; private set; }
        public IEntityCollectionManager CollectionManager { get; private set; }
        public List<IEcsRxPlugin> Plugins { get; } = new List<IEcsRxPlugin>();

        [Inject]
        private void Init(DiContainer container)
        {
            DependencyContainer = new ZenjectDependencyContainer(container);
            StartApplication();
        }

        public virtual void StartApplication()
        {
            RegisterModules();
            ApplicationStarting();
            RegisterAllPluginDependencies();
            SetupAllPluginSystems();
            ApplicationStarted();
        }

        protected virtual void RegisterModules()
        {
            DependencyContainer.LoadModule<FrameworkModule>();

            // HACK
            var container = DependencyContainer.NativeContainer as DiContainer;
            var systems = container.ResolveAll<IConventionalSystemHandler>();
            SystemExecutor = new SystemExecutor(systems);
            // END HACK

            //SystemExecutor = DependencyContainer.Resolve<ISystemExecutor>();
            EventSystem = DependencyContainer.Resolve<IEventSystem>();
            CollectionManager = DependencyContainer.Resolve<IEntityCollectionManager>();
        }

        protected virtual void ApplicationStarting() { }
        protected abstract void ApplicationStarted();

        protected virtual void RegisterAllPluginDependencies()
        { Plugins.ForEachRun(x => x.SetupDependencies(DependencyContainer)); }

        protected virtual void SetupAllPluginSystems()
        {
            Plugins.SelectMany(x => x.GetSystemsForRegistration(DependencyContainer))
                .ForEachRun(x => SystemExecutor.AddSystem(x));
        }

        protected void RegisterPlugin(IEcsRxPlugin plugin)
        { Plugins.Add(plugin); }

        protected virtual void RegisterAllBoundSystems()
        {
            var allSystems = DependencyContainer.ResolveAll<ISystem>();

            var orderedSystems = allSystems
                .OrderByDescending(x => x is ViewResolverSystem)
                .ThenByDescending(x => x is ISetupSystem);

            orderedSystems.ForEachRun(SystemExecutor.AddSystem);
        }

        protected virtual void RegisterSystem<T>() where T : ISystem
        {
            var system = DependencyContainer.Resolve<T>();
            SystemExecutor.AddSystem(system);
        }
    }
}