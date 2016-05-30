using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;

namespace Zenject
{
    // Zero parameter

    public class CommandBindingFinalizer<TCommand, THandler>
        : CommandBindingFinalizerBase<TCommand, THandler, Action>
        where TCommand : Command
    {
        readonly Func<THandler, Action> _methodGetter;

        public CommandBindingFinalizer(
            BindInfo bindInfo,
            Func<THandler, Action> methodGetter, Func<IProvider> handlerProviderFactory)
            : base(bindInfo, handlerProviderFactory)
        {
            _methodGetter = methodGetter;
        }

        // The returned delegate is executed every time the command is executed
        protected override Action GetCommandAction(
            IProvider handlerProvider, InjectContext handlerInjectContext)
        {
            // Here we lazily create/get the handler instance when the command is called
            // If using AsSingle this might re-use an existing instance
            // If using AsTransient this will create a new instance
            return () =>
            {
                var handler = (THandler)handlerProvider.TryGetInstance(handlerInjectContext);

                // Null check is necessary when using ToOptionalResolve
                if (handler != null)
                {
                    _methodGetter(handler)();
                }
            };
        }
    }

    // One parameter

    public class CommandBindingFinalizer<TCommand, THandler, TParam1>
        : CommandBindingFinalizerBase<TCommand, THandler, Action<TParam1>>
        where TCommand : Command<TParam1>
    {
        readonly Func<THandler, Action<TParam1>> _methodGetter;

        public CommandBindingFinalizer(
            BindInfo bindInfo,
            Func<THandler, Action<TParam1>> methodGetter,
            Func<IProvider> handlerProviderFactory)
            : base(bindInfo, handlerProviderFactory)
        {
            _methodGetter = methodGetter;
        }

        // The returned delegate is executed every time the command is executed
        protected override Action<TParam1> GetCommandAction(
            IProvider handlerProvider, InjectContext handlerInjectContext)
        {
            // Here we lazily create/get the handler instance when the command is called
            // If using AsSingle this might re-use an existing instance
            // If using AsTransient this will create a new instance
            return (p1) =>
            {
                var handler = (THandler)handlerProvider.TryGetInstance(handlerInjectContext);

                // Null check is necessary when using ToOptionalResolve
                if (handler != null)
                {
                    _methodGetter(handler)(p1);
                }
            };
        }
    }

    // Two parameters

    public class CommandBindingFinalizer<TCommand, THandler, TParam1, TParam2>
        : CommandBindingFinalizerBase<TCommand, THandler, Action<TParam1, TParam2>>
        where TCommand : Command<TParam1, TParam2>
    {
        readonly Func<THandler, Action<TParam1, TParam2>> _methodGetter;

        public CommandBindingFinalizer(
            BindInfo bindInfo,
            Func<THandler, Action<TParam1, TParam2>> methodGetter,
            Func<IProvider> handlerProviderFactory)
            : base(bindInfo, handlerProviderFactory)
        {
            _methodGetter = methodGetter;
        }

        // The returned delegate is executed every time the command is executed
        protected override Action<TParam1, TParam2> GetCommandAction(
            IProvider handlerProvider, InjectContext handlerInjectContext)
        {
            // Here we lazily create/get the handler instance when the command is called
            // If using AsSingle this might re-use an existing instance
            // If using AsTransient this will create a new instance
            return (p1, p2) =>
            {
                var handler = (THandler)handlerProvider.TryGetInstance(handlerInjectContext);

                // Null check is necessary when using ToOptionalResolve
                if (handler != null)
                {
                    _methodGetter(handler)(p1, p2);
                }
            };
        }
    }

    // Three parameters

