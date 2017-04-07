using System;
using System.Xml.Linq;
using Persistity.Serialization;
using Persistity.Serialization.Xml;
using UniRx;

namespace EcsRx.Persistence.TypeHandlers.Reactive
{
    public class ReactiveXmlTypeHandler : ITypeHandler<XElement, XElement>
    {
        private readonly XmlPrimitiveSerializer _primitiveSerializer;
        private readonly XmlPrimitiveDeserializer _primitiveDeserializer;

        public ReactiveXmlTypeHandler()
        {
            _primitiveSerializer = new XmlPrimitiveSerializer();
            _primitiveDeserializer = new XmlPrimitiveDeserializer();
        }

        public bool MatchesType(Type type)
        {
            return (type.GetGenericTypeDefinition() == typeof(ReactiveProperty<>));
        }

        public void HandleTypeSerialization(XElement state, object data, Type type)
        {
            var underlyingType = type.GetGenericArguments()[0];
            var underlyingValue = GetValue(data, underlyingType);
            _primitiveSerializer.SerializeDefaultPrimitive(underlyingValue, underlyingType, state);
        }

        public object HandleTypeDeserialization(XElement state, Type type)
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