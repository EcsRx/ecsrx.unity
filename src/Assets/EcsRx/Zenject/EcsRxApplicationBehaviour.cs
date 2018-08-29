using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Collections;
using EcsRx.Events;
using EcsRx.Executor;
using EcsRx.Extensions;
using EcsRx.Infrastructure;
using EcsRx.Infrastructure.Dependencies;
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
        public IDependencyContainer DependencyContainer { get; private set; }
        
        public ISystemExecutor SystemExecutor { get; private set; }
        public IEventSystem EventSystem { get; private set; }
        public IEntityCollectionManager CollectionManager { get; private set; }
        public List<IEcsRxPlugin> Plugins { get; } = new List<IEcsRxPlugin>();
        
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
            DependencyContainer = new ZenjectDependencyContainer(_sceneContext.Container);
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

            SystemExecutor = DependencyContainer.Resolve<ISystemExecutor>();
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
    }
}