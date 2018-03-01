using EcsRx.Components;
using UnityEngine;

namespace EcsRx.Unity.Examples.PooledViews.Components
{
    public class SelfDestructComponent : IComponent
    {
        public Vector3 StartingPosition { get; set; }
        public float Lifetime { get; set; }
    }
}