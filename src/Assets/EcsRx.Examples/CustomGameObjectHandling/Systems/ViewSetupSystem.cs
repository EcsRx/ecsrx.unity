using Assets.EcsRx.Examples.CustomGameObjectHandling.Components;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UnityEngine;

namespace Assets.EcsRx.Examples.CustomGameObjectHandling.Systems
{
    public class ViewSetupSystem : ISetupSystem
    {
        public IGroup TargetGroup { get { return new Group().WithComponent<CustomViewComponent>();} }
        
        public void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            viewComponent.View = GameObject.CreatePrimitive(PrimitiveType.Cube);
            viewComponent.View.name = "entity-" + entity.Id;
            var rigidBody = viewComponent.View.AddComponent<Rigidbody>();
            rigidBody.freezeRotation = true;
        }
    }
}