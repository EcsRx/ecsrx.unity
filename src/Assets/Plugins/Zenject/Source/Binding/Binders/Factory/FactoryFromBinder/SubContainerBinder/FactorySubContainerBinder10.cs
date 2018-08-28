using System;

namespace Zenject
{
    public class FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>
        : FactorySubContainerBinderWithParams<TContract>
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
            ByMethod(
#if !NET_4_6
            ModestTree.Util.
#endif
            Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> installerMethod)
        {
            var subcontainerBindInfo = new SubContainerCreatorBindInfo();

            ProviderFunc =
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10>(
                        container, subcontainerBindInfo, installerMethod), false);

#if NOT_UNITY3D
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo);
#else
            return new DefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder(subcontainerBindInfo, BindInfo);
#endif
        }

#if !NOT_UNITY3D
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabMethod(
            UnityEngine.Object prefab,
#if !NET_4_6
            ModestTree.Util.
#endif
            Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> installerMethod)
        {
            BindingUtil.AssertIsValidPrefab(prefab);

            var gameObjectInfo = new GameObjectCreationParameters();

            ProviderFunc =
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10>( container,
                        new PrefabProvider(prefab),
                        gameObjectInfo, installerMethod), false);

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceMethod(
            string resourcePath,
#if !NET_4_6
            ModestTree.Util.
#endif
            Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> installerMethod)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);

            var gameObjectInfo = new GameObjectCreationParameters();

            ProviderFunc =
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10>( container,
                        new PrefabProviderResource(resourcePath),
                        gameObjectInfo, installerMethod), false);

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }
#endif
    }
}
