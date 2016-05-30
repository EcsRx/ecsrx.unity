#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    public interface IPrefabProvider
    {
        GameObject GetPrefab();
    }
}

#endif

