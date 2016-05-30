using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public interface IBindingFinalizer
    {
        bool InheritInSubContainers
        {
            get;
        }

        void FinalizeBinding(DiContainer container);
    }
}
