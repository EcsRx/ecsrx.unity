using Persistity.Attributes;
using UnityEngine;

namespace EcsRx.Tests.Components
{
    [Persist]
    public class MoveableComponent
    {
        [PersistData]
        public Vector3 Position { get; set; }
    }
}