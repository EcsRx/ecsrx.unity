/*
using System;
using EcsRx.Persistence.Primitives.Checkers;
using EcsRx.Persistence.Primitives.Helpers;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Serialization.Json.Handlers;
using Newtonsoft.Json.Linq;

namespace EcsRx.Persistence.Primitives.Handlers
{
    
    public class ReactiveJsonPrimitiveHandler : IJsonPrimitiveHandler
    {
        private readonly BasicJsonPrimitiveHandler _basicPrimitiveHandler;
        private readonly ReactiveValueHandler _reactiveValueHandler;
        
        public IPrimitiveChecker PrimitiveChecker { get; } = new ReactivePrimitiveChecker();
        
        public ReactiveJsonPrimitiveHandler()
        {
            _basicPrimitiveHandler = new BasicJsonPrimitiveHandler();
            _reactiveValueHandler = new ReactiveValueHandler();
        }
        
        public void Serialize(JToken state, object data, Type type)
        {
            var underlyingType = type.GetGenericArguments()[0];
            var underlyingValue = _reactiveValueHandler.GetValue(data, underlyingType);
            _basicPrimitiveHandler.Serialize(state, underlyingValue, underlyingType);
        }

        public object Deserialize(JToken state, Type type)
        {
            var underlyingType = type.GetGenericArguments()[0];
            var data = _basicPrimitiveHandler.Deserialize(state, underlyingType);
            var reactiveProperty = Activator.CreateInstance(type);
            _reactiveValueHandler.SetValue(reactiveProperty, underlyingType, data);
            return reactiveProperty;
        }
    }
}
*/