    public class CommandBindingFinalizer<TCommand, THandler, TParam1, TParam2, TParam3>
        : CommandBindingFinalizerBase<TCommand, THandler, Action<TParam1, TParam2, TParam3>>
        where TCommand : Command<TParam1, TParam2, TParam3>
    {
        readonly Func<THandler, Action<TParam1, TParam2, TParam3>> _methodGetter;

        public CommandBindingFinalizer(
            BindInfo bindInfo,
            Func<THandler, Action<TParam1, TParam2, TParam3>> methodGetter,
            Func<IProvider> handlerProviderFactory)
            : base(bindInfo, handlerProviderFactory)
        {
            _methodGetter = methodGetter;
        }

        // The returned delegate is executed every time the command is executed
        protected override Action<TParam1, TParam2, TParam3> GetCommandAction(
            IProvider handlerProvider, InjectContext handlerInjectContext)
        {
            // Here we lazily create/get the handler instance when the command is called
            // If using AsSingle this might re-use an existing instance
            // If using AsTransient this will create a new instance
            return (p1, p2, p3) =>
            {
                var handler = (THandler)handlerProvider.TryGetInstance(handlerInjectContext);

                // Null check is necessary when using ToOptionalResolve
                if (handler != null)
                {
                    _methodGetter(handler)(p1, p2, p3);
                }
            };
        }
    }

    // Four parameters

    public class CommandBindingFinalizer<TCommand, THandler, TParam1, TParam2, TParam3, TParam4>
        : CommandBindingFinalizerBase<TCommand, THandler, Action<TParam1, TParam2, TParam3, TParam4>>
        where TCommand : Command<TParam1, TParam2, TParam3, TParam4>
    {
        readonly Func<THandler, Action<TParam1, TParam2, TParam3, TParam4>> _methodGetter;

        public CommandBindingFinalizer(
            BindInfo bindInfo,
            Func<THandler, Action<TParam1, TParam2, TParam3, TParam4>> methodGetter,
            Func<IProvider> handlerProviderFactory)
            : base(bindInfo, handlerProviderFactory)
        {
            _methodGetter = methodGetter;
        }

        // The returned delegate is executed every time the command is executed
        protected override Action<TParam1, TParam2, TParam3, TParam4> GetCommandAction(
            IProvider handlerProvider, InjectContext handlerInjectContext)
        {
            // Here we lazily create/get the handler instance when the command is called
            // If using AsSingle this might re-use an existing instance
            // If using AsTransient this will create a new instance
            return (p1, p2, p3, p4) =>
            {
                var handler = (THandler)handlerProvider.TryGetInstance(handlerInjectContext);

                // Null check is necessary when using ToOptionalResolve
                if (handler != null)
                {
                    _methodGetter(handler)(p1, p2, p3, p4);
                }
            };
        }
    }

    // Five parameters

    public class CommandBindingFinalizer<TCommand, THandler, TParam1, TParam2, TParam3, TParam4, TParam5>
        : CommandBindingFinalizerBase<TCommand, THandler, ModestTree.Util.Action<TParam1, TParam2, TParam3, TParam4, TParam5>>
        where TCommand : Command<TParam1, TParam2, TParam3, TParam4, TParam5>
    {
        readonly Func<THandler, ModestTree.Util.Action<TParam1, TParam2, TParam3, TParam4, TParam5>> _methodGetter;

        public CommandBindingFinalizer(
            BindInfo bindInfo,
            Func<THandler, ModestTree.Util.Action<TParam1, TParam2, TParam3, TParam4, TParam5>> methodGetter,
            Func<IProvider> handlerProviderFactory)
            : base(bindInfo, handlerProviderFactory)
        {
            _methodGetter = methodGetter;
        }

        // The returned delegate is executed every time the command is executed
        protected override ModestTree.Util.Action<TParam1, TParam2, TParam3, TParam4, TParam5> GetCommandAction(
            IProvider handlerProvider, InjectContext handlerInjectContext)
        {
            // Here we lazily create/get the handler instance when the command is called
            // If using AsSingle this might re-use an existing instance
            // If using AsTransient this will create a new instance
            return (p1, p2, p3, p4, p5) =>
            {
                var handler = (THandler)handlerProvider.TryGetInstance(handlerInjectContext);

                // Null check is necessary when using ToOptionalResolve
                if (handler != null)
                {
                    _methodGetter(handler)(p1, p2, p3, p4, p5);
                }
            };
        }
    }
}

