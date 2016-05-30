#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using Zenject;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // This factory type can be useful if you want to control where the prefab comes from at runtime
    // rather than from within the installers

    //No parameters
    public class PrefabFactory<T> : IValidatable
        where T : Component
    {
        [Inject]
        protected readonly DiContainer _container;

        public T Create(GameObject prefab)
        {
            Assert.That(prefab != null,
               "Null prefab given to factory create method when instantiating object with type '{0}'.", typeof(T));

            return _container.InstantiatePrefabForComponent<T>(prefab);
        }

        public virtual T Create(string prefabResourceName)
        {
            Assert.That(!string.IsNullOrEmpty(prefabResourceName),
              "Null or empty prefab resource name given to factory create method when instantiating object with type '{0}'.", typeof(T));

            return Create((GameObject)Resources.Load(prefabResourceName));
        }

        public void Validate()
        {
            _container.InjectExplicit(
                new ValidationMarker(typeof(T)), ValidationUtil.CreateDefaultArgs());
        }
    }

    // One parameter
    public class PrefabFactory<P1, T> : IValidatable
        where T : Component
    {
        [Inject]
        protected readonly DiContainer _container;

        public virtual T Create(GameObject prefab, P1 param)
        {
            Assert.That(prefab != null,
               "Null prefab given to factory create method when instantiating object with type '{0}'.", typeof(T));

            return _container.InstantiatePrefabForComponentExplicit<T>(
                prefab, InjectUtil.CreateArgListExplicit(param));
        }

        public virtual T Create(string prefabResourceName, P1 param)
        {
            Assert.That(!string.IsNullOrEmpty(prefabResourceName),
              "Null or empty prefab resource name given to factory create method when instantiating object with type '{0}'.", typeof(T));

            return Create((GameObject)Resources.Load(prefabResourceName), param);
        }

        public void Validate()
        {
            _container.InjectExplicit(
                new ValidationMarker(typeof(T)), ValidationUtil.CreateDefaultArgs(typeof(P1)));
        }
    }

    // Two parameters
    public class PrefabFactory<P1, P2, T> : IValidatable
        where T : Component
    {
        [Inject]
        protected readonly DiContainer _container;

        public virtual T Create(GameObject prefab, P1 param, P2 param2)
        {
            Assert.That(prefab != null,
               "Null prefab given to factory create method when instantiating object with type '{0}'.", typeof(T));

            return _container.InstantiatePrefabForComponentExplicit<T>(
                prefab, InjectUtil.CreateArgListExplicit(param, param2));
        }

        public virtual T Create(string prefabResourceName, P1 param, P2 param2)
        {
            Assert.That(!string.IsNullOrEmpty(prefabResourceName),
              "Null or empty prefab resource name given to factory create method when instantiating object with type '{0}'.", typeof(T));

            return Create((GameObject)Resources.Load(prefabResourceName), param, param2);
        }

        public void Validate()
        {
            _container.InjectExplicit(
                new ValidationMarker(typeof(T)), ValidationUtil.CreateDefaultArgs(typeof(P1), typeof(P2)));
        }
    }

    // Three parameters
    public class PrefabFactory<P1, P2, P3, T> : IValidatable
        where T : Component
    {
        [Inject]
        protected readonly DiContainer _container;

        public virtual T Create(GameObject prefab, P1 param, P2 param2, P3 param3)
        {
            Assert.That(prefab != null,
               "Null prefab given to factory create method when instantiating object with type '{0}'.", typeof(T));

            return _container.InstantiatePrefabForComponentExplicit<T>(
                prefab, InjectUtil.CreateArgListExplicit(param, param2, param3));
        }

        public virtual T Create(string prefabResourceName, P1 param, P2 param2, P3 param3)
        {
            Assert.That(!string.IsNullOrEmpty(prefabResourceName),
              "Null or empty prefab resource name given to factory create method when instantiating object with type '{0}'.", typeof(T));

            return Create((GameObject)Resources.Load(prefabResourceName), param, param2, param3);
        }

        public void Validate()
        {
            _container.InjectExplicit(
                new ValidationMarker(typeof(T)),
                    ValidationUtil.CreateDefaultArgs(typeof(P1), typeof(P2), typeof(P3)));
        }
    }

    // Four parameters
    public class PrefabFactory<P1, P2, P3, P4, T> : IValidatable
        where T : Component
    {
        [Inject]
        protected readonly DiContainer _container;

        public virtual T Create(GameObject prefab, P1 param, P2 param2, P3 param3, P4 param4)
        {
            Assert.That(prefab != null,
               "Null prefab given to factory create method when instantiating object with type '{0}'.", typeof(T));

            return _container.InstantiatePrefabForComponentExplicit<T>(
                prefab, InjectUtil.CreateArgListExplicit(param, param2, param3, param4));
        }

        public virtual T Create(string prefabResourceName, P1 param, P2 param2, P3 param3, P4 param4)
        {
            Assert.That(!string.IsNullOrEmpty(prefabResourceName),
              "Null or empty prefab resource name given to factory create method when instantiating object with type '{0}'.", typeof(T));

            return Create((GameObject)Resources.Load(prefabResourceName), param, param2, param3, param4);
        }

        public void Validate()
        {
            _container.InjectExplicit(
                new ValidationMarker(typeof(T)),
                ValidationUtil.CreateDefaultArgs(typeof(P1), typeof(P2), typeof(P3), typeof(P4)));
        }
    }
}

#endif
