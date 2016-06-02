using EcsRx.Components;
using UnityEngine;

namespace EcsRx.Unity.Components
{
    public class ViewComponent : IComponent
    {
        public bool DestroyWithView { get; set; }
        public GameObject View { get; set; }
    }
}