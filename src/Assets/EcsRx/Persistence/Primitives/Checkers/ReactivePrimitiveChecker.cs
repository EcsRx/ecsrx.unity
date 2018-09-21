using System;
using LazyData.Mappings.Types.Primitives.Checkers;
using ModestTree;
using UniRx;

namespace EcsRx.Persistence.Primitives.Checkers
{
    public class ReactivePrimitiveChecker : IPrimitiveChecker
    {
        public bool IsPrimitive(Type type)
        {
            if(!type.IsGenericType) { return false; }
            return type.GetGenericTypeDefinition().IsAssignableFrom(typeof(ReactiveProperty<>));
        }
    }
}