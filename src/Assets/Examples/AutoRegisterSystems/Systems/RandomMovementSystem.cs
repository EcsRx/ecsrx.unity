using System;
using Assets.Examples.AutoRegisterSystems.Components;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Examples.AutoRegisterSystems.Systems
{
    public class RandomMovementSystem : IReactToGroupSystem
    {
        public IGroup TargetGroup { get { return new Group(typeof (ViewComponent)); } }

        public IObservable<GroupAccessor> ReactToGroup(GroupAccessor @group)
        {
            return Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => @group);
        }

        public void Execute(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var positionChange = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            viewComponent.GameObject.transform.position += positionChange;
        }
    }
}