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
using Zenject;

namespace EcsRx.Unity.Systems
{
    public abstract class ViewResolverSystem : ISetupSystem
    {
        public IPoolManager PoolManager { get; private set; }
        public IInstantiator Instantiator { get; private set; }

        public virtual IGroup TargetGroup
        {
            get {  return new Group(typeof(ViewComponent)); }
        }

        protected ViewResolverSystem(IPoolManager poolManager, IInstantiator instantiator)
        {
            PoolManager = poolManager;
            Instantiator = instantiator;
        }

        protected GameObject InstantiateAndInject(GameObject prefab,
            Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion))
        {
            var createdPrefab = Instantiator.InstantiatePrefab(prefab);
            createdPrefab.transform.position = position;
            createdPrefab.transform.rotation = rotation;
            return createdPrefab;
        }

        public abstract GameObject ResolveView(IEntity entity);
        
        public virtual void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            if(viewComponent.View != null) { return; }

            var viewObject = ResolveView(entity);
            viewComponent.View = viewObject;

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
    }
}