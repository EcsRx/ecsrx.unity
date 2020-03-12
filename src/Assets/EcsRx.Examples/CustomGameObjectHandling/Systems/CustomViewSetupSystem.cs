using EcsRx.Entities;
using EcsRx.Examples.CustomGameObjectHandling.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using UnityEngine;

namespace EcsRx.Examples.CustomGameObjectHandling.Systems
{
    public class CustomViewSetupSystem : ISetupSystem
    {
        public IGroup Group { get { return new Group().WithComponent<CustomViewComponent>();} }
        
        public void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<CustomViewComponent>();
            viewComponent.CustomView = GameObject.CreatePrimitive(PrimitiveType.Cube);
            viewComponent.CustomView.name = $"entity-{entity.Id}";
            var rigidBody = viewComponent.CustomView.AddComponent<Rigidbody>();
            rigidBody.freezeRotation = true;
        }
    }
}