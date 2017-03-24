using EcsRx.Entities;
using EcsRx.Pools;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public class DataBoundEntity : MonoBehaviour
    {
        [Inject]
        public IPoolManager PoolManager { get; private set; }

        [SerializeField]
        public string PoolName;

        [SerializeField]
        public byte[] EntitySceneCache;

        public IPool Pool { get; set; }
        public IEntity Entity { get; set; }
    }
}