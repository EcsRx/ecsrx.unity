using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public interface ISignalHandler
    {
        void Execute(object[] args);
    }
}
