using System.Collections.Generic;
using System.Linq;
using EcsRx.Extensions;
using EcsRx.Systems;
using EcsRx.Systems.Executor;
using Zenject;

namespace EcsRx.Unity.Installers
{
    public class AutoRegisterSystemsInstaller : MonoInstaller
    {
        public List<string> SystemNamespaces = new List<string>();

        public override void InstallBindings()
        {
            Container.Bind<ISystem>().To(x => x.AllTypes().DerivingFrom<ISystem>().InNamespaces(SystemNamespaces));

            RegisterSystems();
        }

        private void RegisterSystems()
        {
            var allSystems = Container.ResolveAll<ISystem>();
            var systemExecutor = Container.Resolve<ISystemExecutor>();

            var orderedSystems = allSystems.OrderByDescending(x => x is ISetupSystem);
            orderedSystems.ForEachRun(systemExecutor.AddSystem);
        }
    }
}