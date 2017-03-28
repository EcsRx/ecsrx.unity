using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Persistity.Mappings;
using Persistity.Registries;
using UnityEngine;

namespace Persistity.Serialization.Xml
{
    public class XmlDeserializer : IXmlDeserializer
    {
        public IMappingRegistry MappingRegistry { get; private set; }
        public XmlConfiguration Configuration { get; private set; }

        public XmlDeserializer(IMappingRegistry mappingRegistry, XmlConfiguration configuration = null)
        {
            MappingRegistry = mappingRegistry;
            Configuration = configuration ?? XmlConfiguration.Default;
        }

        private bool IsElementNull(XElement element)
        { return element.Attribute("IsNull") != null; }

        private object DeserializePrimitive(Type type, XElement element)
        {
            if (type == typeof(byte)) { return byte.Parse(element.Value); }
            if (type == typeof(short)) { return short.Parse(element.Value); }
            if (type == typeof(int)) { return int.Parse(element.Value); }
            if (type == typeof(long)) { return long.Parse(element.Value); }
            if (type == typeof(bool)) { return bool.Parse(element.Value); }
            if (type == typeof(float)) { return float.Parse(element.Value); }
            if (type == typeof(double)) { return double.Parse(element.Value); }
            if (type == typeof(decimal)) { return decimal.Parse(element.Value); }
            if (type.IsEnum) { return Enum.Parse(type, element.Value); }
            if (type == typeof(Vector2))
            {
                var x = float.Parse(element.Element("x").Value);
                var y = float.Parse(element.Element("y").Value);
                return new Vector2(x, y);
            }
            if (type == typeof(Vector3))
            {
                var x = float.Parse(element.Element("x").Value);
                var y = float.Parse(element.Element("y").Value);
                var z = float.Parse(element.Element("z").Value);
                return new Vector3(x, y, z);
            }
            if (type == typeof(Vector4))
            {
                var x = float.Parse(element.Element("x").Value);
                var y = float.Parse(element.Element("y").Value);
                var z = float.Parse(element.Element("z").Value);
                var w = float.Parse(element.Element("w").Value);
                return new Vector4(x, y, z, w);
            }
            if (type == typeof(Quaternion))
            {
                var x = float.Parse(element.Element("x").Value);
                var y = float.Parse(element.Element("y").Value);
                var z = float.Parse(element.Element("z").Value);
                var w = float.Parse(element.Element("w").Value);
                return new Quaternion(x, y, z, w);
            }
            if (type == typeof(Guid))
            {
                return new Guid(element.Value);
            }
            if (type == typeof(DateTime))
            {
                var binaryTime = long.Parse(element.Value);
                return DateTime.FromBinary(binaryTime);
            }

            var matchingHandler = Configuration.TypeHandlers.SingleOrDefault(x => x.MatchesType(type));
            if (matchingHandler != null)
            { return matchingHandler.HandleTypeOut(element); }

            return element.Value;
        }

        public T Deserialize<T>(DataObject data) where T: new()
        { return (T) Deserialize(data); }

        public object Deserialize(DataObject data)
        {
            var xDoc = XDocument.Parse(data.AsString);
            var containerElement = xDoc.Element("Container");
            var typeName = containerElement.Element("Type").Value;
            var type = Type.GetType(typeName);
            var typeMapping = MappingRegistry.GetMappingFor(type);

            var instance = Activator.CreateInstance(typeMapping.Type);
            Deserialize(typeMapping.InternalMappings, instance, containerElement);
            return instance;
        }

        private void DeserializeProperty<T>(PropertyMapping propertyMapping, T instance, XElement element)
        {
            if (IsElementNull(element))
            { propertyMapping.SetValue(instance, null); }
            else
            {
                var underlyingValue = DeserializePrimitive(propertyMapping.Type, element);
                propertyMapping.SetValue(instance, underlyingValue);
            }
        }

        private void DeserializeNestedObject<T>(NestedMapping nestedMapping, T instance, XElement element)
        { Deserialize(nestedMapping.InternalMappings, instance, element); }

