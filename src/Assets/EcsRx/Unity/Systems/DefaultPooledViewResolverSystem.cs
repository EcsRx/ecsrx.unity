using Assets.EcsRx.Unity.ViewPooling;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Pools;
using EcsRx.Unity.MonoBehaviours;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Systems
{
    public abstract class DefaultPooledViewResolverSystem : PooledViewResolverSystem
    {
        public IViewPool ViewPool { get; private set; }
        
        protected DefaultPooledViewResolverSystem(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator) : 
            base(poolManager, eventSystem, instantiator)
        {
            ViewPool = new ViewPool(instantiator, PrefabTemplate);
        }
        
        protected override void RecycleView(GameObject viewToRecycle)
        {
            viewToRecycle.transform.parent = null;

            var entityView = viewToRecycle.GetComponent<EntityView>();
            entityView.Entity = null;
            entityView.Pool = null;
            ViewPool.ReleaseInstance(viewToRecycle);
        }

        protected override GameObject AllocateView(IEntity entity, IPool pool)
        {
            var viewToAllocate = ViewPool.AllocateInstance();
            var entityView = viewToAllocate.GetComponent<EntityView>();
            entityView.Entity = entity;
            entityView.Pool = pool;
            viewToAllocate.SetActive(true);
            return viewToAllocate;
        }
    }
}