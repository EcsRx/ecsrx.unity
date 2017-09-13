using System;

namespace Zenject
{
    public class FactorySubContainerBinder<TParam1, TParam2, TParam3, TContract>
        : FactorySubContainerBinderWithParams<TContract>
    {
        public FactorySubContainerBinder(
            BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier)
            : base(bindInfo, factoryBindInfo, subIdentifier)
        {
        }

        public ConditionCopyNonLazyBinder ByMethod(Action<DiContainer, TParam1, TParam2, TParam3> installerMethod)
        {
            ProviderFunc = 
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByMethod<TParam1, TParam2, TParam3>(
                        container, installerMethod));

            return new ConditionCopyNonLazyBinder(BindInfo);
        }
    }
}


