using System;
using Zenject;

namespace Zenject.TestFramework
{
    public class InitMethodHandler : IInitializable
    {
        readonly Action _method;

        public InitMethodHandler(Action method)
        {
            _method = method;
        }

        public void Initialize()
        {
            _method();
        }
    }

    public class InitMethodHandler<TParam1> : IInitializable
    {
        readonly TParam1 _param1;
        readonly Action<TParam1> _method;

        public InitMethodHandler(
            Action<TParam1> method,
            TParam1 param1)
        {
            _param1 = param1;
            _method = method;
        }

        public void Initialize()
        {
            _method(_param1);
        }
    }

    public class InitMethodHandler<TParam1, TParam2> : IInitializable
    {
        readonly TParam1 _param1;
        readonly TParam2 _param2;
        readonly Action<TParam1, TParam2> _method;

        public InitMethodHandler(
            Action<TParam1, TParam2> method,
            TParam1 param1,
            TParam2 param2)
        {
            _param1 = param1;
            _param2 = param2;
            _method = method;
        }

        public void Initialize()
        {
            _method(_param1, _param2);
        }
    }
}
