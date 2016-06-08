using EcsRx.Components;
using UnityEngine;

namespace Assets.EcsRx.Examples.SimpleMovement.Components
{
    public class CameraFollowsComponent : IComponent
    {
        public Camera Camera { get; set; }         
    }
}