using System;
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
    public class ViewHandler : IViewHandler
    {
        public IPoolManager PoolManager { get; private set; }
        public IEventSystem EventSystem { get; private set; }
        public IInstantiator Instantiator { get; private set; }

        protected ViewHandler(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator)
        {
            PoolManager = poolManager;
            EventSystem = eventSystem;
            Instantiator = instantiator;
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

            var entityBinding = viewObject.GetComponent<EntityView>();
            if (entityBinding == null)
            {
                entityBinding = viewObject.AddComponent<EntityView>();
                entityBinding.Entity = entity;

                entityBinding.Pool = PoolManager.GetContainingPoolFor(entity);
            }

            IDisposable viewSubscription = null;
            if (viewComponent.DestroyWithView)
            {
                viewSubscription = viewObject.OnDestroyAsObservable()
                    .Subscribe(x => entityBinding.Pool.RemoveEntity(entity))
                    .AddTo(viewObject);
            }

            EventSystem.Receive<ComponentRemovedEvent>()
                .First(x => x.Component is ViewComponent && x.Entity == entity)
                .Subscribe(x =>
                {
                    if (viewSubscription != null)
                    { viewSubscription.Dispose(); }

                    DestroyView(viewObject);
                });
        }
    }
}