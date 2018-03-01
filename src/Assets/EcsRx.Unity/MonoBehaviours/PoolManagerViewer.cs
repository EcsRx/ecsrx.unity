using EcsRx.Pools;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public class PoolManagerViewer : MonoBehaviour
    {
         [Inject]
         public IPoolManager PoolManager { get; private set; }
    }
}