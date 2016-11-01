using EcsRx.Entities;
using EcsRx.Pools;
using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours
{
    public class EntityView : MonoBehaviour
    {
        public IPool Pool { get; set; }
        public IEntity Entity { get; set; }
    }
}