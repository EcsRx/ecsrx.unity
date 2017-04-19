using System;
using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Systems
{
    public abstract class PooledViewResolverSystem : ISetupSystem, IDisposable
    {
        public IPoolManager PoolManager { get; private set; }
        public IEventSystem EventSystem { get; private set; }
        public IInstantiator Instantiator { get; private set; }

        protected GameObject PrefabTemplate { get; set; }

        private readonly IDictionary<Guid, GameObject> _viewCache;
        private readonly IDisposable _entitySubscription;

        public virtual IGroup TargetGroup
        {
            get { return new Group(typeof(ViewComponent)); }
        }
        
        protected PooledViewResolverSystem(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator)
        {
            PoolManager = poolManager;
            Instantiator = instantiator;
            EventSystem = eventSystem;

            _viewCache = new Dictionary<Guid, GameObject>();

            _entitySubscription = EventSystem
                .Receive<ComponentRemovedEvent>()
                .Where(x => x.Component is ViewComponent && _viewCache.ContainsKey(x.Entity.Id))
                .Subscribe(x =>
                {
                    var view = _viewCache[x.Entity.Id];
                    RecycleView(view);
                    _viewCache.Remove(x.Entity.Id);
                });

            PrefabTemplate = ResolvePrefabTemplate();
        }

        protected abstract GameObject ResolvePrefabTemplate();
        protected abstract void RecycleView(GameObject viewToRecycle);
        protected abstract GameObject AllocateView(IEntity entity, IPool pool);

        public virtual void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            if (viewComponent.View != null) { return; }

            var containingPool = PoolManager.GetContainingPoolFor(entity);
            var viewObject = AllocateView(entity, containingPool);
            viewComponent.View = viewObject;

            _viewCache.Add(entity.Id, viewObject);
        }

        public void Dispose()
        { _entitySubscription.Dispose(); }
    }
}