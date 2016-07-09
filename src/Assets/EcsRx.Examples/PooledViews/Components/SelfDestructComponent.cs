using EcsRx.Components;
using UnityEngine;

public class SelfDestructComponent : IComponent
{
    public Vector3 StartingPosition { get; set; }
    public float Lifetime { get; set; }
}