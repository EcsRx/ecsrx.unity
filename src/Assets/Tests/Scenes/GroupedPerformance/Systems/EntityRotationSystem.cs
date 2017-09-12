using System;
using Assets.Tests.Scenes.GroupedPerformance.Components;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;

namespace Assets.Tests.Scenes.GroupedPerformance.Systems
{
    public class EntityRotationSystem : IReactToEntitySystem
    {
        public IGroup TargetGroup { get { return new Group(typeof(ViewComponent), typeof(RotationComponent)); } }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            return Observable.EveryUpdate().Select(x => entity);
        }

        public void Execute(IEntity entity)
        {
            var rotationComponent = entity.GetComponent<RotationComponent>();
            var viewComponent = entity.GetComponent<ViewComponent>();

            var rotation = rotationComponent.RotationSpeed * Time.deltaTime;
            viewComponent.View.transform.Rotate(0, rotation, 0);
        }
    }
}