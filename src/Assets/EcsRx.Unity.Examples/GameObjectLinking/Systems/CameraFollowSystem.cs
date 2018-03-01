using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Views.Components;
using UnityEngine;

namespace EcsRx.Unity.Examples.GameObjectLinking.Systems
{
    public class ChangeScaleOnLinkingSystem : ISetupSystem
    {
        public IGroup TargetGroup { get; } = new Group(x => {
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