using System;
using System.IO;
using Persistity.Serialization;
using Persistity.Serialization.Binary;
using UniRx;

namespace EcsRx.Persistence.TypeHandlers.Reactive
{
    public class ReactiveBinaryTypeHandler : ITypeHandler<BinaryWriter, BinaryReader>
    {
        private readonly BinaryPrimitiveSerializer _primitiveSerializer;
        private readonly BinaryPrimitiveDeserializer _primitiveDeserializer;

        public ReactiveBinaryTypeHandler()
        {
            _primitiveSerializer = new BinaryPrimitiveSerializer();
            _primitiveDeserializer = new BinaryPrimitiveDeserializer();
        }

        public bool MatchesType(Type type)
        {
            return (type.GetGenericTypeDefinition() == typeof(ReactiveProperty<>));
        }

        public void HandleTypeSerialization(BinaryWriter state, object data, Type type)
        {
            var underlyingType = type.GetGenericArguments()[0];
            var underlyingValue = GetValue(data, underlyingType);
            _primitiveSerializer.SerializeDefaultPrimitive(underlyingValue, underlyingType, state);
        }

        public object HandleTypeDeserialization(BinaryReader state, Type type)
        {
            var underlyingType = type.GetGenericArguments()[0];
            var data = _primitiveDeserializer.DeserializeDefaultPrimitive(underlyingType, state);
            var reactiveProperty = Activator.CreateInstance(type); ;
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