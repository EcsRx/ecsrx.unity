using System;
using ModestTree;
using System.Linq;

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public class FactorySubContainerBinder<TContract>
        : FactorySubContainerBinderBase<TContract>
    {
        public FactorySubContainerBinder(
            BindInfo bindInfo, Type factoryType,
            BindFinalizerWrapper finalizerWrapper, object subIdentifier)
            : base(bindInfo, factoryType, finalizerWrapper, subIdentifier)
        {
        }

        public ConditionBinder ByMethod(Action<DiContainer> installerMethod)
        {
            SubFinalizer = CreateFinalizer(
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByMethod(
                        container, installerMethod)));

            return new ConditionBinder(BindInfo);
        }

#if !NOT_UNITY3D

        public GameObjectNameGroupNameBinder ByPrefab(GameObject prefab)
        {
            BindingUtil.AssertIsValidPrefab(prefab);

            var gameObjectInfo = new GameObjectBindInfo();

            SubFinalizer = CreateFinalizer(
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByPrefab(
                        container,
                        new PrefabProvider(prefab),
                        gameObjectInfo.Name,
                        gameObjectInfo.GroupName)));

            return new GameObjectNameGroupNameBinder(BindInfo, gameObjectInfo);
        }

        public GameObjectNameGroupNameBinder ByPrefabResource(string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);

            var gameObjectInfo = new GameObjectBindInfo();

            SubFinalizer = CreateFinalizer(
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByPrefab(
                        container,
                        new PrefabProviderResource(resourcePath),
                        gameObjectInfo.Name,
                        gameObjectInfo.GroupName)));

            return new GameObjectNameGroupNameBinder(BindInfo, gameObjectInfo);
        }
#endif
    }
}
