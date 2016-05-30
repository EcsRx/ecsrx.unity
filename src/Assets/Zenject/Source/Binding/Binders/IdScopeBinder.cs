using System;
using ModestTree;

namespace Zenject
{
    public class IdScopeBinder : ScopeBinder
    {
        public IdScopeBinder(BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        public ScopeBinder WithId(object identifier)
        {
            BindInfo.Identifier = identifier;
            return this;
        }
    }
}
