using System;
using System.Collections.Generic;
using ModestTree;
using Zenject.Internal;
using System.Linq;

#if !NOT_UNITY3D
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#endif

namespace Zenject
{
    internal static class BindingUtil
    {
#if !NOT_UNITY3D

        public static void AssertIsValidPrefab(GameObject prefab)
        {
            Assert.That(!ZenUtilInternal.IsNull(prefab), "Received null prefab during bind command");

#if UNITY_EDITOR
            // This won't execute in dll builds sadly
            Assert.That(PrefabUtility.GetPrefabType(prefab) == PrefabType.Prefab,
                "Expected prefab but found game object with name '{0}' during bind command", prefab.name);
#endif
        }

        public static void AssertIsValidGameObject(GameObject gameObject)
        {
            Assert.That(!ZenUtilInternal.IsNull(gameObject), "Received null game object during bind command");

#if UNITY_EDITOR
            Assert.That(PrefabUtility.GetPrefabType(gameObject) != PrefabType.Prefab,
                "Expected game object but found prefab instead with name '{0}' during bind command", gameObject.name);
#endif
        }

        public static void AssertIsNotComponent(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                AssertIsNotComponent(type);
            }
        }

        public static void AssertIsNotComponent<T>()
        {
            AssertIsNotComponent(typeof(T));
        }

        public static void AssertIsNotComponent(Type type)
        {
            Assert.That(!type.DerivesFrom(typeof(Component)),
                "Invalid type given during bind command.  Expected type '{0}' to NOT derive from UnityEngine.Component", type.Name());
        }

        public static void AssertDerivesFromUnityObject(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                AssertDerivesFromUnityObject(type);
            }
        }

        public static void AssertDerivesFromUnityObject<T>()
        {
            AssertDerivesFromUnityObject(typeof(T));
        }

        public static void AssertDerivesFromUnityObject(Type type)
        {
            Assert.That(type.DerivesFrom<UnityEngine.Object>(),
                "Invalid type given during bind command.  Expected type '{0}' to derive from UnityEngine.Object", type.Name());
        }

        public static void AssertTypesAreNotComponents(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                AssertIsNotComponent(type);
            }
        }

        public static void AssertIsValidResourcePath(string resourcePath)
        {
            Assert.That(!string.IsNullOrEmpty(resourcePath), "Null or empty resource path provided");

            // We'd like to validate the path here but unfortunately there doesn't appear to be
            // a way to do this besides loading it
        }

        public static void AssertIsAbstractOrComponent(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                AssertIsAbstractOrComponent(type);
            }
        }

        public static void AssertIsAbstractOrComponent<T>()
        {
            AssertIsAbstractOrComponent(typeof(T));
        }

        public static void AssertIsAbstractOrComponent(Type type)
        {
            Assert.That(type.DerivesFrom(typeof(Component)) || type.IsInterface(),
                "Invalid type given during bind command.  Expected type '{0}' to either derive from UnityEngine.Component or be an interface", type.Name());
        }

        public static void AssertIsAbstractOrComponentOrGameObject(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                AssertIsAbstractOrComponentOrGameObject(type);
            }
        }

        public static void AssertIsAbstractOrComponentOrGameObject<T>()
        {
            AssertIsAbstractOrComponentOrGameObject(typeof(T));
        }

        public static void AssertIsAbstractOrComponentOrGameObject(Type type)
        {
            Assert.That(type.DerivesFrom(typeof(Component)) || type.IsInterface() || type == typeof(GameObject) || type == typeof(UnityEngine.Object),
                "Invalid type given during bind command.  Expected type '{0}' to either derive from UnityEngine.Component or be an interface or be GameObject", type.Name());
        }

        public static void AssertIsComponent(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                AssertIsComponent(type);
            }
        }

        public static void AssertIsComponent<T>()
        {
            AssertIsComponent(typeof(T));
        }

        public static void AssertIsComponent(Type type)
        {
            Assert.That(type.DerivesFrom(typeof(Component)),
                "Invalid type given during bind command.  Expected type '{0}' to derive from UnityEngine.Component", type.Name());
        }

        public static void AssertIsComponentOrGameObject(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                AssertIsComponentOrGameObject(type);
            }
        }

        public static void AssertIsComponentOrGameObject<T>()
        {
            AssertIsComponentOrGameObject(typeof(T));
        }

        public static void AssertIsComponentOrGameObject(Type type)
        {
            Assert.That(type.DerivesFrom(typeof(Component)) || type == typeof(GameObject),
                "Invalid type given during bind command.  Expected type '{0}' to derive from UnityEngine.Component", type.Name());
        }
#else
        public static void AssertTypesAreNotComponents(IEnumerable<Type> types)
        {
        }

        public static void AssertIsNotComponent(Type type)
        {
        }

        public static void AssertIsNotComponent<T>()
        {
        }

        public static void AssertIsNotComponent(IEnumerable<Type> types)
        {
        }
#endif

        public static void AssertTypesAreNotAbstract(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                AssertIsNotAbstract(type);
            }
        }

        public static void AssertIsNotAbstract(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                AssertIsNotAbstract(type);
            }
        }

        public static void AssertIsNotAbstract<T>()
        {
            AssertIsNotAbstract(typeof(T));
        }

        public static void AssertIsNotAbstract(Type type)
        {
            Assert.That(!type.IsAbstract(),
                "Invalid type given during bind command.  Expected type '{0}' to not be abstract.", type);
        }

        public static void AssertIsDerivedFromType(Type concreteType, Type parentType)
        {
            Assert.That(concreteType.DerivesFromOrEqual(parentType),
                "Invalid type given during bind command.  Expected type '{0}' to derive from type '{1}'", concreteType.Name(), parentType.Name());
        }

        public static void AssertConcreteTypeListIsNotEmpty(IEnumerable<Type> concreteTypes)
        {
            Assert.That(concreteTypes.Count() >= 1,
                "Must supply at least one concrete type to the current binding");
        }

        public static void AssertIsIInstallerType(Type installerType)
        {
            Assert.That(installerType.DerivesFrom<IInstaller>(),
                "Invalid installer type given during bind command.  Expected type '{0}' to derive from either 'MonoInstaller' or 'Installer'", installerType.Name());
        }

        public static void AssertIsInstallerType(Type installerType)
        {
            Assert.That(installerType.DerivesFrom<Installer>(),
                "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer'", installerType.Name());
        }

        public static void AssertIsDerivedFromTypes(IEnumerable<Type> concreteTypes, IEnumerable<Type> parentTypes)
        {
            foreach (var concreteType in concreteTypes)
            {
                AssertIsDerivedFromTypes(concreteType, parentTypes);
            }
        }

        public static void AssertIsDerivedFromTypes(Type concreteType, IEnumerable<Type> parentTypes)
        {
            foreach (var parentType in parentTypes)
            {
                AssertIsDerivedFromType(concreteType, parentType);
            }
        }

        public static void AssertInstanceDerivesFromOrEqual(object instance, IEnumerable<Type> parentTypes)
        {
            if (!ZenUtilInternal.IsNull(instance))
            {
                foreach (var baseType in parentTypes)
                {
                    AssertInstanceDerivesFromOrEqual(instance, baseType);
                }
            }
        }

        public static void AssertInstanceDerivesFromOrEqual(object instance, Type baseType)
        {
            if (!ZenUtilInternal.IsNull(instance))
            {
                Assert.That(instance.GetType().DerivesFromOrEqual(baseType),
                    "Invalid type given during bind command.  Expected type '{0}' to derive from type '{1}'", instance.GetType().Name(), baseType.Name());
            }
        }
    }
}
