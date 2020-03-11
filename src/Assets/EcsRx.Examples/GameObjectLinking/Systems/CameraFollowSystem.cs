using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Plugins.Views.Components;
using UnityEngine;

namespace EcsRx.Examples.GameObjectLinking.Systems
{
    public class ChangeScaleOnLinkingSystem : ISetupSystem
    {
        public IGroup Group { get; } = new GroupWithPredicate(x => {
            var viewComponent = x.GetComponent<ViewComponent>();
            return viewComponent.View != null;
        }, typeof(ViewComponent));

        public void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var view = viewComponent.View as GameObject;
            view.transform.localScale = Vector3.one*3;
        }
    }
}