using System;
using System.Collections.Generic;

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public class SingletonProviderCreator
    {
        readonly StandardSingletonProviderCreator _standardProviderCreator;
        readonly SubContainerSingletonProviderCreatorByMethod _subContainerMethodProviderCreator;
        readonly SubContainerSingletonProviderCreatorByInstaller _subContainerInstallerProviderCreator;

#if !NOT_UNITY3D
        readonly SubContainerSingletonProviderCreatorByPrefab _subContainerPrefabProviderCreator;
        readonly SubContainerSingletonProviderCreatorByPrefabResource _subContainerPrefabResourceProviderCreator;

        readonly PrefabSingletonProviderCreator _prefabProviderCreator;
        readonly PrefabResourceSingletonProviderCreator _prefabResourceProviderCreator;
#endif
        public SingletonProviderCreator(
            DiContainer container, SingletonMarkRegistry markRegistry)
        {
            _standardProviderCreator = new StandardSingletonProviderCreator(container, markRegistry);

            _subContainerMethodProviderCreator = new SubContainerSingletonProviderCreatorByMethod(container, markRegistry);
            _subContainerInstallerProviderCreator = new SubContainerSingletonProviderCreatorByInstaller(container, markRegistry);

#if !NOT_UNITY3D
            _subContainerPrefabProviderCreator = new SubContainerSingletonProviderCreatorByPrefab(container, markRegistry);
            _subContainerPrefabResourceProviderCreator = new SubContainerSingletonProviderCreatorByPrefabResource(container, markRegistry);

            _prefabProviderCreator = new PrefabSingletonProviderCreator(container, markRegistry);
            _prefabResourceProviderCreator = new PrefabResourceSingletonProviderCreator(container, markRegistry);
#endif
        }

        public IProvider CreateProviderStandard(
            StandardSingletonDeclaration dec, Func<DiContainer, Type, IProvider> providerCreator)
        {
            return _standardProviderCreator.GetOrCreateProvider(dec, providerCreator);
        }

        public IProvider CreateProviderForSubContainerMethod(
            Type resultType, string concreteIdentifier,
            Action<DiContainer> installMethod, object identifier)
        {
            return _subContainerMethodProviderCreator.CreateProvider(
                resultType, concreteIdentifier, installMethod, identifier);
        }

        public IProvider CreateProviderForSubContainerInstaller(
            Type resultType, string concreteIdentifier,
            Type installerType, object identifier)
        {
            return _subContainerInstallerProviderCreator.CreateProvider(
                resultType, concreteIdentifier, installerType, identifier);
        }

#if !NOT_UNITY3D
        public IProvider CreateProviderForPrefab(
            GameObject prefab, Type resultType, string gameObjectName, string gameObjectGroupName,
            List<TypeValuePair> extraArguments, string concreteIdentifier)
        {
            return _prefabProviderCreator.CreateProvider(
                prefab, resultType, gameObjectName, gameObjectGroupName,
                extraArguments, concreteIdentifier);
        }

        public IProvider CreateProviderForPrefabResource(
            string resourcePath, Type resultType, string gameObjectName, string gameObjectGroupName,
            List<TypeValuePair> extraArguments, string concreteIdentifier)
        {
            return _prefabResourceProviderCreator.CreateProvider(
                resourcePath, resultType, gameObjectName, gameObjectGroupName,
                extraArguments, concreteIdentifier);
        }

        public IProvider CreateProviderForSubContainerPrefab(
            Type resultType, string concreteIdentifier, string gameObjectName, string gameObjectGroupName,
            GameObject prefab, object identifier)
        {
            return _subContainerPrefabProviderCreator.CreateProvider(
                resultType, concreteIdentifier, prefab, identifier, gameObjectName, gameObjectGroupName);
        }

        public IProvider CreateProviderForSubContainerPrefabResource(
            Type resultType, string concreteIdentifier, string gameObjectName, string gameObjectGroupName,
            string resourcePath, object identifier)
        {
            return _subContainerPrefabResourceProviderCreator.CreateProvider(
                resultType, concreteIdentifier, resourcePath, identifier, gameObjectName, gameObjectGroupName);
        }
#endif
    }
}
