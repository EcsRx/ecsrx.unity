using System;
using UnityEngine;
using ModestTree;

namespace Zenject
{
    public class ZenjectStateMachineBehaviourAutoInjecter : MonoBehaviour
    {
        DiContainer _container;
        Animator _animator;

        [Inject]
        public void Construct(DiContainer container)
        {
            _container = container;
            _animator = GetComponent<Animator>();
            Assert.IsNotNull(_animator);
        }

        // The unity docs (https://unity3d.com/learn/tutorials/modules/beginner/5-pre-order-beta/state-machine-behaviours)
        // mention that StateMachineBehaviour's should only be retrieved in the Start method
        // which is why we do it here
        public void Start()
        {
            var behaviours = _animator.GetBehaviours<StateMachineBehaviour>();

            if (behaviours != null)
            {
                foreach (var behaviour in behaviours)
                {
                    _container.Inject(behaviour);
                }
            }
        }
    }
}
