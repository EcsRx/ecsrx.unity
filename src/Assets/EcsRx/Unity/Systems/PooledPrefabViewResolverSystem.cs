using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Unity.Handlers;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Views.Pooling;
using EcsRx.Views.Systems;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Systems
{
    public abstract class PooledPrefabViewResolverSystem : PooledViewResolverSystem
    {
        public IInstantiator Instantiator { get; }
        public IEntityCollectionManager CollectionManager { get; }
        
        protected abstract GameObject PrefabTemplate { get; }
        protected abstract int PoolIncrementSize { get; }

        protected override IViewPool CreateViewPool()
        { return new ViewPool(PoolIncrementSize, new PrefabViewHandler(Instantiator, PrefabTemplate)); }

        protected PooledPrefabViewResolverSystem(IInstantiator instantiator, IEntityCollectionManager collectionManager, IEventSystem eventSystem) : base(eventSystem) 
        {
            Instantiator = instantiator;
            CollectionManager = collectionManager;            
        }

        protected abstract void OnViewAllocated(GameObject view, IEntity entity);
        protected abstract void OnViewRecycled(GameObject view, IEntity entity);

        protected override void OnViewRecycled(object view, IEntity entity)
        {
            var gameObject = view as GameObject;
            gameObject.transform.parent = null;

            var entityView = gameObject.GetComponent<EntityView>();
            entityView.Entity = null;
            entityView.EntityCollection = null;

            OnViewRecycled(gameObject, entity);
        }

        protected override void OnViewAllocated(object view, IEntity entity)
        {
            var gameObject = view as GameObject;
            var entityView = gameObject.GetComponent<EntityView>();
            var entityCollection = CollectionManager.GetCollectionFor(entity);
            entityView.Entity = entity;
            entityView.EntityCollection = entityCollection;

            OnViewAllocated(gameObject, entity);
        }
    }
}