using System;
using ModestTree;

namespace Zenject
{
    [System.Diagnostics.DebuggerStepThrough]
    public class Kernel : IInitializable, IDisposable, ITickable, ILateTickable, IFixedTickable, ILateDisposable
    {
        [InjectLocal]
        TickableManager _tickableManager = null;

        [InjectLocal]
        InitializableManager _initializableManager = null;

        [InjectLocal]
        DisposableManager _disposablesManager = null;

        public virtual void Initialize()
        {
            Log.Debug("Zenject: Initializing IInitializable's");

            _initializableManager.Initialize();
        }

        public virtual void Dispose()
        {
            Log.Debug("Zenject: Disposing IDisposable's");
            _disposablesManager.Dispose();
        }

        public virtual void LateDispose()
        {
            Log.Debug("Zenject: Disposing ILateDisposable's");
            _disposablesManager.LateDispose();
        }

        public virtual void Tick()
        {
            _tickableManager.Update();
        }

        public virtual void LateTick()
        {
            _tickableManager.LateUpdate();
        }

        public virtual void FixedTick()
        {
            _tickableManager.FixedUpdate();
        }
    }
}
