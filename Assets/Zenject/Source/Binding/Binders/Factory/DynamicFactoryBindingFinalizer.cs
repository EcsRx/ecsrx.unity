using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public class DynamicFactoryBindingFinalizer<TContract> : ProviderBindingFinalizer
    {
        readonly Func<DiContainer, IProvider> _providerFunc;
        readonly Type _factoryType;

        public DynamicFactoryBindingFinalizer(
            BindInfo bindInfo, Type factoryType, Func<DiContainer, IProvider> providerFunc)
            : base(bindInfo)
        {
            // Note that it doesn't derive from Factory<TContract>
            // when used with To<>, so we can only check IDynamicFactory
            Assert.That(factoryType.DerivesFrom<IDynamicFactory>());

            _factoryType = factoryType;
            _providerFunc = providerFunc;
        }

        protected override void OnFinalizeBinding(DiContainer container)
        {
            var provider = _providerFunc(container);

            RegisterProviderForAllContracts(
                container,
                new CachedProvider(
                    new TransientProvider(
                        _factoryType,
                        container,
                        InjectUtil.CreateArgListExplicit(
                            provider,
                            new InjectContext(container, typeof(TContract))),
                            null)));
        }
    }
}
