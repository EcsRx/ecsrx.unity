using System.Collections.Generic;
using EcsRx.Systems;
using Zenject;

namespace EcsRx.Unity.Installers
{
    /// <summary>
    /// This is for just binding systems and not registering them
    /// </summary>
    public class AutoBindSystemsInstaller : MonoInstaller
    {
        public List<string> SystemNamespaces = new List<string>();

        public override void InstallBindings()
        {
            Container.Bind<ISystem>().To(x => x.AllTypes().DerivingFrom<ISystem>().InNamespaces(SystemNamespaces)).AsSingle();
            Container.Bind(x => x.AllTypes().DerivingFrom<ISystem>().InNamespaces(SystemNamespaces)).AsSingle();
        }
    }
}