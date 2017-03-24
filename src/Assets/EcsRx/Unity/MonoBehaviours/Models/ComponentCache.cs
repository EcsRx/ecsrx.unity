using System;
using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours.Models
{
    [Serializable]
    public class ComponentCache
    {
        [SerializeField]
        public string ComponentTypeReference;

        [SerializeField]
        public byte[] ComponentState;
    }
}