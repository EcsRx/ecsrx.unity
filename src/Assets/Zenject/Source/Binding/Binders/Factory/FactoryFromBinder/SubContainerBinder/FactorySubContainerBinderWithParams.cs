using System;
using ModestTree;
using System.Linq;

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public class FactorySubContainerBinderWithParams<TContract> : FactorySubContainerBinderBase<TContract>
    {
        public FactorySubContainerBinderWithParams(
            BindInfo bindInfo, Type factoryType,
            BindFinalizerWrapper finalizerWrapper, object subIdentifier)
            : base(bindInfo, factoryType, finalizerWrapper, subIdentifier)
        {
        }

#if !NOT_UNITY3D

        public GameObjectNameGroupNameBinder ByPrefab<TInstaller>(GameObject prefab)
            where TInstaller : IInstaller
        {
            return ByPrefab(typeof(TInstaller), prefab);
        }

        public GameObjectNameGroupNameBinder ByPrefab(Type installerType, GameObject prefab)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            BindingUtil.AssertIsIInstallerType(installerType);

            var gameObjectInfo = new GameObjectBindInfo();

            SubFinalizer = CreateFinalizer(
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByPrefabWithParams(
                        installerType,
                        container,
                        new PrefabProvider(prefab),
                        gameObjectInfo.Name,
                        gameObjectInfo.GroupName)));

            return new GameObjectNameGroupNameBinder(BindInfo, gameObjectInfo);
        }

        public GameObjectNameGroupNameBinder ByPrefabResource<TInstaller>(string resourcePath)
            where TInstaller : IInstaller
        {
            return ByPrefabResource(typeof(TInstaller), resourcePath);
        }

        public GameObjectNameGroupNameBinder ByPrefabResource(
            Type installerType, string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);

            var gameObjectInfo = new GameObjectBindInfo();

            SubFinalizer = CreateFinalizer(
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByPrefabWithParams(
                        installerType,
                        container,
                        new PrefabProviderResource(resourcePath),
                        gameObjectInfo.Name,
                        gameObjectInfo.GroupName)));

            return new GameObjectNameGroupNameBinder(BindInfo, gameObjectInfo);
        }
#endif
    }
}
