using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public class SubContainerInstallerBindingFinalizer : ProviderBindingFinalizer
    {
        readonly object _subIdentifier;
        readonly Type _installerType;
        readonly bool _resolveAll;

        public SubContainerInstallerBindingFinalizer(
            BindInfo bindInfo, Type installerType, object subIdentifier, bool resolveAll)
            : base(bindInfo)
        {
            _subIdentifier = subIdentifier;
            _installerType = installerType;
            _resolveAll = resolveAll;
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

        ISubContainerCreator CreateContainerCreator(DiContainer container)
        {
            return new SubContainerCreatorCached(
                new SubContainerCreatorByInstaller(container, _installerType));
        }

        void FinalizeBindingConcrete(DiContainer container, List<Type> concreteTypes)
        {
            var scope = GetScope();

            switch (scope)
            {
                case ScopeTypes.Transient:
                {
                    RegisterProvidersForAllContractsPerConcreteType(
                        container,
                        concreteTypes,
                        (_, concreteType) =>
                            new SubContainerDependencyProvider(
                                concreteType, _subIdentifier, CreateContainerCreator(container), _resolveAll));
                    break;
                }
                case ScopeTypes.Singleton:
                {
                    var containerCreator = CreateContainerCreator(container);

                    RegisterProvidersForAllContractsPerConcreteType(
                        container,
                        concreteTypes,
                        (_, concreteType) =>
                            new SubContainerDependencyProvider(
                                concreteType, _subIdentifier, containerCreator, _resolveAll));
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
            var scope = GetScope();

            switch (scope)
            {
                case ScopeTypes.Transient:
                {
                    RegisterProviderPerContract(
                        container,
                        (_, contractType) => new SubContainerDependencyProvider(
                            contractType, _subIdentifier, CreateContainerCreator(container), _resolveAll));
                    break;
                }
                case ScopeTypes.Singleton:
                {
                    var containerCreator = CreateContainerCreator(container);

                    RegisterProviderPerContract(
                        container,
                        (_, contractType) =>
                            new SubContainerDependencyProvider(
                                contractType, _subIdentifier, containerCreator, _resolveAll));
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

