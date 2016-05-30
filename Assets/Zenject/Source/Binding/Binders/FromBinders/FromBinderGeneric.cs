using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;

#if !NOT_UNITY3D
using UnityEngine;
#endif

using Zenject.Internal;

namespace Zenject
{
    public class FromBinderGeneric<TContract> : FromBinder
    {
        public FromBinderGeneric(
            BindInfo bindInfo,
            BindFinalizerWrapper finalizerWrapper)
            : base(bindInfo, finalizerWrapper)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TContract), BindInfo.ContractTypes);
        }

        public ScopeBinder FromFactory<TFactory>()
            where TFactory : IFactory<TContract>
        {
            return FromFactoryBase<TContract, TFactory>();
        }

        public ScopeBinder FromFactory<TConcrete, TFactory>()
            where TFactory : IFactory<TConcrete>
            where TConcrete : TContract
        {
            return FromFactoryBase<TConcrete, TFactory>();
        }

        public ScopeArgBinder FromMethod(Func<InjectContext, TContract> method)
        {
            return FromMethodBase<TContract>(method);
        }

        public ScopeBinder FromGetter<TObj>(Func<TObj, TContract> method)
        {
            return FromGetter<TObj>(null, method);
        }

        public ScopeBinder FromGetter<TObj>(object identifier, Func<TObj, TContract> method)
        {
            return FromGetterBase<TObj, TContract>(identifier, method);
        }

        public ScopeBinder FromInstance(TContract instance)
        {
            return FromInstanceBase(instance);
        }
    }
}

