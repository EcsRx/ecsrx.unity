#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class SubContainerPrefabBindingFinalizer : ProviderBindingFinalizer
    {
        readonly GameObject _prefab;
        readonly object _subIdentifier;
        readonly GameObjectBindInfo _gameObjectBindInfo;

        public SubContainerPrefabBindingFinalizer(
            BindInfo bindInfo,
            GameObjectBindInfo gameObjectBindInfo,
            GameObject prefab,
            object subIdentifier)
            : base(bindInfo)
        {
            _gameObjectBindInfo = gameObjectBindInfo;
            _prefab = prefab;
            _subIdentifier = subIdentifier;
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
                        (_, concreteType) => container.SingletonProviderCreator.CreateProviderForSubContainerPrefab(
                            concreteType,
                            BindInfo.ConcreteIdentifier,
                            _gameObjectBindInfo.Name,
                            _gameObjectBindInfo.GroupName,
                            _prefab,
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
                                container, new PrefabProvider(_prefab), _gameObjectBindInfo.Name, _gameObjectBindInfo.GroupName)));
                    break;
                }
                case ScopeTypes.Cached:
                {
                    var containerCreator = new SubContainerCreatorCached(
                        new SubContainerCreatorByPrefab(
                            container, new PrefabProvider(_prefab), _gameObjectBindInfo.Name, _gameObjectBindInfo.GroupName));

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
                        (_, contractType) => container.SingletonProviderCreator.CreateProviderForSubContainerPrefab(
                            contractType,
                            BindInfo.ConcreteIdentifier,
                            _gameObjectBindInfo.Name,
                            _gameObjectBindInfo.GroupName,
                            _prefab,
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
                                container, new PrefabProvider(_prefab), _gameObjectBindInfo.Name, _gameObjectBindInfo.GroupName)));
                    break;
                }
                case ScopeTypes.Cached:
                {
                    var containerCreator = new SubContainerCreatorCached(
                        new SubContainerCreatorByPrefab(
                            container, new PrefabProvider(_prefab), _gameObjectBindInfo.Name, _gameObjectBindInfo.GroupName));

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
