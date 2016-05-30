using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenject
{
    [System.Diagnostics.DebuggerStepThrough]
    public abstract class Installer : IInstaller
    {
        [Inject]
        DiContainer _container = null;

        protected DiContainer Container
        {
            get
            {
                return _container;
            }
        }

        public virtual bool IsEnabled
        {
            get
            {
                return true;
            }
        }

        public abstract void InstallBindings();
    }
}
