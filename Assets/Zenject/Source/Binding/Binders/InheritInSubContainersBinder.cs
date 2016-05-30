using System;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class InheritInSubContainersBinder : NonLazyBinder
    {
        public InheritInSubContainersBinder(BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        public NonLazyBinder InheritInSubContainers()
        {
            BindInfo.InheritInSubContainers = true;
            return this;
        }
    }
}

