using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Collections;
using EcsRx.Events;
using EcsRx.Executor;
using EcsRx.Extensions;
using EcsRx.Infrastructure;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Infrastructure.Modules;
using EcsRx.Infrastructure.Plugins;
using EcsRx.Zenject.Dependencies;
using UnityEngine;
using Zenject;

namespace EcsRx.Zenject
{
    [DefaultExecutionOrder(-20000)]
    public abstract class EcsRxApplicationBehaviour : MonoBehaviour, IEcsRxApplication
    {
        public IDependencyContainer Container { get; private set; }
        
        public ISystemExecutor SystemExecutor { get; private set; }
        public IEventSystem EventSystem { get; private set; }
        public IEntityCollectionManager EntityCollectionManager { get; private set; }
        public IEnumerable<IEcsRxPlugin> Plugins => _plugins;
        
        private List<IEcsRxPlugin> _plugins { get; } = new List<IEcsRxPlugin>();
        
        private SceneContext _sceneContext;

        private void Awake()
        {
            var sceneContexts = FindObjectsOfType<SceneContext>();
            _sceneContext = sceneContexts.FirstOrDefault();
            
            if(_sceneContext == null) 
            { throw new Exception("Cannot find SceneContext, please make sure one is on the scene"); }
            
            _sceneContext.PostInstall += OnZenjectReady;
        }

        protected void OnZenjectReady()
        {   
            Container = new ZenjectDependencyContainer(_sceneContext.Container);
            StartApplication();
        }

        public virtual void StartApplication()
        {
            RegisterModules();
            _sceneContext.Container.Inject(this);
            
            ApplicationStarting();
            RegisterAllPluginDependencies();
            SetupAllPluginSystems();
            ApplicationStarted();
        }

        protected virtual void RegisterModules()
        {
            Container.LoadModule<FrameworkModule>();

            SystemExecutor = Container.Resolve<ISystemExecutor>();
            EventSystem = Container.Resolve<IEventSystem>();
            EntityCollectionManager = Container.Resolve<IEntityCollectionManager>();
        }

        protected virtual void ApplicationStarting() { }
        protected abstract void ApplicationStarted();

        protected virtual void RegisterAllPluginDependencies()
        { _plugins.ForEachRun(x => x.SetupDependencies(Container)); }

        protected virtual void SetupAllPluginSystems()
        {
            _plugins.SelectMany(x => x.GetSystemsForRegistration(Container))
                .ForEachRun(x => SystemExecutor.AddSystem(x));
        }

        protected void RegisterPlugin(IEcsRxPlugin plugin)
        { _plugins.Add(plugin); }
    }
}