using System.Collections.Generic;
using UnityEngine;

namespace EcsRx.Unity.Dependencies
{
    public interface IUnityInstantiator
    {
        T Resolve<T>(string name = null);
        IEnumerable<T> ResolveAll<T>();
        GameObject InstantiatePrefab(GameObject prefab);
    }
}