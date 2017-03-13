using System;
using System.Collections.Generic;
using EcsRx.Json;
using UnityEngine;

namespace Tests.Editor.Helpers.Mapping
{
    public class JsonDeserializer
    {
        private object DeserializePrimitive(JSONNode value, Type type)
        {
            if (type == typeof(int)) return value.AsInt;
            if (type == typeof(Vector2)) return value.AsVector2;
            if (type == typeof(Vector3)) return value.AsVector3;
            if (type == typeof(bool)) return value.AsBool;
            if (type == typeof(float)) return value.AsFloat;
            if (type == typeof(double)) return value.AsDouble;

            return value.Value;
        }

        public T DeserializeData<T>(TypePropertyMappings typePropertyMappings, string data) where T : new()
        {
            var instance = new T();
            var jsonData = JSON.Parse(data);
            Deserialize(typePropertyMappings.Mappings, jsonData, instance);
            return instance;
        }

        private void DeserializeProperty<T>(PropertyMapping propertyMapping, JSONNode data, T instance)
        {
            var underlyingValue = DeserializePrimitive(data, propertyMapping.Type);
            propertyMapping.SetValue(instance, underlyingValue);
        }

        private void DeserializeNestedObject<T>(NestedMapping nestedMapping, JSONNode data, T instance)
        { Deserialize(nestedMapping.InternalMappings, data, instance); }

        private void DeserializeCollection<T>(CollectionPropertyMapping collectionMapping, JSONNode data, T instance)
        {
            var jsonArray = data.AsArray;
            for(var i=0;i<data.AsArray.Count;i++)
            {
                var elementInstance = Activator.CreateInstance(collectionMapping.ArrayType);
                Deserialize(collectionMapping.InternalMappings, jsonArray, elementInstance);
            }
        }

        private void Deserialize<T>(IEnumerable<Mapping> mappings, JSONNode jsonNode, T instance)
        {
            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                {
                    var jsonData = jsonNode[mapping.LocalName];
                    DeserializeProperty((mapping as PropertyMapping), jsonData, instance);                   
                }
                else if (mapping is NestedMapping)
                {
                    var nestedMapping = (mapping as NestedMapping);
                    var jsonData = jsonNode[mapping.LocalName];
                    var childInstance = Activator.CreateInstance(nestedMapping.Type);
                    DeserializeNestedObject(nestedMapping, jsonData, childInstance);
                    nestedMapping.SetValue(instance, childInstance);

                    // Add to instance
                }/*
                else
                {
                    var collectionMapping = (mapping as CollectionPropertyMapping);
                    var jsonData = jsonNode[mapping.LocalName];
                    var arrayCount = jsonData.AsArray.Count;
                    var arrayInstance = Activator.CreateInstance(collectionMapping.Type, arrayCount);
                    DeserializeCollection((mapping as CollectionPropertyMapping), jsonData, arrayInstance);
                    // Add to instance
                }*/
            }
        }
    }
}