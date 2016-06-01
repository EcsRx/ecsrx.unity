using EcsRx.Entities;
using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours
{
    public class EntityBinding : MonoBehaviour
    {
        public string PoolName { get; set; }
        public IEntity Entity { get; set; }
    }
}