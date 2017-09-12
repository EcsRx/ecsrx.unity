using System;
using System.Collections.Generic;
using ModestTree;
using ModestTree.Util;

#if ZEN_SIGNALS_ADD_UNIRX
using UniRx;
#endif

namespace Zenject
{
    // This is just used for generic constraints
    public interface ISignal : ISignalBase
    {
        void Fire();

        void Listen(Action listener);
        void Unlisten(Action listener);
    }

    public abstract class Signal<TDerived> : SignalBase, ISignal
        where TDerived : Signal<TDerived>
    {
        readonly List<Action> _listeners = new List<Action>();
#if ZEN_SIGNALS_ADD_UNIRX
        readonly Subject<Unit> _observable = new Subject<Unit>();
#endif

#if ZEN_SIGNALS_ADD_UNIRX
        public UniRx.IObservable<Unit> AsObservable
        {
            get
            {
                return _observable;
            }
        }
#endif

        public int NumListeners
        {
            get { return _listeners.Count; }
        }

        public void Listen(Action listener)
        {
            Assert.That(!_listeners.Contains(listener),
                () => "Tried to add method '{0}' to signal '{1}' but it has already been added"
                .Fmt(listener.ToDebugString(), this.GetType()));
            _listeners.Add(listener);
        }

        public void Unlisten(Action listener)
        {
            bool success = _listeners.Remove(listener);

            Assert.That(success,
                () => "Tried to remove method '{0}' from signal '{1}' without adding it first"
                .Fmt(listener.ToDebugString(), this.GetType()));
        }

        public static TDerived operator + (Signal<TDerived> signal, Action listener)
        {
            signal.Listen(listener);
            return (TDerived)signal;
        }

        public static TDerived operator - (Signal<TDerived> signal, Action listener)
        {
            signal.Unlisten(listener);
            return (TDerived)signal;
        }

        public void Fire()
        {
#if UNITY_EDITOR
            using (ProfileBlock.Start("Signal '{0}'", this.GetType().Name))
#endif
            {
                var wasHandled = Manager.Trigger(SignalId, new object[0]);

                wasHandled |= !_listeners.IsEmpty();

                // Use ToArray in case they remove in the handler
                foreach (var listener in _listeners.ToArray())
                {
#if UNITY_EDITOR
                    using (ProfileBlock.Start(listener.ToDebugString()))
#endif
                    {
                        listener();
                    }
                }

#if ZEN_SIGNALS_ADD_UNIRX
                wasHandled |= _observable.HasObservers;
#if UNITY_EDITOR
                using (ProfileBlock.Start("UniRx Stream"))
#endif
                {
                    _observable.OnNext(Unit.Default);
                }
#endif
                if (Settings.RequiresHandler && !wasHandled)
                {
                    throw Assert.CreateException(
                        "Signal '{0}' was fired but no handlers were attached and the signal is marked to require a handler!", SignalId);
                }
            }
        }
    }
}
