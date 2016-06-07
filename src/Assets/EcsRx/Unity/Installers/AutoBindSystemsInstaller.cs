using System.Collections.Generic;
using System.Linq;
using EcsRx.Systems;
using UnityEngine;
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
            
            Container.Bind<ISystem>().To(x => x.AllTypes().DerivingFrom<ISystem>().InNamespaces(SystemNamespaces));
            /*
            var systems = Container.ResolveAll<ISystem>();
            Debug.Log(string.Join(", ", systems.Select(x => x.GetType().Name).ToArray()));
            */
        }
    }
}