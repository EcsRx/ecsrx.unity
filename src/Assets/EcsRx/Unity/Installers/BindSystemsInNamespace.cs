using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Systems;
using ModestTree;
using Zenject;

namespace EcsRx.Unity.Installers
{
    public class BindSystemsInNamespace
    {
        public DiContainer Container { get; }
        public IEnumerable<string> SystemNamespaces { get; }

        public BindSystemsInNamespace(DiContainer container, IEnumerable<string> systemNamespaces)
        {
            Container = container;
            SystemNamespaces = systemNamespaces;
        }

        public static void Bind(DiContainer container, IEnumerable<string> systemNamespaces)
        {
            new BindSystemsInNamespace(container, systemNamespaces).Bind();
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