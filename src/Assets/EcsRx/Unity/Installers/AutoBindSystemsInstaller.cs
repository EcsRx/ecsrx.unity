using System;
using System.Collections.Generic;
using EcsRx.Systems;
using ModestTree;
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
            Container.Bind<ISystem>()
                .To(GetAllApplicableSystemsInNamespace)
                .AsSingle();
        }

        private void GetAllApplicableSystemsInNamespace(ConventionSelectTypesBinder binding)
        {
            binding.AllTypes()
                .Where(IsWithinNamespace)
                .Where(NoAbstractOrInterfaces)
                .Where(IsApplicableSystem);
        }

        private bool IsWithinNamespace(Type type)
        { return SystemNamespaces.Contains(type.Namespace); }

        private bool NoAbstractOrInterfaces(Type type)
        { return !type.IsInterface && !type.IsAbstract; }

        private bool IsApplicableSystem(Type possibleSystemType)
        {
            var isApplicable = possibleSystemType == typeof(ISystem) || possibleSystemType.DerivesFrom<ISystem>();
            return isApplicable;
        }
    }
}