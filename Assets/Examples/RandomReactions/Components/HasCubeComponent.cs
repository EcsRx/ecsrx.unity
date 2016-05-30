using EcsRx.Components;
using UnityEngine;

namespace Assets.Examples.RandomReactions.Components
{
    public class HasCubeComponent : IComponent
    {
         public GameObject Cube { get; set; }
    }
}