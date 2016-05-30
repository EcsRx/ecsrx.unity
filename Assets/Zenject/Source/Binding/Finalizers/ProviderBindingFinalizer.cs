using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public abstract class ProviderBindingFinalizer : IBindingFinalizer
    {
        public ProviderBindingFinalizer(BindInfo bindInfo)
        {
            BindInfo = bindInfo;
        }

        public bool InheritInSubContainers
        {
            get
            {
                return BindInfo.InheritInSubContainers;
            }
        }

        protected BindInfo BindInfo
        {
            get;
            private set;
        }

        public void FinalizeBinding(DiContainer container)
        {
            if (BindInfo.ContractTypes.IsEmpty())
            {
                // We could assert her instead but it is nice when used with things like
                // BindAllInterfaces() (and there aren't any interfaces) to allow
                // interfaces to be added later
                return;
            }

            OnFinalizeBinding(container);

            if (BindInfo.NonLazy)
            {
                container.BindRootResolve(
                    BindInfo.Identifier, BindInfo.ContractTypes.ToArray());
            }
        }

        protected abstract void OnFinalizeBinding(DiContainer container);

        protected void RegisterProvider<TContract>(
            DiContainer container, IProvider provider)
        {
            RegisterProvider(container, typeof(TContract), provider);
        }

        protected void RegisterProvider(
            DiContainer container, Type contractType, IProvider provider)
        {
            container.RegisterProvider(
                new BindingId(contractType, BindInfo.Identifier),
                BindInfo.Condition,
                provider);

            if (contractType.IsValueType())
            {
                var nullableType = typeof(Nullable<>).MakeGenericType(contractType);

                // Also bind to nullable primitives
                // this is useful so that we can have optional primitive dependencies
                container.RegisterProvider(
                    new BindingId(nullableType, BindInfo.Identifier),
                    BindInfo.Condition,
                    provider);
            }
        }

        protected void RegisterProviderPerContract(
            DiContainer container, Func<DiContainer, Type, IProvider> providerFunc)
        {
            foreach (var contractType in BindInfo.ContractTypes)
            {
                RegisterProvider(container, contractType, providerFunc(container, contractType));
            }
        }

        protected void RegisterProviderForAllContracts(
            DiContainer container, IProvider provider)
        {
            foreach (var contractType in BindInfo.ContractTypes)
            {
                RegisterProvider(container, contractType, provider);
            }
        }

        protected void RegisterProvidersPerContractAndConcreteType(
            DiContainer container,
            List<Type> concreteTypes,
            Func<Type, Type, IProvider> providerFunc)
        {
            Assert.That(!BindInfo.ContractTypes.IsEmpty());
            Assert.That(!concreteTypes.IsEmpty());

            foreach (var contractType in BindInfo.ContractTypes)
            {
                foreach (var concreteType in concreteTypes)
                {
                    Assert.DerivesFromOrEqual(concreteType, contractType);

                    RegisterProvider(
                        container, contractType, providerFunc(contractType, concreteType));
                }
            }
        }

        // Note that if multiple contract types are provided per concrete type,
        // it will re-use the same provider for each contract type
        // (each concrete type will have its own provider though)
        protected void RegisterProvidersForAllContractsPerConcreteType(
            DiContainer container,
            List<Type> concreteTypes,
            Func<DiContainer, Type, IProvider> providerFunc)
        {
            Assert.That(!BindInfo.ContractTypes.IsEmpty());
            Assert.That(!concreteTypes.IsEmpty());

            var providerMap = concreteTypes.ToDictionary(x => x, x => providerFunc(container, x));

            foreach (var contractType in BindInfo.ContractTypes)
            {
                foreach (var concreteType in concreteTypes)
                {
                    Assert.DerivesFromOrEqual(concreteType, contractType);

                    RegisterProvider(container, contractType, providerMap[concreteType]);
                }
            }
        }
    }
}

