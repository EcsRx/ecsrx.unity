using System;
using System.Collections.Generic;

namespace Zenject
{
    public class FromBinderNonGeneric : FromBinder
    {
        public FromBinderNonGeneric(
            DiContainer bindContainer, BindInfo bindInfo,
            BindFinalizerWrapper finalizerWrapper)
            : base(bindContainer, bindInfo, finalizerWrapper)
        {
        }

        // Shortcut for FromIFactory and also for backwards compatibility
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromFactory<TConcrete, TFactory>()
            where TFactory : IFactory<TConcrete>
        {
            return FromIFactory<TConcrete>(x => x.To<TFactory>().AsCached());
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromIFactory<TContract>(
            Action<ConcreteBinderGeneric<IFactory<TContract>>> factoryBindGenerator)
        {
            return FromIFactoryBase<TContract>(factoryBindGenerator);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethod<TConcrete>(Func<InjectContext, TConcrete> method)
        {
            return FromMethodBase<TConcrete>(method);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodMultiple<TConcrete>(Func<InjectContext, IEnumerable<TConcrete>> method)
        {
            return FromMethodMultipleBase<TConcrete>(method);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj, TContract>(Func<TObj, TContract> method)
        {
            return FromResolveGetter<TObj, TContract>(null, method);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj, TContract>(object identifier, Func<TObj, TContract> method)
        {
            return FromResolveGetter<TObj, TContract>(identifier, method, InjectSources.Any);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj, TContract>(object identifier, Func<TObj, TContract> method, InjectSources source)
        {
            return FromResolveGetterBase<TObj, TContract>(identifier, method, source, false);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj, TContract>(Func<TObj, TContract> method)
        {
            return FromResolveAllGetter<TObj, TContract>(null, method);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj, TContract>(object identifier, Func<TObj, TContract> method)
        {
            return FromResolveAllGetter<TObj, TContract>(identifier, method, InjectSources.Any);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj, TContract>(object identifier, Func<TObj, TContract> method, InjectSources source)
        {
            return FromResolveGetterBase<TObj, TContract>(identifier, method, source, true);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromInstance(object instance)
        {
            return FromInstanceBase(instance);
        }
    }
}
