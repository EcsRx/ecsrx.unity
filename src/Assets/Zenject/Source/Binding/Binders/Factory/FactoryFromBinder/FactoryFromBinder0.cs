using System;
using ModestTree;
using System.Linq;

namespace Zenject
{
    public class FactoryFromBinder<TContract> : FactoryFromBinderBase<TContract>
    {
        public FactoryFromBinder(
            BindInfo bindInfo,
            Type factoryType,
            BindFinalizerWrapper finalizerWrapper)
            : base(bindInfo, factoryType, finalizerWrapper)
        {
        }

        public ConditionBinder FromGetter<TObj>(Func<TObj, TContract> method)
        {
            return FromGetter<TObj>(null, method);
        }

        public ConditionBinder FromGetter<TObj>(
            object subIdentifier, Func<TObj, TContract> method)
        {
            SubFinalizer = CreateFinalizer(
                (container) => new GetterProvider<TObj, TContract>(subIdentifier, method, container));

            return this;
        }

        public ConditionBinder FromMethod(Func<DiContainer, TContract> method)
        {
            SubFinalizer = CreateFinalizer(
                (container) => new MethodProviderWithContainer<TContract>(method));

            return this;
        }

        public ConditionBinder FromInstance(object instance)
        {
            BindingUtil.AssertInstanceDerivesFromOrEqual(instance, AllParentTypes);

            SubFinalizer = CreateFinalizer(
                (container) => new InstanceProvider(ContractType, instance));

            return this;
        }

        public ConditionBinder FromFactory<TSubFactory>()
            where TSubFactory : IFactory<TContract>
        {
            SubFinalizer = CreateFinalizer(
                (container) => new FactoryProvider<TContract, TSubFactory>(container));

            return this;
        }

        public FactorySubContainerBinder<TContract> FromSubContainerResolve()
        {
            return FromSubContainerResolve(null);
        }

        public FactorySubContainerBinder<TContract> FromSubContainerResolve(object subIdentifier)
        {
            return new FactorySubContainerBinder<TContract>(
                BindInfo, FactoryType, FinalizerWrapper, subIdentifier);
        }

#if !NOT_UNITY3D

        public ConditionBinder FromResource(string resourcePath)
        {
            BindingUtil.AssertDerivesFromUnityObject(ContractType);

            SubFinalizer = CreateFinalizer(
                (container) => new ResourceProvider(resourcePath, ContractType));

            return this;
        }
#endif
    }
}
