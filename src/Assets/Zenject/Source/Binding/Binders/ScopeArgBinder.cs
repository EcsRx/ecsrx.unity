using System;
using ModestTree;

namespace Zenject
{
    public class ScopeArgBinder : ArgumentsBinder
    {
        public ScopeArgBinder(BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        public ArgumentsBinder AsSingle()
        {
            return AsSingle(null);
        }

        public ArgumentsBinder AsSingle(string concreteIdentifier)
        {
            Assert.IsNull(BindInfo.ConcreteIdentifier);

            BindInfo.Scope = ScopeTypes.Singleton;
            BindInfo.ConcreteIdentifier = concreteIdentifier;
            return this;
        }

        public ArgumentsBinder AsCached()
        {
            BindInfo.Scope = ScopeTypes.Cached;
            return this;
        }

        // Note that this is the default so it's not necessary to call this
        public ArgumentsBinder AsTransient()
        {
            BindInfo.Scope = ScopeTypes.Transient;
            return this;
        }
    }
}
