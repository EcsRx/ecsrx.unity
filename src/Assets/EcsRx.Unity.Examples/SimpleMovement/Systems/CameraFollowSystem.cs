using System;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Groups.Accessors;
using EcsRx.Systems;
using EcsRx.Unity.Examples.SimpleMovement.Components;
using EcsRx.Views.Components;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Examples.SimpleMovement.Systems
{
    public class CameraFollowSystem : ISetupSystem, IReactToGroupSystem
    {
        public IGroup TargetGroup => new GroupBuilder()
            .WithComponent<CameraFollowsComponent>()
            .WithComponent<ViewComponent>()
            .Build();

        public void Setup(IEntity entity)
        {
            var cameraFollows = entity.GetComponent<CameraFollowsComponent>();
            cameraFollows.Camera = Camera.main;
        }

        public IObservable<IObservableGroup> ReactToGroup(IObservableGroup group)
        { return Observable.EveryUpdate().Select(x => group); }

        public void Execute(IEntity entity)
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