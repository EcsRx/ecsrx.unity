using System;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Plugins.Views.Components;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EcsRx.Examples.ManuallyRegisterSystems.Systems
{
    public class RandomMovementSystem : IReactToGroupSystem
    {
        public IGroup Group => new Group(typeof (ViewComponent));

        public IObservable<IObservableGroup> ReactToGroup(IObservableGroup group)
        { return Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => group); }

        public void Process(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var positionChange = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));

            var gameObject = viewComponent.View as GameObject;
            gameObject.transform.position += positionChange;
        }
    }
}