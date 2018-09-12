using System;
using System.IO;
using EcsRx.Persistence.Primitives.Checkers;
using EcsRx.Persistence.Primitives.Helpers;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Serialization.Binary.Handlers;

namespace EcsRx.Persistence.Primitives.Handlers
{
    public class ReactiveBinaryPrimitiveHandler : IBinaryPrimitiveHandler
    {
        private readonly BasicBinaryPrimitiveHandler _basicPrimitiveHandler;
        private readonly ReactiveValueHandler _reactiveValueHandler;
        
        public IPrimitiveChecker PrimitiveChecker { get; } = new ReactivePrimitiveChecker();
        
        public ReactiveBinaryPrimitiveHandler()
        {
            _basicPrimitiveHandler = new BasicBinaryPrimitiveHandler();
            _reactiveValueHandler = new ReactiveValueHandler();
        }
        
        public void Serialize(BinaryWriter state, object data, Type type)
        {
            var underlyingType = type.GetGenericArguments()[0];
            var underlyingValue = _reactiveValueHandler.GetValue(data, underlyingType);
            _basicPrimitiveHandler.Serialize(state, underlyingValue, underlyingType);
        }

        public object Deserialize(BinaryReader state, Type type)
        {
            var underlyingType = type.GetGenericArguments()[0];
            var data = _basicPrimitiveHandler.Deserialize(state, underlyingType);
            var reactiveProperty = Activator.CreateInstance(type);
            _reactiveValueHandler.SetValue(reactiveProperty, underlyingType, data);
            return reactiveProperty;
        }
    }
}