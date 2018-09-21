using System;
using System.Collections;
using UnityEngine;

namespace EcsRx.Unity.Dependencies
{
    public interface IUnityInstantiator
    {
        object Resolve(Type type, string name = null);
        IEnumerable ResolveAll(Type type);
        GameObject InstantiatePrefab(GameObject prefab);
    }
}