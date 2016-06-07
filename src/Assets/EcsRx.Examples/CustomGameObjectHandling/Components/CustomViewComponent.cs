using EcsRx.Components;
using UnityEngine;

namespace Assets.EcsRx.Examples.CustomGameObjectHandling.Components
{
    public class CustomViewComponent : IComponent
    {
        public GameObject CustomView { get; set; }
    }
}