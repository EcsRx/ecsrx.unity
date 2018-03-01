using EcsRx.Components;
using UnityEngine;

namespace EcsRx.Unity.Examples.SimpleMovement.Components
{
    public class CameraFollowsComponent : IComponent
    {
        public Camera Camera { get; set; }         
    }
}