using EcsRx.Collections;
using EcsRx.Collections.Database;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Handlers;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Plugins.Views.Components;
using EcsRx.Plugins.Views.Systems;
using EcsRx.Plugins.Views.ViewHandlers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace EcsRx.Unity.Systems
{
    public abstract class PrefabViewResolverSystem : ViewResolverSystem
    {
        public IEntityDatabase EntityDatabase { get; }
        public IUnityInstantiator Instantiator { get; }

        protected abstract GameObject PrefabTemplate { get; }

        protected PrefabViewResolverSystem(IEntityDatabase entityDatabase, IEventSystem eventSystem, IUnityInstantiator instantiator) : base(eventSystem)
        {
            EntityDatabase = entityDatabase;
            Instantiator = instantiator;
            ViewHandler = CreateViewHandler();
        }

        protected IViewHandler CreateViewHandler()
        { return new PrefabViewHandler(Instantiator, PrefabTemplate); }

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

                entityBinding.EntityCollection = EntityDatabase.GetCollectionFor(entity);
            }

            if (viewComponent.DestroyWithView)
            {
                gameObject.OnDestroyAsObservable()
                    .Subscribe(x => entityBinding.EntityCollection.RemoveEntity(entity.Id))
                    .AddTo(gameObject);
            }
        }
    }
}