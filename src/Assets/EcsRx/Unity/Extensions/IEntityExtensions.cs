using System;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Views.Components;
using UnityEngine;

namespace EcsRx.Unity.Extensions
{
    public static class IEntityExtensions
    {
        public static T GetUnityComponent<T>(this IEntity entity) where T : MonoBehaviour
        {
            if(!entity.HasComponent<ViewComponent>())
            { return null; }

            var viewComponent = entity.GetComponent<ViewComponent>();

            if(viewComponent.View == null)
            { return null; }

            var castView = (GameObject) viewComponent.View;
            return castView.GetComponent<T>();
        }

        public static T AddUnityComponent<T>(this IEntity entity) where T : MonoBehaviour
        {
            if (!entity.HasComponent<ViewComponent>())
            { throw new Exception("Entity has no ViewComponent, ensure a valid ViewComponent is applied with an active View"); }

            var viewComponent = entity.GetComponent<ViewComponent>();

            if (viewComponent.View == null)
            { throw new Exception("Entity's ViewComponent has no assigned View, GameObject has been applied to the View"); }

            var castView = (GameObject) viewComponent.View;
            return castView.AddComponent<T>();
        }
    }
}