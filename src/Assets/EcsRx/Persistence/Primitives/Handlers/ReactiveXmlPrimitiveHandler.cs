using System;
using System.Xml.Linq;
using EcsRx.Persistence.Primitives.Checkers;
using EcsRx.Persistence.Primitives.Helpers;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Xml.Handlers;

namespace EcsRx.Persistence.Primitives.Handlers
{
    
    public class ReactiveXmlPrimitiveHandler : IXmlPrimitiveHandler
    {
        private readonly BasicXmlPrimitiveHandler _basicPrimitiveHandler;
        private readonly ReactiveValueHandler _reactiveValueHandler;
        
        public IPrimitiveChecker PrimitiveChecker { get; } = new ReactivePrimitiveChecker();
        
        public ReactiveXmlPrimitiveHandler()
        {
            _basicPrimitiveHandler = new BasicXmlPrimitiveHandler();
            _reactiveValueHandler = new ReactiveValueHandler();
        }
        
        public void Serialize(XElement state, object data, Type type)
        {
            var underlyingType = type.GetGenericArguments()[0];
            var underlyingValue = _reactiveValueHandler.GetValue(data, underlyingType);
            _basicPrimitiveHandler.Serialize(state, underlyingValue, underlyingType);
        }

        public object Deserialize(XElement state, Type type)
        {
            var underlyingType = type.GetGenericArguments()[0];
            var data = _basicPrimitiveHandler.Deserialize(state, underlyingType);
            var reactiveProperty = Activator.CreateInstance(type);
            _reactiveValueHandler.SetValue(reactiveProperty, underlyingType, data);
            return reactiveProperty;
        }
    }
}