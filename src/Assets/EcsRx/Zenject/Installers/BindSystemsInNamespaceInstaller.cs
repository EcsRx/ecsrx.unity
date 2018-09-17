using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject;

namespace EcsRx.Zenject.Installers
{
    public class BindSystemsInNamespaceInstaller
    {
        public DiContainer Container { get; }
        public IEnumerable<string> SystemNamespaces { get; }

        public BindSystemsInNamespaceInstaller(DiContainer container, IEnumerable<string> systemNamespaces)
        {
            Container = container;
            SystemNamespaces = systemNamespaces;
        }

        public static void Bind(DiContainer container, IEnumerable<string> systemNamespaces)
        {
            new BindSystemsInNamespaceInstaller(container, systemNamespaces).Bind();
        }

        public void Bind()
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