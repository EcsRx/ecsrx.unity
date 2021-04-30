using EcsRx.Entities;
using EcsRx.Examples.SimpleMovement.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Plugins.Views.Components;
using EcsRx.Systems;
using UnityEngine;

namespace EcsRx.Examples.SimpleMovement.Systems
{
    public class CameraFollowSystem : ISetupSystem, IBasicEntitySystem
    {
        public IGroup Group => new GroupBuilder()
            .WithComponent<CameraFollowsComponent>()
            .WithComponent<ViewComponent>()
            .Build();

        public void Setup(IEntity entity)
        {
            var cameraFollows = entity.GetComponent<CameraFollowsComponent>();
            cameraFollows.Camera = Camera.main;
        }

        public void Process(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var view = viewComponent.View as GameObject;
            var entityPosition = view.transform.position;
            var trailPosition = entityPosition + (Vector3.back*5.0f);
            trailPosition += Vector3.up*2.0f;

            var camera = entity.GetComponent<CameraFollowsComponent>().Camera;
            camera.transform.position = trailPosition;
            camera.transform.LookAt(entityPosition);
        }
    }
}