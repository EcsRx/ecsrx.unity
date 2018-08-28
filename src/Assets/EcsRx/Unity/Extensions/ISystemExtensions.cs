using System;
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
    }
}