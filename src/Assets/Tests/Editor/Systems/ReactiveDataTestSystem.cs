using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Tests.Components;
using UniRx;
using UnityEngine;

namespace EcsRx.Tests.Systems
{
    public class ReactiveDataTestSystem : IReactToDataSystem<float>
    {
        public IGroup TargetGroup { get { return new Group().WithComponent<TestComponentOne>();} }

        public IObservable<float> ReactToData(IEntity entity)
        {
            return Observable.EveryUpdate().Select(x => Time.deltaTime);
        }

        public void Execute(IEntity entity, float reactionData)
        {
            throw new System.NotImplementedException();
        }
    }
}