using System;
using ModestTree;
using System.Linq;

namespace Zenject
{
    public class FactoryFromBinder<TParam1, TContract> : FactoryFromBinderWithParams<TContract>
    {
        public FactoryFromBinder(
            BindInfo bindInfo,
            Type factoryType,
            BindFinalizerWrapper finalizerWrapper)
            : base(bindInfo, factoryType, finalizerWrapper)
        {
        }

        public ConditionBinder FromMethod(Func<DiContainer, TParam1, TContract> method)
        {
            SubFinalizer = CreateFinalizer(
                (container) => new MethodProviderWithContainer<TParam1, TContract>(method));

            return this;
        }

        public ConditionBinder FromFactory<TSubFactory>()
            where TSubFactory : IFactory<TParam1, TContract>
        {
            SubFinalizer = CreateFinalizer(
                (container) => new FactoryProvider<TParam1, TContract, TSubFactory>(container));

            return this;
        }

        public FactorySubContainerBinder<TParam1, TContract> FromSubContainerResolve()
        {
            return FromSubContainerResolve(null);
        }

        public FactorySubContainerBinder<TParam1, TContract> FromSubContainerResolve(object subIdentifier)
        {
            return new FactorySubContainerBinder<TParam1, TContract>(
                BindInfo, FactoryType, FinalizerWrapper, subIdentifier);
        }
    }
}

