using EcsRx.Collections;
using EcsRx.Entities;
using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours
{
    public class EntityView : MonoBehaviour
    {
        public IEntityCollection EntityCollection { get; set; }
        public IEntity Entity { get; set; }
    }
}