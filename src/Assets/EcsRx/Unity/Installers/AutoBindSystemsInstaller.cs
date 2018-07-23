using System.Collections.Generic;
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
            BindSystemsInNamespace.Bind(Container, SystemNamespaces);
        }
    }
}