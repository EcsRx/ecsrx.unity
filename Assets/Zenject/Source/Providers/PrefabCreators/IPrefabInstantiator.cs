#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    public interface IPrefabInstantiator
    {
        List<TypeValuePair> ExtraArguments
        {
            get;
        }

        string GameObjectName
        {
            get;
        }

        string GameObjectGroupName
        {
            get;
        }

        IEnumerator<GameObject> Instantiate(List<TypeValuePair> args);

        GameObject GetPrefab();
    }
}

#endif
