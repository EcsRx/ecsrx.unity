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
            if(viewComponent.View != null) { return; }

            var viewObject = ResolveView(entity);
            viewComponent.View = viewObject;

            IPool containingPool;

            var entityBinding = viewObject.GetComponent<EntityBinding>();
            if (entityBinding == null)
            {
                entityBinding = viewObject.AddComponent<EntityBinding>();
                entityBinding.Entity = entity;

                containingPool = PoolManager.GetContainingPoolFor(entity);
                entityBinding.PoolName = containingPool.Name;
            }
            else
            { containingPool = PoolManager.GetPool(entityBinding.PoolName); }

            if (viewComponent.DestroyWithView)
            {
                viewObject.OnDestroyAsObservable()
                    .Subscribe(x => containingPool.RemoveEntity(entity))
                    .AddTo(viewObject);
            }
        }
    }
}