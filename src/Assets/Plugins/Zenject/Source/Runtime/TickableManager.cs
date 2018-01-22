using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class TickableManager
    {
        // Make this lazy just as bit of extra assurance that we don't trigger circular dependencies
        // accidentally
        [Inject(Optional = true, Source = InjectSources.Parent)]
        readonly Lazy<List<TickableManager>> _parents = null;

        [Inject(Optional = true, Source = InjectSources.Local)]
        readonly List<ITickable> _tickables = null;

        [Inject(Optional = true, Source = InjectSources.Local)]
        readonly List<IFixedTickable> _fixedTickables = null;

        [Inject(Optional = true, Source = InjectSources.Local)]
        readonly List<ILateTickable> _lateTickables = null;

        [Inject(Optional = true, Source = InjectSources.Local)]
        readonly List<ModestTree.Util.ValuePair<Type, int>> _priorities = null;

        [Inject(Optional = true, Id = "Fixed", Source = InjectSources.Local)]
        readonly List<ModestTree.Util.ValuePair<Type, int>> _fixedPriorities = null;

        [Inject(Optional = true, Id = "Late", Source = InjectSources.Local)]
        readonly List<ModestTree.Util.ValuePair<Type, int>> _latePriorities = null;

        readonly TickablesTaskUpdater _updater = new TickablesTaskUpdater();
        readonly FixedTickablesTaskUpdater _fixedUpdater = new FixedTickablesTaskUpdater();
        readonly LateTickablesTaskUpdater _lateUpdater = new LateTickablesTaskUpdater();

        bool _isPaused;

        [Inject]
        public TickableManager()
        {
        }

        public IEnumerable<ITickable> Tickables
        {
            get { return _tickables; }
        }

        public bool IsPaused
        {
            get
            {
                if (_isPaused)
                {
                    return true;
                }

                if (_parents.Value != null)
                {
                    for (int i = 0; i < _parents.Value.Count; i++)
                    {
                        if (_parents.Value[i].IsPaused)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        [Inject]
        public void Initialize()
        {
            InitTickables();
            InitFixedTickables();
            InitLateTickables();
        }

        void InitFixedTickables()
        {
            foreach (var type in _fixedPriorities.Select(x => x.First))
            {
                Assert.That(type.DerivesFrom<IFixedTickable>(),
                    "Expected type '{0}' to drive from IFixedTickable while checking priorities in TickableHandler", type);
            }

            foreach (var tickable in _fixedTickables)
            {
                // Note that we use zero for unspecified priority
                // This is nice because you can use negative or positive for before/after unspecified
                var matches = _fixedPriorities.Where(x => tickable.GetType().DerivesFromOrEqual(x.First)).Select(x => x.Second).ToList();
                int priority = matches.IsEmpty() ? 0 : matches.Distinct().Single();

                _fixedUpdater.AddTask(tickable, priority);
            }
        }

        void InitTickables()
        {
            foreach (var type in _priorities.Select(x => x.First))
            {
                Assert.That(type.DerivesFrom<ITickable>(),
                    "Expected type '{0}' to drive from ITickable while checking priorities in TickableHandler", type);
            }

            foreach (var tickable in _tickables)
            {
                // Note that we use zero for unspecified priority
                // This is nice because you can use negative or positive for before/after unspecified
                var matches = _priorities.Where(x => tickable.GetType().DerivesFromOrEqual(x.First)).Select(x => x.Second).ToList();
                int priority = matches.IsEmpty() ? 0 : matches.Distinct().Single();

                _updater.AddTask(tickable, priority);
            }
        }

        void InitLateTickables()
        {
            foreach (var type in _latePriorities.Select(x => x.First))
            {
                Assert.That(type.DerivesFrom<ILateTickable>(),
                    "Expected type '{0}' to drive from ILateTickable while checking priorities in TickableHandler", type);
            }

            foreach (var tickable in _lateTickables)
            {
                // Note that we use zero for unspecified priority
                // This is nice because you can use negative or positive for before/after unspecified
                var matches = _latePriorities.Where(x => tickable.GetType().DerivesFromOrEqual(x.First)).Select(x => x.Second).ToList();
                int priority = matches.IsEmpty() ? 0 : matches.Distinct().Single();

                _lateUpdater.AddTask(tickable, priority);
            }
        }

        public void Add(ITickable tickable, int priority)
        {
            _updater.AddTask(tickable, priority);
        }

        public void Add(ITickable tickable)
        {
            Add(tickable, 0);
        }

        public void AddLate(ILateTickable tickable, int priority)
        {
            _lateUpdater.AddTask(tickable, priority);
        }

        public void AddLate(ILateTickable tickable)
        {
            AddLate(tickable, 0);
        }

        public void AddFixed(IFixedTickable tickable, int priority)
        {
            _fixedUpdater.AddTask(tickable, priority);
        }

        public void AddFixed(IFixedTickable tickable)
        {
            _fixedUpdater.AddTask(tickable, 0);
        }

        public void Remove(ITickable tickable)
        {
            _updater.RemoveTask(tickable);
        }

        public void RemoveLate(ILateTickable tickable)
        {
            _lateUpdater.RemoveTask(tickable);
        }

        public void RemoveFixed(IFixedTickable tickable)
        {
            _fixedUpdater.RemoveTask(tickable);
        }

        public void Update()
        {
            if(IsPaused)
            {
                return;
            }

            _updater.OnFrameStart();
            _updater.UpdateAll();
        }

        public void FixedUpdate()
        {
            if(IsPaused)
            {
                return;
            }

            _fixedUpdater.OnFrameStart();
            _fixedUpdater.UpdateAll();
        }

        public void LateUpdate()
        {
            if(IsPaused)
            {
                return;
            }

            _lateUpdater.OnFrameStart();
            _lateUpdater.UpdateAll();
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }
    }
}
