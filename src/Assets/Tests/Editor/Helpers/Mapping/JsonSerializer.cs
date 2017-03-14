using System;
using System.Collections.Generic;
using EcsRx.Json;
using UnityEngine;

namespace Tests.Editor.Helpers.Mapping
{
    public class JsonSerializer
    {
        private JSONNode SerializePrimitive(object value, Type type)
        {
            JSONNode node = null;
            if (type == typeof(byte)) node = new JSONData((byte)value);
            if (type == typeof(short)) node = new JSONData((short)value);
            if (type == typeof(int)) node = new JSONData((int)value);
            if (type == typeof(long)) node = new JSONData((long)value);
            else if (type == typeof(Vector2)) node = new JSONClass() { AsVector2 = (Vector2)value };
            else if (type == typeof(Vector3)) node = new JSONClass() { AsVector3 = (Vector3)value };
            else if (type == typeof(Vector4)) node = new JSONClass() { AsVector4 = (Vector4)value };
            else if (type == typeof(Quaternion)) node = new JSONClass() { AsQuaternion = (Quaternion)value };
            else if (type == typeof(bool)) node = new JSONData((bool)value);
            else if (type == typeof(float)) node = new JSONData((float)value);
            else if (type == typeof(double)) node = new JSONData((double)value);
            else node = new JSONData(value.ToString());
            return node;
        }

        public JSONNode SerializeData<T>(TypeMapping typeMapping, T data)
        { return Serialize(typeMapping.InternalMappings, data); }

        private JSONNode SerializeProperty<T>(PropertyMapping propertyMapping, T data)
        {
            var underlyingValue = propertyMapping.GetValue(data);
            return SerializePrimitive(underlyingValue, propertyMapping.Type);
        }

        private JSONNode SerializeNestedObject<T>(NestedMapping nestedMapping, T data)
        {
            var currentData = nestedMapping.GetValue(data);
            return Serialize(nestedMapping.InternalMappings, currentData);
        }
        
        private JSONNode SerializeCollection<T>(CollectionPropertyMapping collectionMapping, T data)
        {
            var collectionValue = collectionMapping.GetValue(data);
            var jsonArray = new JSONArray();

            for (var i = 0; i < collectionValue.Count; i++)
            {
                var currentData = collectionValue[i];

                if (collectionMapping.InternalMappings.Count > 0)
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

        private JSONNode Serialize<T>(IEnumerable<Mapping> mappings, T data)
        {
            var jsonNode = new JSONClass();
            
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
                else
                {
                    var result = SerializeCollection((mapping as CollectionPropertyMapping), data);
                    jsonNode.Add(mapping.LocalName, result);
                }
            }

            return jsonNode;
        }
    }
}