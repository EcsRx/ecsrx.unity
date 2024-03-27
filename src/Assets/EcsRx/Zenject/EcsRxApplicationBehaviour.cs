using System;
using System.Linq;
using EcsRx.Unity;
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
            DependencyRegistry = new ZenjectDependencyRegistry(_sceneContext.Container);
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