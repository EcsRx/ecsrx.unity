using ModestTree.Util;
using System;

namespace Zenject
{
    public interface ICommand
    {
    }

    // Zero params
    public abstract class Command : ICommand
    {
        Action _handler;

        [Inject]
        public void Construct(Action handler)
        {
            _handler = handler;
        }

        public void Execute()
        {
            _handler();
        }
    }

    // One param
    public abstract class Command<TParam1> : ICommand
    {
        Action<TParam1> _handler;

        [Inject]
        public void Construct(Action<TParam1> handler)
        {
            _handler = handler;
        }

        public void Execute(TParam1 param1)
        {
            _handler(param1);
        }
    }

    // Two params
    public abstract class Command<TParam1, TParam2> : ICommand
    {
        Action<TParam1, TParam2> _handler;

        [Inject]
        public void Construct(Action<TParam1, TParam2> handler)
        {
            _handler = handler;
        }

        public void Execute(TParam1 param1, TParam2 param2)
        {
            _handler(param1, param2);
        }
    }

    // Three params
    public abstract class Command<TParam1, TParam2, TParam3> : ICommand
    {
        Action<TParam1, TParam2, TParam3> _handler;

        [Inject]
        public void Construct(Action<TParam1, TParam2, TParam3> handler)
        {
            _handler = handler;
        }

        public void Execute(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            _handler(param1, param2, param3);
        }
    }

    // Four params
    public abstract class Command<TParam1, TParam2, TParam3, TParam4> : ICommand
    {
        Action<TParam1, TParam2, TParam3, TParam4> _handler;

        [Inject]
        public void Construct(Action<TParam1, TParam2, TParam3, TParam4> handler)
        {
            _handler = handler;
        }

        public void Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            _handler(param1, param2, param3, param4);
        }
    }

    // Five params
    public abstract class Command<TParam1, TParam2, TParam3, TParam4, TParam5> : ICommand
    {
        ModestTree.Util.Action<TParam1, TParam2, TParam3, TParam4, TParam5> _handler;

        [Inject]
        public void Construct(ModestTree.Util.Action<TParam1, TParam2, TParam3, TParam4, TParam5> handler)
        {
            _handler = handler;
        }

        public void Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            _handler(param1, param2, param3, param4, param5);
        }
    }

    // Six params
    public abstract class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : ICommand
    {
        ModestTree.Util.Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> _handler;

        [Inject]
        public void Construct(ModestTree.Util.Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> handler)
        {
            _handler = handler;
        }

        public void Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            _handler(param1, param2, param3, param4, param5, param6);
        }
    }
}
