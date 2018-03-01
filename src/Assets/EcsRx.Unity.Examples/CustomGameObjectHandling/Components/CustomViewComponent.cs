using EcsRx.Components;
using UnityEngine;

namespace EcsRx.Unity.Examples.CustomGameObjectHandling.Components
{
    public class CustomViewComponent : IComponent
    {
        public GameObject CustomView { get; set; }
    }
}