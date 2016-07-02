using EcsRx.Systems;
using UniRx;

namespace EcsRx.Extensions
{
    public static class IManualSystemExtensions
    {
        public static IObservable<long> WaitForScene(this IManualSystem manualSystem)
        {
            return Observable.EveryUpdate().First();
        }  
    }
}