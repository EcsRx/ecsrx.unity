using System;
using Newtonsoft.Json.Linq;
using Persistity.Serialization;
using Persistity.Serialization.Json;
using UniRx;

namespace EcsRx.Persistence.TypeHandlers.Reactive
{
    public class ReactiveJsonTypeHandler : ITypeHandler<JToken, JToken>
    {
        private readonly JsonPrimitiveSerializer _primitiveSerializer;
        private readonly JsonPrimitiveDeserializer _primitiveDeserializer;

        public ReactiveJsonTypeHandler()
        {
            _primitiveSerializer = new JsonPrimitiveSerializer();
            _primitiveDeserializer = new JsonPrimitiveDeserializer();
        }

        public bool MatchesType(Type type)
        {
            return (type.GetGenericTypeDefinition() == typeof(ReactiveProperty<>));
        }

        public void HandleTypeSerialization(JToken state, object data, Type type)
        {
            var underlyingType = type.GetGenericArguments()[0];
            var underlyingValue = GetValue(data, underlyingType);
            _primitiveSerializer.SerializeDefaultPrimitive(underlyingValue, underlyingType, state);
        }

        public object HandleTypeDeserialization(JToken state, Type type)
        {
            var underlyingType = type.GetGenericArguments()[0];
            var data = _primitiveDeserializer.DeserializeDefaultPrimitive(underlyingType, state);
            var reactiveProperty = Activator.CreateInstance(type);;
            SetValue(reactiveProperty, underlyingType, data);
            return reactiveProperty;
        }

        private void SetValue(object reactiveProperty, Type genericType, object newValue)
        {
            var typedGeneric = typeof(ReactiveProperty<>).MakeGenericType(genericType);
            var propertyInfo = typedGeneric.GetProperty("Value");
            propertyInfo.SetValue(reactiveProperty, newValue, null);
        }

        private object GetValue(object reactiveProperty, Type genericType)
        {
            var typedGeneric = typeof(ReactiveProperty<>).MakeGenericType(genericType);
            var propertyInfo = typedGeneric.GetProperty("Value");
            return propertyInfo.GetValue(reactiveProperty, null);
        }
    }
}