using System;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Groups.Accessors;
using EcsRx.Systems;
using EcsRx.Views.Components;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EcsRx.Unity.Examples.ManuallyRegisterSystems.Systems
{
    public class RandomMovementSystem : IReactToGroupSystem
    {
        public IGroup TargetGroup => new Group(typeof (ViewComponent));

        public IObservable<IObservableGroup> ReactToGroup(IObservableGroup group)
        { return Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => group); }

        public void Execute(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var positionChange = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));

            var gameObject = viewComponent.View as GameObject;
            gameObject.transform.position += positionChange;
        }
    }
}