using SystemsRx.Infrastructure.Dependencies;
using UnityEngine;

namespace EcsRx.Unity.Dependencies
{
    public interface IUnityInstantiator : IDependencyResolver
    {
        GameObject InstantiatePrefab(GameObject prefab);
    }
}