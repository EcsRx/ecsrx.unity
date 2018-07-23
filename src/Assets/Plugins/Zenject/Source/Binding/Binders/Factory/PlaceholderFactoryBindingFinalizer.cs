using System;
using ModestTree;
using System.Linq;

namespace Zenject
{
    public class PlaceholderFactoryBindingFinalizer<TContract> : ProviderBindingFinalizer
    {
        readonly FactoryBindInfo _factoryBindInfo;

        public PlaceholderFactoryBindingFinalizer(
            BindInfo bindInfo, FactoryBindInfo factoryBindInfo)
            : base(bindInfo)
        {
            // Note that it doesn't derive from PlaceholderFactory<TContract>
            // when used with To<>, so we can only check IPlaceholderFactory
            Assert.That(factoryBindInfo.FactoryType.DerivesFrom<IPlaceholderFactory>());

            _factoryBindInfo = factoryBindInfo;
        }

        protected override void OnFinalizeBinding(DiContainer container)
        {
            var provider = _factoryBindInfo.ProviderFunc(container);

            RegisterProviderForAllContracts(
                container,
                BindingUtil.CreateCachedProvider(
                    new TransientProvider(
                        _factoryBindInfo.FactoryType,
                        container,
                        _factoryBindInfo.Arguments.Concat(
                            InjectUtil.CreateArgListExplicit(
                                provider,
                                new InjectContext(container, typeof(TContract)))).ToList(),
                        BindInfo.ContextInfo, BindInfo.ConcreteIdentifier)));
        }
    }
}
