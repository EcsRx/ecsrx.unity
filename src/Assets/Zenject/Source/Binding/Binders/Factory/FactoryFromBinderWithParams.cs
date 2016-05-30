using System;
using ModestTree;
using System.Linq;

namespace Zenject
{
    public class FactoryFromBinderWithParams<TContract> : FactoryFromBinderBase<TContract>
    {
        public FactoryFromBinderWithParams(
            BindInfo bindInfo, Type factoryType, BindFinalizerWrapper finalizerWrapper)
            : base(bindInfo, factoryType, finalizerWrapper)
        {
        }
    }
}

