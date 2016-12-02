using System;
using EcsRx.Entities;
using EcsRx.Pools;
using EcsRx.Unity.Components;
using EcsRx.Unity.MonoBehaviours;
using UnityEngine;

namespace Assets.EcsRx.Unity.Extensions
{
    public static class GameObjectExtensions
    {
        public static void LinkEntity(this GameObject gameObject, IEntity entity, IPool withinPool)
        {
            if(gameObject.GetComponent<EntityView>())
            { throw new Exception("GameObject already has an EntityView monobehaviour applied"); }

            if (gameObject.GetComponent<RegisterAsEntity>())
            { throw new Exception("GameObject already has a RegisterAsEntity monobehaviour applied"); }

            if (!entity.HasComponents(typeof(ViewComponent)))
            { entity.AddComponent<ViewComponent>(); }

            var entityViewMb = gameObject.AddComponent<EntityView>();
            entityViewMb.Entity = entity;
            entityViewMb.Pool = withinPool;
            
            var viewComponent = entity.GetComponent<ViewComponent>();
            viewComponent.View = gameObject;
        }
    }
}