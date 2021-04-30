using System;
using SystemsRx.Systems.Conventional;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace EcsRx.Examples.ManualSystems.Systems
{
    public class ConstantOutputSystem : IManualSystem
    {
        private IDisposable _updateLoop;
        private float _timesOutputted = 0;

        public void StartSystem()
        {
            _updateLoop = Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(x =>
            {
                Debug.Log($"Outputting: {_timesOutputted++}");
            });
        }

        public void StopSystem()
        {
            _updateLoop.Dispose();
            _timesOutputted = 0;
        }
    }
}