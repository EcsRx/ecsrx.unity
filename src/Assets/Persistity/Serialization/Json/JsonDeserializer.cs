using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persistity.Json;
using Persistity.Mappings;
using UnityEngine;

namespace Persistity.Serialization.Json
{
    public class JsonDeserializer : IJsonDeserializer
    {
        public JsonConfiguration Configuration { get; private set; }

        public JsonDeserializer(JsonConfiguration configuration = null)
        { Configuration = configuration ?? JsonConfiguration.Default; }

        private bool IsNullNode(JSONNode node)
        { return node == null; }

        private object DeserializePrimitive(Type type, JSONNode value)
        {
            if (type == typeof(byte)) { return (byte)value.AsInt; }
            if (type == typeof(short)) { return (short)value.AsInt; }
            if (type == typeof(int)) { return value.AsInt; }
            if (type == typeof(long)) { return long.Parse(value.Value); }
            if (type == typeof(Guid)) { return new Guid(value.Value); }
            if (type == typeof(bool)) { return value.AsBool; }
            if (type == typeof(float)) { return value.AsFloat; }
            if (type == typeof(double)) { return value.AsDouble; }
            if (type.IsEnum) { return Enum.Parse(type, value.Value); }
            if (type == typeof(DateTime)) { return DateTime.FromBinary(long.Parse(value.Value)); }
            if (type.IsEnum) { return Enum.Parse(type, value.Value); }
            if (type == typeof(Vector2))
            { return new Vector2(value["x"].AsFloat, value["y"].AsFloat); }
            if (type == typeof(Vector3))
            { return new Vector3(value["x"].AsFloat, value["y"].AsFloat, value["z"].AsFloat); }
            if (type == typeof(Vector4))
            { return new Vector4(value["x"].AsFloat, value["y"].AsFloat, value["z"].AsFloat, value["w"].AsFloat); }
            if (type == typeof(Quaternion))
            { return new Quaternion(value["x"].AsFloat, value["y"].AsFloat, value["z"].AsFloat, value["w"].AsFloat); }

            var matchingHandler = Configuration.TypeHandlers.SingleOrDefault(x => x.MatchesType(type));
            if (matchingHandler != null)
            { return matchingHandler.HandleTypeOut(value); }

            return value.Value;
        }

        public T DeserializeData<T>(TypeMapping typeMapping, byte[] data) where T : new()
        {
            var jsonString = Configuration.Encoder.GetString(data);
            var instance = new T();
            var jsonData = JSON.Parse(jsonString);
            Deserialize(typeMapping.InternalMappings, instance, jsonData);
            return instance;
        }

        public object DeserializeData(TypeMapping typeMapping, byte[] data)
        {
            var jsonString = Configuration.Encoder.GetString(data);
            var instance = Activator.CreateInstance(typeMapping.Type);
            var jsonData = JSON.Parse(jsonString);
            Deserialize(typeMapping.InternalMappings, instance, jsonData);
            return instance;
        }

        private void DeserializeProperty<T>(PropertyMapping propertyMapping, T instance, JSONNode data)
        {
            if (IsNullNode(data))
            {
                propertyMapping.SetValue(instance, null);
                return;
            }

            var underlyingValue = DeserializePrimitive(propertyMapping.Type, data);
            propertyMapping.SetValue(instance, underlyingValue);
        }

        private void DeserializeNestedObject<T>(NestedMapping nestedMapping, T instance, JSONNode data)
        { Deserialize(nestedMapping.InternalMappings, instance, data); }

        private void DeserializeCollection(CollectionMapping collectionMapping, IList instance, JSONArray data)
        {
            for(var i=0;i<data.Count;i++)
            {
                var currentElementNode = data[i];
                if (IsNullNode(currentElementNode))
                {
                    if (instance.IsFixedSize)
                    { instance[i] = null; }
                    else
                    { instance.Insert(i, null); }
                }
                else if (collectionMapping.InternalMappings.Count > 0)
                {
                    var elementInstance = Activator.CreateInstance(collectionMapping.CollectionType);
                    Deserialize(collectionMapping.InternalMappings, elementInstance, currentElementNode);

                    if (instance.IsFixedSize)
                    { instance[i] = elementInstance; }
                    else
                    { instance.Insert(i, elementInstance); }
                }
                else
                {
                    var value = DeserializePrimitive(collectionMapping.CollectionType, currentElementNode);
                    if (instance.IsFixedSize)
                    { instance[i] = value; }
                    else
                    { instance.Insert(i, value); }
                }
            }
        }