        private void DeserializeCollection(CollectionMapping collectionMapping, IList collectionInstance, int arrayCount, XElement element)
        {
            for (var i = 0; i < arrayCount; i++)
            {
                var collectionElement = element.Elements("CollectionElement").ElementAt(i);
                if (IsElementNull(collectionElement))
                {
                    if (collectionInstance.IsFixedSize)
                    { collectionInstance[i] = null; }
                    else
                    { collectionInstance.Insert(i, null); }
                }
                else if (collectionMapping.InternalMappings.Count > 0)
                {
                    var elementInstance = Activator.CreateInstance(collectionMapping.CollectionType);
                    Deserialize(collectionMapping.InternalMappings, elementInstance, collectionElement);

                    if (collectionInstance.IsFixedSize)
                    { collectionInstance[i] = elementInstance; }
                    else
                    { collectionInstance.Insert(i, elementInstance); }
                }
                else
                {
                    var value = DeserializePrimitive(collectionMapping.CollectionType, collectionElement);
                    if (collectionInstance.IsFixedSize)
                    { collectionInstance[i] = value; }
                    else
                    { collectionInstance.Insert(i, value); }
                }
            }
        }

        private void DeserializeDictionary(DictionaryMapping dictionaryMapping, IDictionary dictionaryInstance, int dictionaryCount, XElement element)
        {
            for (var i = 0; i < dictionaryCount; i++)
            {
                object keyInstance, valueInstance;
                var keyValuePairElement = element.Elements("KeyValuePair").ElementAt(i);
                var keyElement = keyValuePairElement.Element("Key");
                var valueElement = keyValuePairElement.Element("Value");

                if (dictionaryMapping.KeyMappings.Count > 0)
                {
                    keyInstance = Activator.CreateInstance(dictionaryMapping.KeyType);
                    Deserialize(dictionaryMapping.KeyMappings, keyInstance, keyElement);
                }
                else
                { keyInstance = DeserializePrimitive(dictionaryMapping.KeyType, keyElement); }

                if (IsElementNull(valueElement))
                { valueInstance = null; }
                else if (dictionaryMapping.ValueMappings.Count > 0)
                {
                    valueInstance = Activator.CreateInstance(dictionaryMapping.ValueType);
                    Deserialize(dictionaryMapping.ValueMappings, valueInstance, valueElement);
                }
                else
                { valueInstance = DeserializePrimitive(dictionaryMapping.ValueType, valueElement); }

                dictionaryInstance.Add(keyInstance, valueInstance);
            }
        }

        private void Deserialize<T>(IEnumerable<Mapping> mappings, T instance, XElement element)
        {
            foreach (var mapping in mappings)
            {
                var currentElement = element.Element(mapping.LocalName);

                if (mapping is PropertyMapping)
                { DeserializeProperty((mapping as PropertyMapping), instance, currentElement); }
                else if (mapping is NestedMapping)
                {
                    var nestedMapping = (mapping as NestedMapping);
                    if (IsElementNull(currentElement))
                    {
                        nestedMapping.SetValue(instance, null);
                        continue;
                    }

                    var childInstance = Activator.CreateInstance(nestedMapping.Type);
                    DeserializeNestedObject(nestedMapping, childInstance, currentElement);
                    nestedMapping.SetValue(instance, childInstance);
                }
                else if (mapping is DictionaryMapping)
                {
                    var dictionaryMapping = (mapping as DictionaryMapping);
                    if (IsElementNull(currentElement))
                    {
                        dictionaryMapping.SetValue(instance, null);
                        continue;
                    }

                    var dictionarytype = typeof(Dictionary<,>);
                    var dictionaryCount = int.Parse(currentElement.Attribute("Count").Value);
                    var constructedDictionaryType = dictionarytype.MakeGenericType(dictionaryMapping.KeyType, dictionaryMapping.ValueType);
                    var dictionary = (IDictionary)Activator.CreateInstance(constructedDictionaryType);
                    DeserializeDictionary(dictionaryMapping, dictionary, dictionaryCount, currentElement);
                    dictionaryMapping.SetValue(instance, dictionary);
                }
                else
                {
                    var collectionMapping = (mapping as CollectionMapping);
                    if (IsElementNull(currentElement))
                    {
                        collectionMapping.SetValue(instance, null);
                        continue;
                    }

                    var arrayCount = int.Parse(currentElement.Attribute("Count").Value);

                    if (collectionMapping.IsArray)
                    {
                        var arrayInstance = (IList) Activator.CreateInstance(collectionMapping.Type, arrayCount);
                        DeserializeCollection(collectionMapping, arrayInstance, arrayCount, currentElement);
                        collectionMapping.SetValue(instance, arrayInstance);
                    }
                    else
                    {
                        var listType = typeof(List<>);
                        var constructedListType = listType.MakeGenericType(collectionMapping.CollectionType);
                        var listInstance = (IList)Activator.CreateInstance(constructedListType);
                        DeserializeCollection(collectionMapping, listInstance, arrayCount, currentElement);
                        collectionMapping.SetValue(instance, listInstance);
                    }
                }
            }
        }
    }
}