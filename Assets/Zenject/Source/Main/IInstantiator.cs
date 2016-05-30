using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public interface IInstantiator
    {
        // Use this method to create any non-monobehaviour
        // Any fields marked [Inject] will be set using the bindings on the container
        // Any methods marked with a [Inject] will be called
        // Any constructor parameters will be filled in with values from the container
        T Instantiate<T>();
        T Instantiate<T>(IEnumerable<object> extraArgs);
        object Instantiate(Type concreteType);
        object Instantiate(Type concreteType, IEnumerable<object> extraArgs);

        T InstantiateExplicit<T>(List<TypeValuePair> extraArgs);
        object InstantiateExplicit(Type concreteType, List<TypeValuePair> extraArgs);
        object InstantiateExplicit(bool autoInject, InjectArgs extraArgs);

#if !NOT_UNITY3D

        // Add new component to existing game object and fill in its dependencies
        // NOTE: Gameobject here is not a prefab prototype, it is an instance
        TContract InstantiateComponent<TContract>(GameObject gameObject)
            where TContract : Component;

        TContract InstantiateComponent<TContract>(
            GameObject gameObject, IEnumerable<object> extraArgs)
            where TContract : Component;

        Component InstantiateComponent(
            Type componentType, GameObject gameObject);

        Component InstantiateComponent(
            Type componentType, GameObject gameObject, IEnumerable<object> extraArgs);

        Component InstantiateComponentExplicit(
            Type componentType, GameObject gameObject, List<TypeValuePair> extraArgs);

        // Create a new game object from a prefab and fill in dependencies for all children
        GameObject InstantiatePrefab(GameObject prefab);
        GameObject InstantiatePrefab(
            GameObject prefab, IEnumerable<object> extraArgs);

        GameObject InstantiatePrefab(
            GameObject prefab, IEnumerable<object> extraArgs, string groupName);

        // Create a new game object from a resource path and fill in dependencies for all children
        GameObject InstantiatePrefabResource(string resourcePath);

        GameObject InstantiatePrefabResource(
            string resourcePath, IEnumerable<object> extraArgs);

        GameObject InstantiatePrefabResource(
            string resourcePath, IEnumerable<object> extraArgs, string groupName);

        /////////////// InstantiatePrefabForComponent

        // Same as InstantiatePrefab but returns a component after it's initialized

        T InstantiatePrefabForComponent<T>(GameObject prefab);

        T InstantiatePrefabForComponent<T>(
            GameObject prefab, IEnumerable<object> extraArgs);

        object InstantiatePrefabForComponent(
            Type concreteType, GameObject prefab, IEnumerable<object> extraArgs);

        /////////////// InstantiatePrefabResourceForComponent

        T InstantiatePrefabResourceForComponent<T>(string resourcePath);

        T InstantiatePrefabResourceForComponent<T>(
            string resourcePath, IEnumerable<object> extraArgs);

        object InstantiatePrefabResourceForComponent(
            Type concreteType, string resourcePath, IEnumerable<object> extraArgs);

        // Create a new game object from a given prefab
        // Without returning any particular component
        // If you want to retrieve a specific component use InstantiatePrefabForComponent
        GameObject InstantiatePrefabExplicit(
            GameObject prefab, List<TypeValuePair> extraArgs);

        GameObject InstantiatePrefabExplicit(
            GameObject prefab, List<TypeValuePair> extraArgs,
            string groupName);

        GameObject InstantiatePrefabExplicit(
            GameObject prefab, List<TypeValuePair> extraArgs,
            string groupName, bool useAllArgs);

        ////

        GameObject InstantiatePrefabResourceExplicit(
            string resourcePath, List<TypeValuePair> extraArgs);

        GameObject InstantiatePrefabResourceExplicit(
            string resourcePath, List<TypeValuePair> extraArgs,
            string groupName);

        GameObject InstantiatePrefabResourceExplicit(
            string resourcePath, List<TypeValuePair> extraArgs,
            string groupName, bool useAllArgs);

        // Instantiate the given prefab, inject on all components, then return the instance of 'componentType'
        // Any arguments supplied are assumed to be used as extra parameters into 'componentType'

        // This is used instead of Instantiate to support specifying null values
        // Note: Any arguments that are used will be removed from extraArgs
        T InstantiatePrefabForComponentExplicit<T>(
            GameObject prefab, List<TypeValuePair> extraArgs);

        // Note: Any arguments that are used will be removed from extraArgs
        object InstantiatePrefabForComponentExplicit(
            Type componentType, GameObject prefab, List<TypeValuePair> extraArgs);

        // Note: Any arguments that are used will be removed from extraArgs
        object InstantiatePrefabForComponentExplicit(
            Type componentType, GameObject prefab, List<TypeValuePair> extraArgs,
            string groupName);

        // Note: Any arguments that are used will be removed from extraArgs
        object InstantiatePrefabForComponentExplicit(
            GameObject prefab, string groupName, InjectArgs args);

        // This is used instead of Instantiate to support specifying null values
        // Note: Any arguments that are used will be removed from extraArgs
        T InstantiatePrefabResourceForComponentExplicit<T>(
            string resourcePath, List<TypeValuePair> extraArgs);

        // Note: Any arguments that are used will be removed from extraArgs
        object InstantiatePrefabResourceForComponentExplicit(
            Type concreteType, string resourcePath, List<TypeValuePair> extraArgs);

        // Note: Any arguments that are used will be removed from extraArgs
        object InstantiatePrefabResourceForComponentExplicit(
            string resourcePath, string groupName, InjectArgs args);

        // This is the same as GameObject.Instantiate(name) except that it will use
        // the default parent, which can sometimes be set to the Context
        GameObject CreateEmptyGameObject(string name);
        GameObject CreateEmptyGameObject(string name, string groupName);
#endif
    }
}

