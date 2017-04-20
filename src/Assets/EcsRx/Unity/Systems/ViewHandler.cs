using System;
using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Pools;
using EcsRx.Unity.Components;
using EcsRx.Unity.MonoBehaviours;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EcsRx.Unity.Systems
{
    public class ViewHandler : IViewHandler, IDisposable
    {
        public IPoolManager PoolManager { get; private set; }
        public IEventSystem EventSystem { get; private set; }
        public IInstantiator Instantiator { get; private set; }

        private readonly IDisposable _destructionSubscription;
        private readonly IDictionary<Guid, GameObject> _viewCache;

        protected ViewHandler(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator)
        {
            PoolManager = poolManager;
            EventSystem = eventSystem;
            Instantiator = instantiator;

            _viewCache = new Dictionary<Guid, GameObject>();

            _destructionSubscription = EventSystem.Receive<ComponentRemovedEvent>()
                .Where(x => x.Component is ViewComponent && _viewCache.ContainsKey(x.Entity.Id))
                .Subscribe(x =>
                {
                    var view = _viewCache[x.Entity.Id];
                    _viewCache.Remove(x.Entity.Id);
                    DestroyView(view);
                });
        }

        public virtual GameObject InstantiateAndInject(GameObject prefab,
            Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion))
        {
            var createdPrefab = Instantiator.InstantiatePrefab(prefab);
            createdPrefab.transform.position = position;
            createdPrefab.transform.rotation = rotation;
            return createdPrefab;
        }

        public virtual void DestroyView(GameObject view)
        { Object.Destroy(view); }
        
        public virtual void SetupView(IEntity entity, Func<IEntity, GameObject> viewResolver)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            if (viewComponent.View != null) { return; }

            var viewObject = viewResolver(entity);
            viewComponent.View = viewObject;

            _viewCache.Add(entity.Id, viewObject);

            var entityBinding = viewObject.GetComponent<EntityView>();
            if (entityBinding == null)
            {
                entityBinding = viewObject.AddComponent<EntityView>();
                entityBinding.Entity = entity;

                entityBinding.Pool = PoolManager.GetContainingPoolFor(entity);
            }

            if (viewComponent.DestroyWithView)
            {
                viewObject.OnDestroyAsObservable()
                    .Subscribe(x => entityBinding.Pool.RemoveEntity(entity))
                    .AddTo(viewObject);
            }
        }

        public void Dispose()
        { _destructionSubscription.Dispose(); }
    }
}