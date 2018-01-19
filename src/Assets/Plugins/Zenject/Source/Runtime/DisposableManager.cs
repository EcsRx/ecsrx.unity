using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class DisposableManager : IDisposable
    {
        readonly List<DisposableInfo> _disposables = new List<DisposableInfo>();
        readonly List<LateDisposableInfo> _lateDisposables = new List<LateDisposableInfo>();
        bool _disposed;
        bool _lateDisposed;

        [Inject]
        public DisposableManager(
            [Inject(Optional = true, Source = InjectSources.Local)]
            List<IDisposable> disposables,
            [Inject(Optional = true, Source = InjectSources.Local)]
            List<ModestTree.Util.ValuePair<Type, int>> priorities,
            [Inject(Optional = true, Source = InjectSources.Local)]
            List<ILateDisposable> lateDisposables,
            [Inject(Id = "Late", Optional = true, Source = InjectSources.Local)]
            List<ModestTree.Util.ValuePair<Type, int>> latePriorities)
        {
            foreach (var disposable in disposables)
            {
                // Note that we use zero for unspecified priority
                // This is nice because you can use negative or positive for before/after unspecified
                var matches = priorities.Where(x => disposable.GetType().DerivesFromOrEqual(x.First)).Select(x => x.Second).ToList();
                int priority = matches.IsEmpty() ? 0 : matches.Distinct().Single();

                _disposables.Add(new DisposableInfo(disposable, priority));
            }

            Log.Debug("Loaded {0} IDisposables to DisposablesHandler", _disposables.Count());

            foreach (var lateDisposable in lateDisposables)
            {
                var matches = latePriorities.Where(x => lateDisposable.GetType().DerivesFromOrEqual(x.First)).Select(x => x.Second).ToList();
                int priority = matches.IsEmpty() ? 0 : matches.Distinct().Single();

                _lateDisposables.Add(new LateDisposableInfo(lateDisposable, priority));
            }

            Log.Debug("Loaded {0} ILateDisposables to DisposablesHandler", _lateDisposables.Count());
        }

        public void Add(IDisposable disposable)
        {
            Add(disposable, 0);
        }

        public void Add(IDisposable disposable, int priority)
        {
            _disposables.Add(
                new DisposableInfo(disposable, priority));
        }

        public void Remove(IDisposable disposable)
        {
            _disposables.RemoveWithConfirm(
                _disposables.Where(x => x.Disposable == disposable).Single());
        }

        public void LateDispose()
        {
            Assert.That(!_lateDisposed, "Tried to late dispose DisposableManager twice!");
            _lateDisposed = true;

            // Dispose in the reverse order that they are initialized in
            var disposablesOrdered = _lateDisposables.OrderBy(x => x.Priority).Reverse().ToList();

#if UNITY_EDITOR
            foreach (var disposable in disposablesOrdered.Select(x => x.LateDisposable).GetDuplicates())
            {
                Assert.That(false, "Found duplicate ILateDisposable with type '{0}'".Fmt(disposable.GetType()));
            }
#endif

            foreach (var disposable in disposablesOrdered)
            {
                Log.Debug("Late Disposing '" + disposable.LateDisposable.GetType() + "'");

                try
                {
                    disposable.LateDisposable.LateDispose();
                }
                catch (Exception e)
                {
                    throw Assert.CreateException(
                        e, "Error occurred while late disposing ILateDisposable with type '{0}'", disposable.LateDisposable.GetType());
                }
            }

            Log.Debug("Late Disposed of {0} disposables in DisposablesHandler", disposablesOrdered.Count());
        }

        public void Dispose()
        {
            Assert.That(!_disposed, "Tried to dispose DisposableManager twice!");
            _disposed = true;

            // Dispose in the reverse order that they are initialized in
            var disposablesOrdered = _disposables.OrderBy(x => x.Priority).Reverse().ToList();

#if UNITY_EDITOR
            foreach (var disposable in disposablesOrdered.Select(x => x.Disposable).GetDuplicates())
            {
                Assert.That(false, "Found duplicate IDisposable with type '{0}'".Fmt(disposable.GetType()));
            }
#endif

            foreach (var disposable in disposablesOrdered)
            {
                Log.Debug("Disposing '" + disposable.Disposable.GetType() + "'");

                try
                {
                    disposable.Disposable.Dispose();
                }
                catch (Exception e)
                {
                    throw Assert.CreateException(
                        e, "Error occurred while disposing IDisposable with type '{0}'", disposable.Disposable.GetType());
                }
            }

            Log.Debug("Disposed of {0} disposables in DisposablesHandler", disposablesOrdered.Count());
        }

        class DisposableInfo
        {
            public IDisposable Disposable;
            public int Priority;

            public DisposableInfo(IDisposable disposable, int priority)
            {
                Disposable = disposable;
                Priority = priority;
            }
        }

        class LateDisposableInfo
        {
            public ILateDisposable LateDisposable;
            public int Priority;

            public LateDisposableInfo(ILateDisposable lateDisposable, int priority)
            {
                LateDisposable = lateDisposable;
                Priority = priority;
            }
        }
    }
}
