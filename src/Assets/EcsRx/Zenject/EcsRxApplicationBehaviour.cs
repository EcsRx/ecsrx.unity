using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Collections;
using EcsRx.Collections.Database;
using EcsRx.Events;
using EcsRx.Executor;
using EcsRx.Extensions;
using EcsRx.Infrastructure;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Infrastructure.Modules;
using EcsRx.Infrastructure.Plugins;
using EcsRx.Plugins.Batching;
using EcsRx.Plugins.Computeds;
using EcsRx.Plugins.ReactiveSystems;
using EcsRx.Plugins.Views;
using EcsRx.Systems;
using EcsRx.Unity;
using EcsRx.Unity.Modules;
using EcsRx.Zenject.Dependencies;
using UnityEngine;
using Zenject;

namespace EcsRx.Zenject
{
    [DefaultExecutionOrder(-20000)]
    public abstract class EcsRxApplicationBehaviour : UnityEcsRxApplicationBehaviour
    {
        private SceneContext _sceneContext;
        
        private void Awake()
        {
            var sceneContexts = FindObjectsOfType<SceneContext>();
            _sceneContext = sceneContexts.FirstOrDefault();
            
            if(_sceneContext == null) 
            { throw new Exception("Cannot find SceneContext, please make sure one is on the scene"); }
            
            _sceneContext.PostInstall += OnZenjectReady;
        }

        /// <summary>
        /// Once the application has loaded get zenject container and whack it into our container
        /// </summary>
        protected void OnZenjectReady()
        {   
            Container = new ZenjectDependencyContainer(_sceneContext.Container);
            StartApplication();
        }
        
        /// <summary>
        /// Resolve any dependencies the application needs
        /// </summary>
        /// <remarks>By default it will setup SystemExecutor, EventSystem, EntityCollectionManager</remarks>
        protected override void ResolveApplicationDependencies()
        {
            base.ResolveApplicationDependencies();
            _sceneContext.Container.Inject(this);
        }
    }
}