using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UnityEngine;

namespace Assets.EcsRx.Examples.GameObjectLinking.Systems
{
    public class ChangeScaleOnLinkingSystem : ISetupSystem
    {
        public IGroup TargetGroup { get { return new Group(typeof(ViewComponent));  } }

        public void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            viewComponent.View.transform.localScale = Vector3.one*3;
        }
    }
}