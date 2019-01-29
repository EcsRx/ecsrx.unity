using System;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Plugins.Views.Components;
using UnityEngine;

namespace EcsRx.Unity.Extensions
{
    public static class GameObjectExtensions
    {
        public static void LinkEntity(this GameObject gameObject, IEntity entity, IEntityCollection withinPool)
        {
            if(gameObject.GetComponent<EntityView>())
            { throw new Exception("GameObject already has an EntityView monobehaviour applied"); }

            if (gameObject.GetComponent<RegisterAsEntity>())
            { throw new Exception("GameObject already has a RegisterAsEntity monobehaviour applied"); }

            if (!entity.HasComponent<ViewComponent>())
            { entity.AddComponent<ViewComponent>(); }

            var entityViewMb = gameObject.AddComponent<EntityView>();
            entityViewMb.Entity = entity;
            entityViewMb.EntityCollection = withinPool;
            
            var viewComponent = entity.GetComponent<ViewComponent>();
            viewComponent.View = gameObject;
        }
    }
}