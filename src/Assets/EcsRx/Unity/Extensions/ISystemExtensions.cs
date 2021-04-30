using System;
using SystemsRx.Systems;
using EcsRx.Systems;
using UniRx;

namespace EcsRx.Unity.Extensions
{
    public static class ISystemExtensions
    {
        public static IObservable<long> WaitForScene(this ISystem system)
        {
            return Observable.EveryUpdate().First();
        }
        
        public static void AfterUpdateDo(this ISystem system, Action<long> action)
        {
            Observable.EveryUpdate().First().Subscribe(action);
        }
    }
}