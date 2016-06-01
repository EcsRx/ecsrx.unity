using EcsRx.Components;
using UnityEngine;

namespace EcsRx.Unity.Components
{
    public class ViewComponent : IComponent
    {
        public GameObject View { get; set; }
    }
}