using System;
using ModestTree;

namespace Zenject
{
    public class FactorySubContainerBinder<TContract>
        : FactorySubContainerBinderBase<TContract>
    {
        public FactorySubContainerBinder(
            DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier)
            : base(bindContainer, bindInfo, factoryBindInfo, subIdentifier)
        {
        }

        public
#if NOT_UNITY3D
            ScopeConcreteIdArgConditionCopyNonLazyBinder
#else
            DefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder
#endif
            ByMethod(Action<DiContainer> installerMethod)
        {
            var subcontainerBindInfo = new SubContainerCreatorBindInfo();

            ProviderFunc =
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByMethod(
                        container, subcontainerBindInfo, installerMethod), false);

#if NOT_UNITY3D
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo);
#else
            return new DefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder(subcontainerBindInfo, BindInfo);
#endif
        }

#if !NOT_UNITY3D

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabMethod(
            UnityEngine.Object prefab, Action<DiContainer> installerMethod)
        {
            BindingUtil.AssertIsValidPrefab(prefab);

            var gameObjectInfo = new GameObjectCreationParameters();

            ProviderFunc =
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByNewPrefabMethod(
                        container,
                        new PrefabProvider(prefab),
                        gameObjectInfo, installerMethod), false);

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceMethod(
            string resourcePath, Action<DiContainer> installerMethod)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);

            var gameObjectInfo = new GameObjectCreationParameters();

            ProviderFunc =
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByNewPrefabMethod(
                        container,
                        new PrefabProviderResource(resourcePath),
                        gameObjectInfo, installerMethod), false);

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        [System.Obsolete("ByNewPrefab has been renamed to ByNewContextPrefab to avoid confusion with ByNewPrefabInstaller and ByNewPrefabMethod")]
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefab(UnityEngine.Object prefab)
        {
            return ByNewContextPrefab(prefab);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefab(UnityEngine.Object prefab)
        {
            BindingUtil.AssertIsValidPrefab(prefab);

            var gameObjectInfo = new GameObjectCreationParameters();

            ProviderFunc =
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByNewPrefab(
                        container,
                        new PrefabProvider(prefab),
                        gameObjectInfo), false);

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResource(string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);

            var gameObjectInfo = new GameObjectCreationParameters();

            ProviderFunc =
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByNewPrefab(
                        container,
                        new PrefabProviderResource(resourcePath),
                        gameObjectInfo), false);

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }
#endif
    }
}
