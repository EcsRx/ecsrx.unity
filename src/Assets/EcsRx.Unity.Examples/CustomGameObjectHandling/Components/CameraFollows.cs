using EcsRx.Components;
using UnityEngine;

namespace EcsRx.Unity.Examples.CustomGameObjectHandling.Components
{
    public class CameraFollowsComponent : IComponent
    {
        public Camera Camera { get; set; }         
    }
}