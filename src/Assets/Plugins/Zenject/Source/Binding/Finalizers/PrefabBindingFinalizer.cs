#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class PrefabBindingFinalizer : ProviderBindingFinalizer
    {
        readonly GameObjectCreationParameters _gameObjectBindInfo;
        readonly UnityEngine.Object _prefab;

        public PrefabBindingFinalizer(
            BindInfo bindInfo,
            GameObjectCreationParameters gameObjectBindInfo,
            UnityEngine.Object prefab)
            : base(bindInfo)
        {
            _gameObjectBindInfo = gameObjectBindInfo;
            _prefab = prefab;
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
                        (_, concreteType) => container.SingletonProviderCreator.CreateProviderForPrefab(
                            _prefab,
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
                                    new PrefabProvider(_prefab))));
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
                            new PrefabProvider(_prefab)));

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
                        (_, contractType) => container.SingletonProviderCreator.CreateProviderForPrefab(
                            _prefab,
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
                                    new PrefabProvider(_prefab))));
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
                            new PrefabProvider(_prefab)));

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
