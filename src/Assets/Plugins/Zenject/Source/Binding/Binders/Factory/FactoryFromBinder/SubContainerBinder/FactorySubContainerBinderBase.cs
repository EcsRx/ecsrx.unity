using System;
using ModestTree;

namespace Zenject
{
    public class FactorySubContainerBinderBase<TContract>
    {
        public FactorySubContainerBinderBase(
            DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier)
        {
            FactoryBindInfo = factoryBindInfo;
            SubIdentifier = subIdentifier;
            BindInfo = bindInfo;
            BindContainer = bindContainer;

            // Reset so we get errors if we end here
            factoryBindInfo.ProviderFunc = null;
        }

        protected DiContainer BindContainer
        {
            get; private set;
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

        public
#if NOT_UNITY3D
            ScopeConcreteIdArgConditionCopyNonLazyBinder
#else
            DefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder
#endif
            ByInstaller<TInstaller>()
            where TInstaller : InstallerBase
        {
            return ByInstaller(typeof(TInstaller));
        }

        public
#if NOT_UNITY3D
            ScopeConcreteIdArgConditionCopyNonLazyBinder
#else
            DefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder
#endif
            ByInstaller(Type installerType)
        {
            Assert.That(installerType.DerivesFrom<InstallerBase>(),
                "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);

            var subcontainerBindInfo = new SubContainerCreatorBindInfo();

            ProviderFunc =
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByInstaller(
                        container, subcontainerBindInfo, installerType, BindInfo.Arguments), false);

#if NOT_UNITY3D
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo);
#else
            return new DefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder(subcontainerBindInfo, BindInfo);
#endif
        }

#if !NOT_UNITY3D
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabInstaller<TInstaller>(
            UnityEngine.Object prefab)
            where TInstaller : InstallerBase
        {
            return ByNewPrefabInstaller(prefab, typeof(TInstaller));
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabInstaller(
            UnityEngine.Object prefab, Type installerType)
        {
            Assert.That(installerType.DerivesFrom<InstallerBase>(),
                "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);

            var gameObjectInfo = new GameObjectCreationParameters();

            ProviderFunc =
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByNewPrefabInstaller(
                        container,
                        new PrefabProvider(prefab),
                        gameObjectInfo, installerType, BindInfo.Arguments), false);

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceInstaller<TInstaller>(
            string resourcePath)
            where TInstaller : InstallerBase
        {
            return ByNewPrefabResourceInstaller(resourcePath, typeof(TInstaller));
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceInstaller(
            string resourcePath, Type installerType)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            Assert.That(installerType.DerivesFrom<InstallerBase>(),
                "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);

            var gameObjectInfo = new GameObjectCreationParameters();

            ProviderFunc =
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByNewPrefabInstaller(
                        container,
                        new PrefabProviderResource(resourcePath),
                        gameObjectInfo, installerType, BindInfo.Arguments), false);

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }
#endif
    }
}
