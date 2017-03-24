using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Persistity.Exceptions;
using Persistity.Extensions;
using Persistity.Mappings;
using UnityEngine;

namespace Persistity.Serialization.Xml
{
    public class XmlSerializer : IXmlSerializer
    {
        public XmlConfiguration Configuration { get; private set; }

        public XmlSerializer(XmlConfiguration configuration = null)
        { Configuration = configuration ?? XmlConfiguration.Default; }

        private readonly Type[] CatchmentTypes =
        {
            typeof(string), typeof(bool), typeof(byte), typeof(short), typeof(int),
            typeof(long), typeof(Guid), typeof(float), typeof(double), typeof(decimal)
        };

        private void MarkAsNull(XElement element)
        { element.Add(new XAttribute("IsNull", true)); }

        private void SerializePrimitive(object value, Type type, XElement element)
        {
            if (type == typeof(Vector2))
            {
                var typedObject = (Vector2)value;
                element.Add(new XElement("x", typedObject.x));    
                element.Add(new XElement("y", typedObject.y));
                return;
            }
            if (type == typeof(Vector3))
            {
                var typedObject = (Vector3)value;
                element.Add(new XElement("x", typedObject.x));    
                element.Add(new XElement("y", typedObject.y));    
                element.Add(new XElement("z", typedObject.z));
                return;
            }
            if (type == typeof(Vector4))
            {
                var typedObject = (Vector4)value;
                element.Add(new XElement("x", typedObject.x));    
                element.Add(new XElement("y", typedObject.y));    
                element.Add(new XElement("z", typedObject.z));    
                element.Add(new XElement("w", typedObject.w));
                return;
            }
            if (type == typeof(Quaternion))
            {
                var typedObject = (Quaternion)value;
                element.Add(new XElement("x", typedObject.x));    
                element.Add(new XElement("y", typedObject.y));    
                element.Add(new XElement("z", typedObject.z));    
                element.Add(new XElement("w", typedObject.w));
                return;
            }
            if (type == typeof(DateTime))
            {
                var typedValue = (DateTime) value;
                var stringValue = typedValue.ToBinary().ToString();
                element.Value = stringValue;
                return;
            }

            if (type.IsTypeOf(CatchmentTypes) || type.IsEnum)
            {
                element.Value = value.ToString();
                return;
            }

            var matchingHandler = Configuration.TypeHandlers.SingleOrDefault(x => x.MatchesType(type));
            if(matchingHandler == null) { throw new NoKnownTypeException(type); }
            matchingHandler.HandleTypeIn(element, value);
        }

        public byte[] SerializeData<T>(TypeMapping typeMapping, T data) where T : new()
        { return SerializeData(typeMapping, (object) data); }

        public byte[] SerializeData(TypeMapping typeMapping, object data)
        {
            var element = new XElement("Container");
            Serialize(typeMapping.InternalMappings, data, element);
            var xmlString = element.ToString();
            return Configuration.Encoder.GetBytes(xmlString);
        }

        private void SerializeProperty<T>(PropertyMapping propertyMapping, T data, XElement element)
        {
            if (data == null)
            {
                MarkAsNull(element);
                return;
            }
            var underlyingValue = propertyMapping.GetValue(data);

            if (underlyingValue == null)
            {
                MarkAsNull(element);
                return;
            }

            SerializePrimitive(underlyingValue, propertyMapping.Type, element);
        }

        private void SerializeNestedObject<T>(NestedMapping nestedMapping, T data, XElement element)
        {
            if (data == null)
            {
                MarkAsNull(element);
                return;
            }
            var currentData = nestedMapping.GetValue(data);

            if (currentData == null)
            {
                MarkAsNull(element);
                return;
            }
            Serialize(nestedMapping.InternalMappings, currentData, element);
        }
        
        private void SerializeCollection<T>(CollectionMapping collectionMapping, T data, XElement element)
        {
            if (data == null)
            {
                MarkAsNull(element);
                return;
            }

            var collectionValue = collectionMapping.GetValue(data);

            if (collectionValue == null)
            {
                MarkAsNull(element);
                return;
            }

            element.Add(new XAttribute("Count", collectionValue.Count));
            for (var i = 0; i < collectionValue.Count; i++)
            {
                var currentData = collectionValue[i];
                var newElement = new XElement("CollectionElement");
                element.Add(newElement);

                if (currentData == null)
                { MarkAsNull(newElement); }
                else if (collectionMapping.InternalMappings.Count > 0)
                { Serialize(collectionMapping.InternalMappings, currentData, newElement); }
                else
                { SerializePrimitive(currentData, collectionMapping.CollectionType, newElement); }
            }
        }

        private void SerializeDictionary<T>(DictionaryMapping dictionaryMapping, T data, XElement element)
        {
            if (data == null)
            {
                MarkAsNull(element);
                return;
            }

            var dictionaryValue = dictionaryMapping.GetValue(data);

            if (dictionaryValue == null)
            {
                MarkAsNull(element);
                return;
            }

            element.Add(new XAttribute("Count", dictionaryValue.Count));

            foreach (var key in dictionaryValue.Keys)
            {
                var currentValue = dictionaryValue[key];

                var keyElement = new XElement("Key");
                if (dictionaryMapping.KeyMappings.Count > 0)
                { Serialize(dictionaryMapping.KeyMappings, key, keyElement); }
                else
                { SerializePrimitive(key, dictionaryMapping.KeyType, keyElement); }

                var valueElement = new XElement("Value");
                if(currentValue == null)
                { MarkAsNull(valueElement); }
                else if (dictionaryMapping.ValueMappings.Count > 0)
                { Serialize(dictionaryMapping.ValueMappings, currentValue, valueElement); }
                else
                { SerializePrimitive(currentValue, dictionaryMapping.ValueType, valueElement); }

                var keyValuePairElement = new XElement("KeyValuePair");
                keyValuePairElement.Add(keyElement, valueElement);
                element.Add(keyValuePairElement);
            }
        }

        private void Serialize<T>(IEnumerable<Mapping> mappings, T data, XElement element)
        {
            foreach (var mapping in mappings)
            {
                var newElement = new XElement(mapping.LocalName);
                element.Add(newElement);

                if (mapping is PropertyMapping)
                { SerializeProperty((mapping as PropertyMapping), data, newElement); }
                else if (mapping is NestedMapping)
                { SerializeNestedObject((mapping as NestedMapping), data, newElement); }
                else if (mapping is DictionaryMapping)
                { SerializeDictionary((mapping as DictionaryMapping), data, newElement); }
                else
                { SerializeCollection((mapping as CollectionMapping), data, newElement); }
            }
        }
    }
}