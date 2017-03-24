using System;
using System.Collections.Generic;
using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours.Models
{
    [Serializable]
    public class ComponentCache
    {
        [SerializeField]
        private List<string> _activeComponents;

        [SerializeField]
        private List<string> _componentState;




    }
}