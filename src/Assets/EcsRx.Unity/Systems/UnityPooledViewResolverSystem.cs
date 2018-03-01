using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Pools;
using EcsRx.Unity.Handlers;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Views.Pooling;
using EcsRx.Views.Systems;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Systems
{
    public abstract class UnityPooledViewResolverSystem : PooledViewResolverSystem
    {
        public IInstantiator Instantiator { get; }
        public IPoolManager PoolManager { get; }

        public override IViewPool ViewPool { get; }
        
        protected abstract GameObject PrefabTemplate { get; }
        protected abstract int PoolIncrementSize { get; }

        protected IViewPool CreateViewPool()
        { return new ViewPool(PoolIncrementSize, new GameObjectViewHandler(Instantiator, PrefabTemplate)); }

        protected UnityPooledViewResolverSystem(IInstantiator instantiator, IPoolManager poolManager)
        {
            Instantiator = instantiator;
            PoolManager = poolManager;
            ViewPool = CreateViewPool();
        }
        
        protected abstract void OnPoolStarting();
        protected abstract void OnViewAllocated(GameObject view, IEntity entity);
        protected abstract void OnViewRecycled(GameObject view);

        protected override void OnViewRecycled(object view)
        {
            var gameObject = view as GameObject;
            gameObject.transform.parent = null;

            var entityView = gameObject.GetComponent<EntityView>();
            entityView.Entity = null;
            entityView.Pool = null;

            OnViewRecycled(gameObject);
        }

        protected override void OnViewAllocated(object view, IEntity entity)
        {
            var gameObject = view as GameObject;
            var entityView = gameObject.GetComponent<EntityView>();
            var containingPool = PoolManager.GetContainingPoolFor(entity);
            entityView.Entity = entity;
            entityView.Pool = containingPool;

            OnViewAllocated(gameObject, entity);
        }
    }
}