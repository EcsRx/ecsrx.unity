using Assets.Examples.AutoRegisterSystems.Components;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using UnityEngine;

namespace Assets.Examples.AutoRegisterSystems.Systems
{
    public class ViewSetupSystem : ISetupSystem
    {
        public IGroup TargetGroup { get { return new Group(typeof (ViewComponent)); } }

        public void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            viewComponent.GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            viewComponent.GameObject.name = "entity-" + entity.Id;
        }
    }
}