        private void DeserializeDictionary(DictionaryMapping dictionaryMapping, IDictionary instance, JSONArray data)
        {
            for (var i = 0; i < data.Count; i++)
            {
                var currentElement = data[i];
                var jsonKey = currentElement["key"];
                var jsonValue = currentElement["value"];
                
                object currentKey, currentValue;

                if (dictionaryMapping.KeyMappings.Count > 0)
                {
                    currentKey = Activator.CreateInstance(dictionaryMapping.KeyType);
                    Deserialize(dictionaryMapping.KeyMappings, currentKey, jsonKey);
                }
                else
                { currentKey = DeserializePrimitive(dictionaryMapping.KeyType, jsonKey); }

                if (IsNullNode(jsonValue))
                { currentValue = null; }
                else if (dictionaryMapping.ValueMappings.Count > 0)
                {
                    currentValue = Activator.CreateInstance(dictionaryMapping.ValueType);
                    Deserialize(dictionaryMapping.ValueMappings, currentValue, jsonValue);
                }
                else
                { currentValue = DeserializePrimitive(dictionaryMapping.ValueType, jsonValue); }

                instance.Add(currentKey, currentValue);
            }
        }

        private void Deserialize<T>(IEnumerable<Mapping> mappings, T instance, JSONNode jsonNode)
        {
            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                {
                    var jsonData = jsonNode[mapping.LocalName];
                    DeserializeProperty((mapping as PropertyMapping), instance, jsonData);                   
                }
                else if (mapping is NestedMapping)
                {
                    var nestedMapping = (mapping as NestedMapping);
                    var jsonData = jsonNode[mapping.LocalName];

                    if (IsNullNode(jsonData))
                    { nestedMapping.SetValue(instance, null); }
                    else
                    {
                        var childInstance = Activator.CreateInstance(nestedMapping.Type);
                        DeserializeNestedObject(nestedMapping, childInstance, jsonData);
                        nestedMapping.SetValue(instance, childInstance);
                    }
                }
                else if (mapping is DictionaryMapping)
                {
                    var dictionaryMapping = (mapping as DictionaryMapping);
                    var jsonData = jsonNode[mapping.LocalName].AsArray;
                    if (IsNullNode(jsonData))
                    { dictionaryMapping.SetValue(instance, null); }
                    else
                    { 
                        var dictionarytype = typeof(Dictionary<,>);
                        var constructedDictionaryType = dictionarytype.MakeGenericType(dictionaryMapping.KeyType, dictionaryMapping.ValueType);
                        var dictionary = (IDictionary)Activator.CreateInstance(constructedDictionaryType);
                        DeserializeDictionary(dictionaryMapping, dictionary, jsonData);
                        dictionaryMapping.SetValue(instance, dictionary);
                    }
                }
                else
                {
                    var collectionMapping = (mapping as CollectionMapping);
                    var jsonData = jsonNode[mapping.LocalName].AsArray;

                    if (IsNullNode(jsonData))
                    {
                        collectionMapping.SetValue(instance, null);
                        continue;
                    }

                    var arrayCount = jsonData.Count;

                    if (collectionMapping.IsArray)
                    {
                        var arrayInstance = (IList) Activator.CreateInstance(collectionMapping.Type, arrayCount);
                        DeserializeCollection(collectionMapping, arrayInstance, jsonData);
                        collectionMapping.SetValue(instance, arrayInstance);
                    }
                    else
                    {
                        var listType = typeof(List<>);
                        var constructedListType = listType.MakeGenericType(collectionMapping.CollectionType);
                        var listInstance = (IList)Activator.CreateInstance(constructedListType);
                        DeserializeCollection(collectionMapping, listInstance, jsonData);
                        collectionMapping.SetValue(instance, listInstance);
                    }
                }
            }
        }
    }
}