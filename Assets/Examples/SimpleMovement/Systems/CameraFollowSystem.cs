using Assets.Examples.SimpleMovement.Components;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace Assets.Examples.SimpleMovement.Systems
{
    public class CameraFollowSystem : ISetupSystem, IReactToGroupSystem
    {
        public IGroup TargetGroup
        {
            get
            {
                return new GroupBuilder()
                    .WithComponent<CameraFollowsComponent>()
                    .WithComponent<ViewComponent>()
                    .Build();
            }
        }

        public void Setup(IEntity entity)
        {
            var cameraFollows = entity.GetComponent<CameraFollowsComponent>();
            cameraFollows.Camera = Camera.main;
        }

        public IObservable<GroupAccessor> ReactToGroup(GroupAccessor @group)
        {
            return Observable.EveryUpdate().Select(x => @group);
        }

        public void Execute(IEntity entity)
        {
            var entityPosition = entity.GetComponent<ViewComponent>().GameObject.transform.position;
            var trailPosition = entityPosition + (Vector3.back*5.0f);
            trailPosition += Vector3.up*2.0f;

            var camera = entity.GetComponent<CameraFollowsComponent>().Camera;
            camera.transform.position = trailPosition;
            camera.transform.LookAt(entityPosition);
        }
    }
}