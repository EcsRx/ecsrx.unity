using System;
using ModestTree;

namespace Zenject
{
    public class FactorySubContainerBinderBase<TContract>
    {
        public FactorySubContainerBinderBase(
            BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier)
        {
            FactoryBindInfo = factoryBindInfo;
            SubIdentifier = subIdentifier;
            BindInfo = bindInfo;

            // Reset so we get errors if we end here
            factoryBindInfo.ProviderFunc = null;
        }

        protected FactoryBindInfo FactoryBindInfo
        {
            get; private set;
        }

        protected Func<DiContainer, IProvider> ProviderFunc
        {
            get { return FactoryBindInfo.ProviderFunc; }
            set { FactoryBindInfo.ProviderFunc = value; }
        }

        protected BindInfo BindInfo
        {
            get;
            private set;
        }

        protected object SubIdentifier
        {
            get;
            private set;
        }

        protected Type ContractType
        {
            get { return typeof(TContract); }
        }

        public ArgConditionCopyNonLazyBinder ByInstaller<TInstaller>()
            where TInstaller : InstallerBase
        {
            return ByInstaller(typeof(TInstaller));
        }

        public ArgConditionCopyNonLazyBinder ByInstaller(Type installerType)
        {
            Assert.That(installerType.DerivesFrom<InstallerBase>(),
                "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);

            ProviderFunc = 
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByInstaller(
                        container, installerType, BindInfo.Arguments));

            return new ArgConditionCopyNonLazyBinder(BindInfo);
        }
    }
}
