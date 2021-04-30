using EcsRx.Entities;
using EcsRx.Examples.CustomGameObjectHandling.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Systems;
using UnityEngine;

namespace EcsRx.Examples.CustomGameObjectHandling.Systems
{
    public class CameraFollowSystem : ISetupSystem, IBasicEntitySystem
    {
        public IGroup Group => new GroupBuilder()
            .WithComponent<CameraFollowsComponent>()
            .WithComponent<CustomViewComponent>()
            .Build();

        public void Setup(IEntity entity)
        {
            var cameraFollows = entity.GetComponent<CameraFollowsComponent>();
            cameraFollows.Camera = Camera.main;
        }

        public void Process(IEntity entity)
        {
            var entityPosition = entity.GetComponent<CustomViewComponent>().CustomView.transform.position;
            var trailPosition = entityPosition + (Vector3.back*5.0f);
            trailPosition += Vector3.up*2.0f;

            var camera = entity.GetComponent<CameraFollowsComponent>().Camera;
            camera.transform.position = trailPosition;
            camera.transform.LookAt(entityPosition);
        }
    }
}