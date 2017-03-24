using System;
using System.Collections.Generic;
using System.Linq;
using Persistity.Exceptions;
using Persistity.Json;
using Persistity.Mappings;
using UnityEngine;

namespace Persistity.Serialization.Json
{
    public class JsonSerializer : IJsonSerializer
    {
        public JsonConfiguration Configuration { get; private set; }

        public JsonSerializer(JsonConfiguration configuration = null)
        { Configuration = configuration ?? JsonConfiguration.Default; }

        private JSONNull GetNullNode()
        { return new JSONNull(); }

        private JSONNode SerializePrimitive(object value, Type type)
        {
            JSONNode node = null;

            if (type == typeof(byte)) { node = new JSONNumber((byte)value); }
            else if (type == typeof(short)) { node = new JSONNumber((short)value); }
            else if (type == typeof(int)) { node = new JSONNumber((int)value); }
            else if (type == typeof(long)) { node = new JSONString(value.ToString()); }
            else if (type == typeof(Guid)) { node = new JSONString(value.ToString()); }
            else if (type == typeof(bool)) { node = new JSONBool((bool)value); }
            else if (type == typeof(float)) { node = new JSONNumber((float)value); }
            else if (type == typeof(double)) { node = new JSONNumber((double)value); }
            else if (type == typeof(Vector2))
            {
                var typedValue = (Vector2)value;
                node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
            }
            else if (type == typeof(Vector3))
            {
                var typedValue = (Vector3)value;
                node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                node.Add("z", new JSONNumber(typedValue.z));
            }
            else if (type == typeof(Vector4))
            {
                var typedValue = (Vector4)value;
                node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                node.Add("z", new JSONNumber(typedValue.z));
                node.Add("w", new JSONNumber(typedValue.w));
            }
            else if (type == typeof(Quaternion))
            {
                var typedValue = (Quaternion) value;
                node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                node.Add("z", new JSONNumber(typedValue.z));
                node.Add("w", new JSONNumber(typedValue.w));
            }
            else if (type == typeof(DateTime))
            {
                var typedValue = (DateTime) value;
                node = new JSONString(typedValue.ToBinary().ToString());
            }
            else if (type == typeof(string) || type.IsEnum)
            { node = new JSONString(value.ToString()); }
            else
            {
                var matchingHandler = Configuration.TypeHandlers.SingleOrDefault(x => x.MatchesType(type));
                if(matchingHandler == null) { throw new NoKnownTypeException(type); }
                matchingHandler.HandleTypeIn(node, value);
            }

            return node;
        }

        public byte[] SerializeData<T>(TypeMapping typeMapping, T data) where T : new()
        { return SerializeData(typeMapping, (object) data); }

        public byte[] SerializeData(TypeMapping typeMapping, object data)
        {
            var jsonNode = Serialize(typeMapping.InternalMappings, data);
            var jsonString = jsonNode.ToString();
            return Configuration.Encoder.GetBytes(jsonString);
        }

        private JSONNode SerializeProperty<T>(PropertyMapping propertyMapping, T data)
        {
            if (data == null) { return GetNullNode(); }
            var underlyingValue = propertyMapping.GetValue(data);
            if (underlyingValue == null) { return GetNullNode(); }

            return SerializePrimitive(underlyingValue, propertyMapping.Type);
        }

        private JSONNode SerializeNestedObject<T>(NestedMapping nestedMapping, T data)
        {
            if (data == null) { return GetNullNode(); }
            var currentData = nestedMapping.GetValue(data);
            if (currentData == null) { return GetNullNode(); }

            return Serialize(nestedMapping.InternalMappings, currentData);
        }
        
        private JSONNode SerializeCollection<T>(CollectionMapping collectionMapping, T data)
        {
            if (data == null) { return GetNullNode(); }
            var collectionValue = collectionMapping.GetValue(data);
            if (collectionValue == null) { return GetNullNode(); }

            var jsonArray = new JSONArray();

            for (var i = 0; i < collectionValue.Count; i++)
            {
                var currentData = collectionValue[i];
                if (currentData == null)
                { jsonArray.Add(GetNullNode()); }
                else if (collectionMapping.InternalMappings.Count > 0)
                {
                    var result = Serialize(collectionMapping.InternalMappings, currentData);
                    jsonArray.Add(result);
                }
                else
                {
                    var result = SerializePrimitive(currentData, collectionMapping.CollectionType);
                    jsonArray.Add(result);
                }
            }

            return jsonArray;
        }

        private JSONNode SerializeDictionary<T>(DictionaryMapping dictionaryMapping, T data)
        {
            if(data == null) { return new JSONArray(); }

            var jsonArray = new JSONArray();
            var dictionaryValue = dictionaryMapping.GetValue(data);
            if (dictionaryValue == null) { return GetNullNode(); }

            foreach (var currentKey in dictionaryValue.Keys)
            {
                JSONNode jsonKey, jsonValue;
                if (dictionaryMapping.KeyMappings.Count > 0)
                { jsonKey = Serialize(dictionaryMapping.KeyMappings, currentKey); }
                else
                { jsonKey = SerializePrimitive(currentKey, dictionaryMapping.KeyType); }

                var currentValue = dictionaryValue[currentKey];
                if(currentValue == null)
                { jsonValue = GetNullNode(); }
                else if (dictionaryMapping.ValueMappings.Count > 0)
                { jsonValue = Serialize(dictionaryMapping.ValueMappings, currentValue); }
                else
                { jsonValue = SerializePrimitive(currentValue, dictionaryMapping.ValueType); }

                var jsonKeyValue = new JSONObject();
                jsonKeyValue.Add("key", jsonKey);
                jsonKeyValue.Add("value", jsonValue);
                jsonArray.Add(jsonKeyValue);
            }

            return jsonArray;
        }

        private JSONNode Serialize<T>(IEnumerable<Mapping> mappings, T data)
        {
            var jsonNode = new JSONObject();

            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                {
                    var result = SerializeProperty((mapping as PropertyMapping), data);
                    jsonNode.Add(mapping.LocalName, result);
                }
                else if (mapping is NestedMapping)
                {
                    var result = SerializeNestedObject((mapping as NestedMapping), data);
                    jsonNode.Add(mapping.LocalName, result);
                }
                else if (mapping is DictionaryMapping)
                {
                    var result = SerializeDictionary((mapping as DictionaryMapping), data);
                    jsonNode.Add(mapping.LocalName, result);
                }
                else
                {
                    var result = SerializeCollection((mapping as CollectionMapping), data);
                    jsonNode.Add(mapping.LocalName, result);
                }
            }
            return jsonNode;
        }
    }
}