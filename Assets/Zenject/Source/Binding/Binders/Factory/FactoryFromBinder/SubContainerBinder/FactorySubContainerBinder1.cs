using System;
using ModestTree;
using System.Linq;

namespace Zenject
{
    public class FactorySubContainerBinder<TParam1, TContract>
        : FactorySubContainerBinderWithParams<TContract>
    {
        public FactorySubContainerBinder(
            BindInfo bindInfo, Type factoryType,
            BindFinalizerWrapper finalizerWrapper, object subIdentifier)
            : base(bindInfo, factoryType, finalizerWrapper, subIdentifier)
        {
        }

        public ConditionBinder ByMethod(Action<DiContainer, TParam1> installerMethod)
        {
            SubFinalizer = CreateFinalizer(
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByMethod<TParam1>(
                        container, installerMethod)));

            return new ConditionBinder(BindInfo);
        }
    }
}
