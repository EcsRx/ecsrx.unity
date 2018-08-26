using System;
using ModestTree;
using System.Linq;

namespace Zenject
{
    public class MemoryPoolBindingFinalizer<TContract> : ProviderBindingFinalizer
    {
        readonly MemoryPoolBindInfo _poolBindInfo;
        readonly FactoryBindInfo _factoryBindInfo;

        public MemoryPoolBindingFinalizer(
            BindInfo bindInfo, FactoryBindInfo factoryBindInfo, MemoryPoolBindInfo poolBindInfo)
            : base(bindInfo)
        {
            // Note that it doesn't derive from MemoryPool<TContract>
            // when used with To<>, so we can only check IMemoryPoolBase
            Assert.That(factoryBindInfo.FactoryType.DerivesFrom<IMemoryPool>());

            _factoryBindInfo = factoryBindInfo;
            _poolBindInfo = poolBindInfo;
        }

        protected override void OnFinalizeBinding(DiContainer container)
        {
            var factory = new FactoryProviderWrapper<TContract>(
                _factoryBindInfo.ProviderFunc(container), new InjectContext(container, typeof(TContract)));

            var settings = new MemoryPoolSettings(
                _poolBindInfo.InitialSize, _poolBindInfo.MaxSize, _poolBindInfo.ExpandMethod);

            RegisterProviderForAllContracts(
                container,
                BindingUtil.CreateCachedProvider(
                    new TransientProvider(
                        _factoryBindInfo.FactoryType,
                        container,
                        _factoryBindInfo.Arguments.Concat(
                            InjectUtil.CreateArgListExplicit(factory, settings)).ToList(),
                        BindInfo.ContextInfo, BindInfo.ConcreteIdentifier)));
        }
    }
}

