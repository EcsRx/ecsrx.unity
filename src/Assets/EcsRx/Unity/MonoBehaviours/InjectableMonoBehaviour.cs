using UniRx;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public abstract class InjectableMonoBehaviour : MonoBehaviour
    {
        [Inject]
        public IMessageBroker MessageBroker { get; private set; }

        /// <summary>
        /// This is the point in which any injected dependencies will have been resolved for use
        /// </summary>
        [Inject]
        public abstract void DependenciesResolved();
    }
}