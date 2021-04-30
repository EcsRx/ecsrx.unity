using SystemsRx.Executor;
using UnityEngine;
using Zenject;

namespace EcsRx.UnityEditor.MonoBehaviours
{
    public class ActiveSystemsViewer : MonoBehaviour
    {
        [Inject]
        public ISystemExecutor SystemExecutor { get; private set; }
    }
}