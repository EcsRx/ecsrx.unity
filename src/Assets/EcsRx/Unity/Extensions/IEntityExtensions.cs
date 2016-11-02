using System;
using EcsRx.Entities;
using EcsRx.Unity.Components;
using UnityEngine;

namespace Assets.EcsRx.Unity.Extensions
{
    public static class IEntityExtensions
    {
        public static T GetComponent<T>(this IEntity entity) where T : MonoBehaviour
        {
            if(!entity.HasComponent<ViewComponent>())
            { return null; }

            var viewComponent = entity.GetComponent<ViewComponent>();

            if(!viewComponent.View)
            { return null; }

            return viewComponent.View.GetComponent<T>();
        }

        public static T AddComponent<T>(this IEntity entity) where T : MonoBehaviour
        {
            if (!entity.HasComponent<ViewComponent>())
            { throw new Exception("Entity has no ViewComponent, ensure a valid ViewComponent is applied with an active View"); }

            var viewComponent = entity.GetComponent<ViewComponent>();

            if (!viewComponent.View)
            { throw new Exception("Entity's ViewComponent has no assigned View, GameObject has been applied to the View"); }

            return viewComponent.View.AddComponent<T>();
        }
    }
}