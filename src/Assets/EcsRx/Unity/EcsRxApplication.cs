using System.Collections.Generic;
using System.Linq;
using EcsRx.Extensions;
using UnityEngine;
using EcsRx.Pools;
using EcsRx.Systems.Executor;
using EcsRx.Unity.Plugins;
using Zenject;

namespace EcsRx.Unity
{
    public abstract class EcsRxApplication : MonoBehaviour
    {
        [Inject]
        public ISystemExecutor SystemExecutor { get; private set; }

        [Inject]
        public IPoolManager PoolManager { get; private set; }

        protected List<IEcsRxPlugin> Plugins { get; private set; }

        protected EcsRxApplication()
        {
            Plugins = new List<IEcsRxPlugin>();
        }

        [Inject]
        private void Init(DiContainer container)
        {
            RegisterAllPluginDependencies(container);
            SetupAllPluginSystems(container);
            GameStarted();
        }

        protected abstract void GameStarted();

        protected virtual void RegisterAllPluginDependencies(DiContainer container)
        { Plugins.ForEachRun(x => x.SetupDependencies(container)); }

        protected virtual void SetupAllPluginSystems(DiContainer container)
        {
            Plugins.SelectMany(x => x.GetSystemForRegistration(container))
                .ForEachRun(x => SystemExecutor.AddSystem(x));
        }

        protected void RegisterPlugin(IEcsRxPlugin plugin)
        { Plugins.Add(plugin); }
    }
}
