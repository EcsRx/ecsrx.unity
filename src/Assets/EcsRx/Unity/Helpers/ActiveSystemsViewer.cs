using EcsRx.Systems.Executor;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Helpers
{
    public class ActiveSystemsViewer : MonoBehaviour
    {
        [Inject]
        public ISystemExecutor SystemExecutor { get; private set; }
    }
}