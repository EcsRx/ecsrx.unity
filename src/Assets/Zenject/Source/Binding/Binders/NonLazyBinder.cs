using System;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class NonLazyBinder
    {
        public NonLazyBinder(BindInfo bindInfo)
        {
            BindInfo = bindInfo;
        }

        protected BindInfo BindInfo
        {
            get;
            private set;
        }

        public void NonLazy()
        {
            BindInfo.NonLazy = true;
        }
    }
}
