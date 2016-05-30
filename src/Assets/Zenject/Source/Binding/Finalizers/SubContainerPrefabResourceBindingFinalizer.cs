#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class SubContainerPrefabResourceBindingFinalizer : ProviderBindingFinalizer
    {
        readonly string _resourcePath;
        readonly object _subIdentifier;
        readonly GameObjectBindInfo _gameObjectBindInfo;

        public SubContainerPrefabResourceBindingFinalizer(
            BindInfo bindInfo,
            GameObjectBindInfo gameObjectBindInfo,
            string resourcePath,
            object subIdentifier)
            : base(bindInfo)
        {
            _gameObjectBindInfo = gameObjectBindInfo;
            _subIdentifier = subIdentifier;
            _resourcePath = resourcePath;
        }

        protected override void OnFinalizeBinding(DiContainer container)
        {
            if (BindInfo.ToChoice == ToChoices.Self)
            {
                Assert.IsEmpty(BindInfo.ToTypes);
                FinalizeBindingSelf(container);
            }
            else
            {
                FinalizeBindingConcrete(container, BindInfo.ToTypes);
            }
        }

        void FinalizeBindingConcrete(DiContainer container, List<Type> concreteTypes)
        {
            switch (BindInfo.Scope)
            {
                case ScopeTypes.Singleton:
                {
                    RegisterProvidersForAllContractsPerConcreteType(
                        container,
                        concreteTypes,
                        (_, concreteType) => container.SingletonProviderCreator.CreateProviderForSubContainerPrefabResource(
                            concreteType,
                            BindInfo.ConcreteIdentifier,
                            _gameObjectBindInfo.Name,
                            _gameObjectBindInfo.GroupName,
                            _resourcePath,
                            _subIdentifier));
                    break;
                }
                case ScopeTypes.Transient:
                {
                    RegisterProvidersForAllContractsPerConcreteType(
                        container,
                        concreteTypes,
                        (_, concreteType) => new SubContainerDependencyProvider(
                            concreteType, _subIdentifier,
                            new SubContainerCreatorByPrefab(
                                container, new PrefabProviderResource(_resourcePath), _gameObjectBindInfo.Name, _gameObjectBindInfo.GroupName)));
                    break;
                }
                case ScopeTypes.Cached:
                {
                    var containerCreator = new SubContainerCreatorCached(
                        new SubContainerCreatorByPrefab(container, new PrefabProviderResource(_resourcePath), _gameObjectBindInfo.Name, _gameObjectBindInfo.GroupName));

                    RegisterProvidersForAllContractsPerConcreteType(
                        container,
                        concreteTypes,
                        (_, concreteType) =>
                        new SubContainerDependencyProvider(
                            concreteType, _subIdentifier, containerCreator));
                    break;
                }
                default:
                {
                    throw Assert.CreateException();
                }
            }
        }

        void FinalizeBindingSelf(DiContainer container)
        {
            switch (BindInfo.Scope)
            {
                case ScopeTypes.Singleton:
                {
                    RegisterProviderPerContract(
                        container,
                        (_, contractType) => container.SingletonProviderCreator.CreateProviderForSubContainerPrefabResource(
                            contractType,
                            BindInfo.ConcreteIdentifier,
                            _gameObjectBindInfo.Name,
                            _gameObjectBindInfo.GroupName,
                            _resourcePath,
                            _subIdentifier));
                    break;
                }
                case ScopeTypes.Transient:
                {
                    RegisterProviderPerContract(
                        container,
                        (_, contractType) => new SubContainerDependencyProvider(
                            contractType, _subIdentifier,
                            new SubContainerCreatorByPrefab(
                                container, new PrefabProviderResource(_resourcePath), _gameObjectBindInfo.Name, _gameObjectBindInfo.GroupName)));
                    break;
                }
                case ScopeTypes.Cached:
                {
                    var containerCreator = new SubContainerCreatorCached(
                        new SubContainerCreatorByPrefab(
                            container, new PrefabProviderResource(_resourcePath), _gameObjectBindInfo.Name, _gameObjectBindInfo.GroupName));

                    RegisterProviderPerContract(
                        container,
                        (_, contractType) =>
                        new SubContainerDependencyProvider(
                            contractType, _subIdentifier, containerCreator));
                    break;
                }
                default:
                {
                    throw Assert.CreateException();
                }
            }
        }
    }
}

#endif
