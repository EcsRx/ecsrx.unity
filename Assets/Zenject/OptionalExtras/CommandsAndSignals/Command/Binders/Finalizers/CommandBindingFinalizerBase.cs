using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;

namespace Zenject
{
    // Zero Parameters

    public abstract class CommandBindingFinalizerBase<TCommand, THandler, TAction>
        : ProviderBindingFinalizer
        where TCommand : ICommand
    {
        readonly Func<IProvider> _handlerProviderFactory;

        public CommandBindingFinalizerBase(
            BindInfo bindInfo,
            Func<IProvider> handlerProviderFactory)
            : base(bindInfo)
        {
            _handlerProviderFactory = handlerProviderFactory;
        }

        protected override void OnFinalizeBinding(DiContainer container)
        {
            Assert.That(BindInfo.ContractTypes.IsLength(1));
            Assert.IsEqual(BindInfo.ContractTypes.Single(), typeof(TCommand));

            // Note that the singleton here applies to the handler, not the command class
            // The command itself is always cached
            RegisterProvider<TCommand>(
                container,
                new CachedProvider(
                    new TransientProvider(
                        typeof(TCommand), container,
                        InjectUtil.CreateArgListExplicit(GetCommandAction(container)), null)));
        }

        // The returned delegate is executed every time the command is executed
        TAction GetCommandAction(DiContainer container)
        {
            var handlerProvider = GetHandlerProvider(container);
            var handlerInjectContext = new InjectContext(container, typeof(THandler));

            return GetCommandAction(handlerProvider, handlerInjectContext);
        }

        IProvider GetHandlerProvider(DiContainer container)
        {
            switch (BindInfo.Scope)
            {
                case ScopeTypes.Singleton:
                {
                    return container.SingletonProviderCreator.CreateProviderStandard(
                        new StandardSingletonDeclaration(
                            typeof(THandler),
                            BindInfo.ConcreteIdentifier,
                            BindInfo.Arguments,
                            SingletonTypes.To,
                            null),
                        (_, type) => _handlerProviderFactory());
                }
                case ScopeTypes.Transient:
                {
                    return _handlerProviderFactory();
                }
                case ScopeTypes.Cached:
                {
                    return new CachedProvider(
                        _handlerProviderFactory());
                }
            }

            throw Assert.CreateException();
        }

        protected abstract TAction GetCommandAction(
            IProvider handlerProvider, InjectContext handlerContext);
    }
}

