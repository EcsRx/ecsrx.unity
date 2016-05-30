using System;
using System.Collections.Generic;
using EcsRx.Systems.Executor;

namespace EcsRx.Extensions
{
    public static class IDisposableExtensions
    {
        public static void DisposeAll(this IEnumerable<IDisposable> disposables)
        { disposables.ForEachRun(x => x.Dispose()); }

        public static void DisposeAll(this IEnumerable<SubscriptionToken> disposables)
        { disposables.ForEachRun(x => x.Disposable.Dispose()); }
    }
}