using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public class BindFinalizerWrapper : IBindingFinalizer
    {
        IBindingFinalizer _subFinalizer;

        public IBindingFinalizer SubFinalizer
        {
            set
            {
                _subFinalizer = value;
            }
        }

        public bool InheritInSubContainers
        {
            get
            {
                return _subFinalizer.InheritInSubContainers;
            }
        }

        public void FinalizeBinding(DiContainer container)
        {
            Assert.IsNotNull(_subFinalizer,
                "Unfinished binding! Finalizer was not given.");

            _subFinalizer.FinalizeBinding(container);
        }
    }
}
