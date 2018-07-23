using System;
using ModestTree;

namespace Zenject
{
    public class ScopeConcreteIdArgNonLazyBinder : ConcreteIdArgNonLazyBinder
    {
        public ScopeConcreteIdArgNonLazyBinder(BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        public ConcreteIdArgNonLazyBinder AsCached()
        {
            BindInfo.Scope = ScopeTypes.Singleton;
            return this;
        }

        //[Obsolete("AsSingle has been deprecated in favour of AsCached and will be removed in future versions.  Note that you should fix any runtime/validation errors first before replacing AsSingle with AsCached.  See upgrade guide for details.")]
        public ConcreteIdArgNonLazyBinder AsSingle()
        {
            BindInfo.Scope = ScopeTypes.Singleton;
            BindInfo.MarkAsUniqueSingleton = true;
            return this;
        }

        // Note that this is the default so it's not necessary to call this
        public ConcreteIdArgNonLazyBinder AsTransient()
        {
            BindInfo.Scope = ScopeTypes.Transient;
            return this;
        }
    }
}
