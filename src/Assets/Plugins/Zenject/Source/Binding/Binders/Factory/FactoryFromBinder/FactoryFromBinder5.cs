using System;
using System.Collections.Generic;

namespace Zenject
{
    public class FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>
        : FactoryFromBinderBase<TContract>
    {
        public FactoryFromBinder(
            BindInfo bindInfo, FactoryBindInfo factoryBindInfo)
            : base(bindInfo, factoryBindInfo)
        {
        }

        public ConditionCopyNonLazyBinder FromMethod(ModestTree.Util.Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TContract> method)
        {
            ProviderFunc = 
                (container) => new MethodProviderWithContainer<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>(method);

            return this;
        }

        public ConditionCopyNonLazyBinder FromFactory<TSubFactory>()
            where TSubFactory : IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>
        {
            ProviderFunc = 
                (container) => new FactoryProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, TSubFactory>(container, new List<TypeValuePair>());

            return this;
        }

        public FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> FromSubContainerResolve()
        {
            return FromSubContainerResolve(null);
        }

        public FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> FromSubContainerResolve(object subIdentifier)
        {
            return new FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>(
                BindInfo, FactoryBindInfo, subIdentifier);
        }
    }
}
