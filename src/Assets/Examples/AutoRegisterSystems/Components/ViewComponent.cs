using EcsRx.Components;
using UnityEngine;

namespace Assets.Examples.AutoRegisterSystems.Components
{
    public class ViewComponent : IComponent
    {
         public GameObject GameObject { get; set; }
    }
}