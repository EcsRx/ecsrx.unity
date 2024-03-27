using System;
using System.Collections;
using EcsRx.Unity.Dependencies;
using EcsRx.Zenject.Extensions;
using UnityEngine;
using Zenject;

namespace EcsRx.Zenject.Dependencies
{
    public class ZenjectDependencyResolver : IUnityInstantiator
    {
        private readonly DiContainer _container;
        public object NativeResolver => _container;

        public ZenjectDependencyResolver(DiContainer container)
        {
            _container = container;
        }

        public object Resolve(Type type, string name = null)
        {
            return string.IsNullOrEmpty(name) ? 
                _container.Resolve(type) : 
                _container.ResolveId(type, name);
        }
        
        public IEnumerable ResolveAll(Type type)
        { return _container.ResolveAllOf(type); }

        public GameObject InstantiatePrefab(GameObject prefab)
        { return _container.InstantiatePrefab(prefab); }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}