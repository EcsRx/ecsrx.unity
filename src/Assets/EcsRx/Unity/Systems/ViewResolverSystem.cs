using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using EcsRx.Unity.MonoBehaviours;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace EcsRx.Unity.Systems
{
    public abstract class ViewResolverSystem : ISetupSystem
    {
        public IPoolManager PoolManager { get; private set; }

        public virtual IGroup TargetGroup
        {
            get {  return new Group(typeof(ViewComponent)); }
        }

        protected ViewResolverSystem(IPoolManager poolManager)
        {
            PoolManager = poolManager;
        }

        public abstract GameObject ResolveView(IEntity entity);

        public virtual void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();

            var viewObject = ResolveView(entity);
            viewComponent.View = viewObject;

            var entityBinding = viewObject.GetComponent<EntityBinding>();
            if (entityBinding == null)
            {
                entityBinding = viewObject.AddComponent<EntityBinding>();
                entityBinding.Entity = entity;

                var containingPool = PoolManager.GetContainingPoolFor(entity);
                entityBinding.PoolName = containingPool.Name;
            }

            viewObject.OnDestroyAsObservable()
                .Subscribe(x => CleanupEntity(entity))
                .Dispose();
        }

        protected virtual void CleanupEntity(IEntity entity)
        {
            var pool = PoolManager.GetPool();
            pool.RemoveEntity(entity);
        }

        protected virtual void CleanupView()
    }
}