using Assets.Examples.SimpleMovement.Components;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using UnityEngine;

namespace Assets.Examples.SimpleMovement.Systems
{
    public class ViewSetupSystem : ISetupSystem
    {
        public IGroup TargetGroup { get { return new Group().WithComponent<ViewComponent>();} }
        
        public void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            viewComponent.GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            viewComponent.GameObject.name = "entity-" + entity.Id;
            var rigidBody = viewComponent.GameObject.AddComponent<Rigidbody>();
            rigidBody.freezeRotation = true;
        }
    }
}