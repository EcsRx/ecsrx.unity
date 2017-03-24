using System;
using System.Collections.Generic;
using EcsRx.Persistence.Data;
using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours.Models
{
    [Serializable]
    public class EntitySceneCache
    {
        [SerializeField] public string PoolName;
        [SerializeField] public List<ComponentData> ComponentCaches;
    }
}