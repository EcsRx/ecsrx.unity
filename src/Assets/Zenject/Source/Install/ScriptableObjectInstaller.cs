#if !NOT_UNITY3D

using UnityEngine;
using System.Collections;
using Zenject;

namespace Zenject
{
    public abstract class ScriptableObjectInstaller : ScriptableObject, IInstaller
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

        bool IInstaller.IsEnabled
        {
            get
            {
                return true;
            }
        }

        public abstract void InstallBindings();
    }
}

#endif
