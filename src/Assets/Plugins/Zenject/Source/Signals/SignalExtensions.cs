using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public static class SignalExtensions
    {
        public static SignalBinderWithId DeclareSignal<T>(this DiContainer container)
            where T : ISignalBase
        {
            var info = new BindInfo(typeof(T));
            var signalSettings = new SignalSettings();
            container.Bind<T>(info).AsCached().WithArguments(signalSettings, info);
            return new SignalBinderWithId(info, signalSettings);
        }

        public static SignalHandlerBinderWithId BindSignal<TSignal>(this DiContainer container)
            where TSignal : ISignal
        {
            var binder = container.StartBinding();
            return new SignalHandlerBinderWithId(
                container, typeof(TSignal), binder);
        }

        public static SignalHandlerBinderWithId<TParam1> BindSignal<TParam1, TSignal>(this DiContainer container)
            where TSignal : ISignal<TParam1>
#if ENABLE_IL2CPP
            // See discussion here for why we do this: https://github.com/modesttree/Zenject/issues/219#issuecomment-284751679
            where TParam1 : class
#endif
        {
            var binder = container.StartBinding();
            return new SignalHandlerBinderWithId<TParam1>(
                container, typeof(TSignal), binder);
        }

        public static SignalHandlerBinderWithId<TParam1, TParam2> BindSignal<TParam1, TParam2, TSignal>(this DiContainer container)
            where TSignal : ISignal<TParam1, TParam2>
#if ENABLE_IL2CPP
            // See discussion here for why we do this: https://github.com/modesttree/Zenject/issues/219#issuecomment-284751679
            where TParam1 : class
            where TParam2 : class
#endif
        {
            var binder = container.StartBinding();
            return new SignalHandlerBinderWithId<TParam1, TParam2>(
                container, typeof(TSignal), binder);
        }

        public static SignalHandlerBinderWithId<TParam1, TParam2, TParam3> BindSignal<TParam1, TParam2, TParam3, TSignal>(this DiContainer container)
            where TSignal : ISignal<TParam1, TParam2, TParam3>
#if ENABLE_IL2CPP
            // See discussion here for why we do this: https://github.com/modesttree/Zenject/issues/219#issuecomment-284751679
            where TParam1 : class
            where TParam2 : class
            where TParam3 : class
#endif
        {
            var binder = container.StartBinding();
            return new SignalHandlerBinderWithId<TParam1, TParam2, TParam3>(
                container, typeof(TSignal), binder);
        }

        public static SignalHandlerBinderWithId<TParam1, TParam2, TParam3, TParam4> BindSignal<TParam1, TParam2, TParam3, TParam4, TSignal>(this DiContainer container)
            where TSignal : ISignal<TParam1, TParam2, TParam3, TParam4>
#if ENABLE_IL2CPP
            // See discussion here for why we do this: https://github.com/modesttree/Zenject/issues/219#issuecomment-284751679
            where TParam1 : class
            where TParam2 : class
            where TParam3 : class
            where TParam4 : class
#endif
        {
            var binder = container.StartBinding();
            return new SignalHandlerBinderWithId<TParam1, TParam2, TParam3, TParam4>(
                container, typeof(TSignal), binder);
        }
    }
}
