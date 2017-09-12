#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class PrefabResourceBindingFinalizer : ProviderBindingFinalizer
    {
        readonly GameObjectCreationParameters _gameObjectBindInfo;
        readonly string _resourcePath;

        public PrefabResourceBindingFinalizer(
            BindInfo bindInfo,
            GameObjectCreationParameters gameObjectBindInfo,
            string resourcePath)
            : base(bindInfo)
        {
            _gameObjectBindInfo = gameObjectBindInfo;
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

        IProvider CreateProviderForType(
            Type contractType, IPrefabInstantiator instantiator)
        {
            Assert.That(contractType.IsInterface() || contractType.DerivesFrom<Component>());

            return new GetFromPrefabComponentProvider(
                contractType, instantiator);
        }

        void FinalizeBindingConcrete(DiContainer container, List<Type> concreteTypes)
        {
            switch (GetScope())
            {
                case ScopeTypes.Singleton:
                {
                    RegisterProvidersForAllContractsPerConcreteType(
                        container,
                        concreteTypes,
                        (_, concreteType) => container.SingletonProviderCreator.CreateProviderForPrefabResource(
                            _resourcePath,
                            concreteType,
                            _gameObjectBindInfo,
                            BindInfo.Arguments,
                            BindInfo.ConcreteIdentifier));
                    break;
                }
                case ScopeTypes.Transient:
                {
                    RegisterProvidersForAllContractsPerConcreteType(
                        container,
                        concreteTypes,
                        (_, concreteType) =>
                            CreateProviderForType(
                                concreteType,
                                new PrefabInstantiator(
                                    container,
                                    _gameObjectBindInfo,
                                    concreteType,
                                    BindInfo.Arguments,
                                    new PrefabProviderResource(_resourcePath))));
                    break;
                }
                case ScopeTypes.Cached:
                {
                    var argumentTarget = concreteTypes.OnlyOrDefault();

                    if (argumentTarget == null)
                    {
                        Assert.That(BindInfo.Arguments.IsEmpty(),
                            "Cannot provide arguments to prefab instantiator when using more than one concrete type");
                    }

                    var prefabCreator = new PrefabInstantiatorCached(
                        new PrefabInstantiator(
                            container,
                            _gameObjectBindInfo,
                            argumentTarget,
                            BindInfo.Arguments,
                            new PrefabProviderResource(_resourcePath)));

                    RegisterProvidersForAllContractsPerConcreteType(
                        container,
                        concreteTypes,
                        (_, concreteType) => new CachedProvider(
                            CreateProviderForType(concreteType, prefabCreator)));
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
            switch (GetScope())
            {
                case ScopeTypes.Singleton:
                {
                    RegisterProviderPerContract(
                        container,
                        (_, contractType) => container.SingletonProviderCreator.CreateProviderForPrefabResource(
                            _resourcePath,
                            contractType,
                            _gameObjectBindInfo,
                            BindInfo.Arguments,
                            BindInfo.ConcreteIdentifier));
                    break;
                }
                case ScopeTypes.Transient:
                {
                    RegisterProviderPerContract(
                        container,
                        (_, contractType) =>
                            CreateProviderForType(
                                contractType,
                                new PrefabInstantiator(
                                    container,
                                    _gameObjectBindInfo,
                                    contractType,
                                    BindInfo.Arguments,
                                    new PrefabProviderResource(_resourcePath))));
                    break;
                }
                case ScopeTypes.Cached:
                {
                    var argumentTarget = BindInfo.ContractTypes.OnlyOrDefault();

                    if (argumentTarget == null)
                    {
                        Assert.That(BindInfo.Arguments.IsEmpty(),
                            "Cannot provide arguments to prefab instantiator when using more than one concrete type");
                    }

                    var prefabCreator = new PrefabInstantiatorCached(
                        new PrefabInstantiator(
                            container,
                            _gameObjectBindInfo,
                            argumentTarget,
                            BindInfo.Arguments,
                            new PrefabProviderResource(_resourcePath)));

                    RegisterProviderPerContract(
                        container,
                        (_, contractType) =>
                            new CachedProvider(
                                CreateProviderForType(contractType, prefabCreator)));
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
