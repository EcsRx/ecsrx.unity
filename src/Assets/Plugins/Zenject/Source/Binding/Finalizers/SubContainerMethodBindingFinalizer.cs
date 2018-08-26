using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public class SubContainerMethodBindingFinalizer : ProviderBindingFinalizer
    {
        readonly object _subIdentifier;
        readonly Action<DiContainer> _installMethod;
        readonly bool _resolveAll;

        public SubContainerMethodBindingFinalizer(
            BindInfo bindInfo, Action<DiContainer> installMethod, object subIdentifier, bool resolveAll)
            : base(bindInfo)
        {
            _subIdentifier = subIdentifier;
            _installMethod = installMethod;
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

        void FinalizeBindingConcrete(DiContainer container, List<Type> concreteTypes)
        {
            var scope = GetScope();

            switch (scope)
            {
                case ScopeTypes.Transient:
                {
                    // Note: each contract/concrete pair gets its own container here
                    RegisterProvidersPerContractAndConcreteType(
                        container,
                        concreteTypes,
                        (contractType, concreteType) => new SubContainerDependencyProvider(
                            concreteType, _subIdentifier,
                            new SubContainerCreatorByMethod(container, _installMethod), _resolveAll));
                    break;
                }
                case ScopeTypes.Singleton:
                {
                    var creator = new SubContainerCreatorCached(
                        new SubContainerCreatorByMethod(container, _installMethod));

                    // Note: each contract/concrete pair gets its own container
                    RegisterProvidersForAllContractsPerConcreteType(
                        container,
                        concreteTypes,
                        (_, concreteType) => new SubContainerDependencyProvider(
                            concreteType, _subIdentifier, creator, _resolveAll));
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
                            contractType, _subIdentifier,
                            new SubContainerCreatorByMethod(
                                container, _installMethod), _resolveAll));
                    break;
                }
                case ScopeTypes.Singleton:
                {
                    var containerCreator = new SubContainerCreatorCached(
                        new SubContainerCreatorByMethod(container, _installMethod));

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


