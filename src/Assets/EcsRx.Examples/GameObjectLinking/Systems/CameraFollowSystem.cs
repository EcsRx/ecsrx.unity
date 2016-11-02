using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UnityEngine;

namespace Assets.EcsRx.Examples.GameObjectLinking.Systems
{
    public class ChangeScaleOnLinkingSystem : ISetupSystem
    {
        private readonly IGroup _targetSystem = new Group(x => {
            var viewComponent = x.GetComponent<ViewComponent>();
            return viewComponent.View != null;
        }, typeof(ViewComponent));

        public IGroup TargetGroup { get { return _targetSystem; } }

        public void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            viewComponent.View.transform.localScale = Vector3.one*3;
        }
    }
}