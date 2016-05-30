using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;

namespace Zenject
{
    public interface IFactory
    {
    }

    public interface IFactory<TValue> : IFactory
    {
        TValue Create();
    }

    public interface IFactory<TParam1, TValue> : IFactory
    {
        TValue Create(TParam1 param);
    }

    public interface IFactory<TParam1, TParam2, TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2);
    }

    public interface IFactory<TParam1, TParam2, TParam3, TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3);
    }

    public interface IFactory<TParam1, TParam2, TParam3, TParam4, TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }

    public interface IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5);
    }
}

