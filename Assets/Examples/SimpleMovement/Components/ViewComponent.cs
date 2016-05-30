using EcsRx.Components;
using UnityEngine;

namespace Assets.Examples.SimpleMovement.Components
{
    public class ViewComponent : IComponent
    {
        public GameObject GameObject { get; set; } 
    }
}