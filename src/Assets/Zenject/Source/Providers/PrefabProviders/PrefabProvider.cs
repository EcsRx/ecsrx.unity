#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class PrefabProvider : IPrefabProvider
    {
        readonly GameObject _prefab;

        public PrefabProvider(GameObject prefab)
        {
            Assert.IsNotNull(prefab);
            _prefab = prefab;
        }

        public GameObject GetPrefab()
        {
            return _prefab;
        }
    }
}

#endif


