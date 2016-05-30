using System;
using System.Collections.Generic;
using ModestTree;

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public enum SingletonTypes
    {
        To,
        ToMethod,
        ToSubContainerMethod,
        ToSubContainerInstaller,
        ToInstance,
        ToPrefab,
        ToPrefabResource,
        ToFactory,
        ToGameObject,
        ToComponent,
        ToComponentGameObject,
        ToGetter,
        ToResolve,
        ToResource,
        ToSubContainerPrefab,
        ToSubContainerPrefabResource,
    }
}

