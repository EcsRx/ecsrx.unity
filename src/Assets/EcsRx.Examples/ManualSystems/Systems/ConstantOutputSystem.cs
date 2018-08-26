using System;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace EcsRx.Examples.ManualSystems.Systems
{
    public class ConstantOutputSystem : IManualSystem
    {
        // Empty group will match nothing
        public IGroup Group => new EmptyGroup();

        private IDisposable _updateLoop;
        private float _timesOutputted = 0;

        public void StartSystem(IObservableGroup group)
        {
            _updateLoop = Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(x =>
            {
                Debug.Log("Outputting: " + _timesOutputted++);
            });
        }

        public void StopSystem(IObservableGroup group)
        {
            _updateLoop.Dispose();
            _timesOutputted = 0;
        }
    }
}