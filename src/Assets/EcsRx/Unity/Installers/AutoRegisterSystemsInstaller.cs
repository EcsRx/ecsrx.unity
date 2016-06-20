using System.Collections.Generic;
using System.Linq;
using EcsRx.Extensions;
using EcsRx.Systems;
using EcsRx.Systems.Executor;
using EcsRx.Unity.Systems;
using Zenject;

namespace EcsRx.Unity.Installers
{
    /// <summary>
    /// This is for binding AND registering systems
    /// </summary>
    public class AutoRegisterSystemsInstaller : MonoInstaller
    {
        public List<string> SystemNamespaces = new List<string>();

        public override void InstallBindings()
        {
            Container.Bind<ISystem>().To(x => x.AllTypes().DerivingFrom<ISystem>().InNamespaces(SystemNamespaces)).AsSingle();
            Container.Bind(x => x.AllTypes().DerivingFrom<ISystem>().InNamespaces(SystemNamespaces)).AsSingle();

            RegisterSystems();
        }

        private void RegisterSystems()
        {
            var allSystems = Container.ResolveAll<ISystem>();
            var systemExecutor = Container.Resolve<ISystemExecutor>();

            var orderedSystems = allSystems
                .OrderByDescending(x => x is ViewResolverSystem)
                .ThenByDescending(x => x is ISetupSystem);
            orderedSystems.ForEachRun(systemExecutor.AddSystem);
        }
    }
}