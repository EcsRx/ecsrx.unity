﻿using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Pools;
using EcsRx.Unity.Handlers;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Views.Components;
using EcsRx.Views.Systems;
using EcsRx.Views.ViewHandlers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Systems
{
    public abstract class UnityViewResolverSystem : ViewResolverSystem
    {
        public IPoolManager PoolManager { get; }
        public IInstantiator Instantiator { get; }

        protected abstract GameObject PrefabTemplate { get; }

        protected UnityViewResolverSystem(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator) : base(eventSystem)
        {
            PoolManager = poolManager;
            Instantiator = instantiator;
            ViewHandler = CreateViewHandler();
        }

        protected IViewHandler CreateViewHandler()
        { return new GameObjectViewHandler(Instantiator, PrefabTemplate); }

        public override IViewHandler ViewHandler { get; }

        protected override void OnViewCreated(IEntity entity, ViewComponent viewComponent)
        {
            var gameObject = viewComponent.View as GameObject;
            OnViewCreated(entity, gameObject);
        }

        protected abstract void OnViewCreated(IEntity entity, GameObject view);

        public override void Setup(IEntity entity)
        {
            base.Setup(entity);

            var viewComponent = entity.GetComponent<ViewComponent>();
            var gameObject = viewComponent.View as GameObject;
            var entityBinding = gameObject.GetComponent<EntityView>();
            if (entityBinding == null)
            {
                entityBinding = gameObject.AddComponent<EntityView>();
                entityBinding.Entity = entity;

                entityBinding.Pool = PoolManager.GetContainingPoolFor(entity);
            }

            if (viewComponent.DestroyWithView)
            {
                gameObject.OnDestroyAsObservable()
                    .Subscribe(x => entityBinding.Pool.RemoveEntity(entity))
                    .AddTo(gameObject);
            }
        }
    }
}