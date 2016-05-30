using UnityEngine;
using EcsRx.Pools;
using EcsRx.Systems.Executor;
using Zenject;

namespace EcsRx.Unity
{
    public abstract class EcsRxContainer : MonoBehaviour
    {
        [Inject]
        public ISystemExecutor SystemExecutor { get; private set; }

        [Inject]
        public IPoolManager PoolManager { get; private set; }

        [Inject]
        private void Init()
        {
            SetupSystems();
            SetupEntities();
        }
        
        protected abstract void SetupSystems();
        protected abstract void SetupEntities();
    }
}
