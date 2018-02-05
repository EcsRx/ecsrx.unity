using System;
using EcsRx.Groups.Accessors;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace Assets.EcsRx.Examples.ManualSystems.Systems
{
    public class ConstantOutputSystem : IManualSystem
    {
        // Empty group will match nothing
        public Group TargetGroup { get { return new EmptyGroup(); } }

        private IDisposable _updateLoop;
        private float _timesOutputted = 0;

        public void StartSystem(IGroupAccessor @group)
        {
            _updateLoop = Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(x =>
            {
                Debug.Log("Outputting: " + _timesOutputted++);
            });
        }

        public void StopSystem(IGroupAccessor @group)
        {
            _updateLoop.Dispose();
            _timesOutputted = 0;
        }
    }
}