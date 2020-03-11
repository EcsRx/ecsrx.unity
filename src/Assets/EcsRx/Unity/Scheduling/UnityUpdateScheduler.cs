using System;
using EcsRx.Scheduling;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Scheduling
{
    public class UnityUpdateScheduler : IUpdateScheduler
    {
        public ElapsedTime ElapsedTime { get; private set; }
        
        private readonly Subject<ElapsedTime> _onPreUpdate = new Subject<ElapsedTime>();
        private readonly Subject<ElapsedTime> _onUpdate = new Subject<ElapsedTime>();
        private readonly Subject<ElapsedTime> _onPostUpdate = new Subject<ElapsedTime>();
        private readonly IDisposable _everyUpdateSub;
        
        public IObservable<ElapsedTime> OnPreUpdate => _onPreUpdate;
        public IObservable<ElapsedTime> OnUpdate => _onUpdate;
        public IObservable<ElapsedTime> OnPostUpdate => _onPostUpdate;

        public UnityUpdateScheduler()
        {
            _everyUpdateSub = Observable.EveryUpdate().Subscribe(x =>
            {
                var deltaTime = TimeSpan.FromSeconds(Time.deltaTime);
                var totalTime = ElapsedTime.TotalTime + deltaTime;
                ElapsedTime = new ElapsedTime(deltaTime, totalTime);
                
                _onPreUpdate?.OnNext(ElapsedTime);
                _onUpdate?.OnNext(ElapsedTime);
                _onPostUpdate.OnNext(ElapsedTime);
            });
        }

        public void Dispose()
        {
            _everyUpdateSub.Dispose();
            _onPreUpdate?.Dispose();
            _onPostUpdate?.Dispose();
            _onUpdate?.Dispose();
        }
    }
}