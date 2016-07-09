using Assets.EcsRx.Unity.ViewPooling;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using EcsRx.Unity.MonoBehaviours;
using UniRx;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Systems
{
    public abstract class PooledViewResolverSystem : ISetupSystem
    {
        public IPoolManager PoolManager { get; private set; }
        public IEventSystem EventSystem { get; private set; }
        public IInstantiator Instantiator { get; private set; }
        public IViewPool ViewPool { get; private set; }

        public virtual IGroup TargetGroup
        {
            get { return new Group(typeof(ViewComponent)); }
        }

        protected abstract GameObject ResolvePrefabTemplate();

        protected PooledViewResolverSystem(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator)
        {
            PoolManager = poolManager;
            Instantiator = instantiator;
            EventSystem = eventSystem;

            var prefab = ResolvePrefabTemplate();
            ViewPool = new ViewPool(instantiator, prefab);
        }

        protected virtual void RecycleView(GameObject viewToRecycle)
        {
            viewToRecycle.transform.parent = null;

            var entityView = viewToRecycle.GetComponent<EntityView>();
            entityView.Entity = null;
            entityView.Pool = null;
            ViewPool.ReleaseInstance(viewToRecycle);
        }

        protected virtual GameObject AllocateView(IEntity entity, IPool pool)
        {
            var viewToAllocate = ViewPool.AllocateInstance();
            var entityView = viewToAllocate.GetComponent<EntityView>();
            entityView.Entity = entity;
            entityView.Pool = pool;
            return viewToAllocate;
        }

        public virtual void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            if (viewComponent.View != null) { return; }

            var containingPool = PoolManager.GetContainingPoolFor(entity);
            var viewObject = AllocateView(entity, containingPool);
            viewComponent.View = viewObject;
            
            EventSystem.Receive<EntityRemovedEvent>()
                .First(x => x.Entity == entity)
                .Subscribe(x => RecycleView(viewObject));
        }
    }
}