using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Persistity.Attributes;
using Persistity.Extensions;
using UnityEngine;

namespace Persistity.Mappings
{
    public class TypeMapper
    {
        public TypeMapping GetTypeMappingsFor(Type type)
        {
            var typeMapping = new TypeMapping
            {
                Name = type.FullName,
                Type = type
            };

            var mappings = GetPropertyMappingsFor(type, type.Name);
            typeMapping.InternalMappings.AddRange(mappings);

            return typeMapping;
        }

        public bool IsGenericList(Type type)
        {
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IList<>)));
        }

        public bool IsGenericDictionary(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>);
        }

        public CollectionMapping CreateCollectionMappingFor(PropertyInfo propertyInfo, string scope)
        {
            var propertyType = propertyInfo.PropertyType;
            var isArray = propertyType.IsArray;
            var collectionType = isArray ? propertyType.GetElementType() : propertyType.GetGenericArguments()[0];

            var collectionMapping = new CollectionMapping
            {
                LocalName = propertyInfo.Name,
                ScopedName = scope,
                CollectionType = collectionType,
                Type = propertyInfo.PropertyType,
                GetValue = (x) => propertyInfo.GetValue(x, null) as IList,
                SetValue = (x, v) => propertyInfo.SetValue(x, v, null),
                IsArray = isArray
            };

            var collectionMappingTypes = GetPropertyMappingsFor(collectionType, scope);
            collectionMapping.InternalMappings.AddRange(collectionMappingTypes);

            return collectionMapping;
        }

        public DictionaryMapping CreateDictionaryMappingFor(PropertyInfo propertyInfo, string scope)
        {
            var propertyType = propertyInfo.PropertyType;
            var dictionaryTypes = propertyType.GetGenericArguments();

            var keyType = dictionaryTypes[0];
            var valueType = dictionaryTypes[1];

            var dictionaryMapping = new DictionaryMapping
            {
                LocalName = propertyInfo.Name,
                ScopedName = scope,
                KeyType = keyType,
                ValueType = valueType,
                Type = propertyInfo.PropertyType,
                GetValue = (x) => propertyInfo.GetValue(x, null) as IDictionary,
                SetValue = (x, v) => propertyInfo.SetValue(x, v, null),
            };

            var keyMappingTypes = GetPropertyMappingsFor(keyType, scope);
            dictionaryMapping.KeyMappings.AddRange(keyMappingTypes);

            var valueMappingTypes = GetPropertyMappingsFor(valueType, scope);
            dictionaryMapping.ValueMappings.AddRange(valueMappingTypes);

            return dictionaryMapping;
        }

        public PropertyMapping CreatePropertyMappingFor(PropertyInfo propertyInfo, string scope)
        {
            return new PropertyMapping
            {
                LocalName = propertyInfo.Name,
                ScopedName = scope,
                Type = propertyInfo.PropertyType,
                GetValue = x => { return propertyInfo.GetValue(x, null); },
                SetValue = (x, v) => propertyInfo.SetValue(x, v, null)
            };
        }

        public NestedMapping CreateNestedMappingFor(PropertyInfo propertyInfo, string scope)
        {
            var nestedMapping = new NestedMapping
            {
                LocalName = propertyInfo.Name,
                ScopedName = scope,
                Type = propertyInfo.PropertyType,
                GetValue = x => propertyInfo.GetValue(x, null),
                SetValue = (x, v) => propertyInfo.SetValue(x, v, null)
            };

            var mappingTypes = GetPropertyMappingsFor(propertyInfo.PropertyType, scope);
            nestedMapping.InternalMappings.AddRange(mappingTypes);
            return nestedMapping;
        }

        public T GetKey<T, K>(IDictionary<T, K> dictionary, int index)
        { return dictionary.Keys.ElementAt(index); }

        public K GetValue<T, K>(IDictionary<T, K> dictionary, T key)
        { return dictionary[key]; }

        public List<Mapping> GetPropertyMappingsFor(Type type, string scope)
        {
            var propertyMappings = new List<Mapping>();
            var properties = type.GetProperties().Where(x => x.HasAttribute<PersistDataAttribute>());

            foreach (var propertyInfo in properties)
            {
                var currentScope = scope + "." + propertyInfo.Name;

                if (isPrimitiveType(propertyInfo.PropertyType))
                {
                    var propertyDescriptor = CreatePropertyMappingFor(propertyInfo, currentScope);
                    propertyMappings.Add(propertyDescriptor);
                    continue;
                }

                if (propertyInfo.PropertyType.IsArray || IsGenericList(propertyInfo.PropertyType))
                {
                    var collectionMapping = CreateCollectionMappingFor(propertyInfo, currentScope);
                    propertyMappings.Add(collectionMapping);
                    continue;
                }

                if (IsGenericDictionary(propertyInfo.PropertyType))
                {
                    var dictionaryMapping = CreateDictionaryMappingFor(propertyInfo, currentScope);
                    propertyMappings.Add(dictionaryMapping);
                    continue;
                }

                var nestedMapping = CreateNestedMappingFor(propertyInfo, currentScope);
                propertyMappings.Add(nestedMapping);
            }

            return propertyMappings;
        }

        private bool isPrimitiveType(Type type)
        {
            return type.IsPrimitive || 
                type == typeof(string) ||
                type == typeof(DateTime) ||
                type == typeof(Vector2) ||
                type == typeof(Vector3) ||
                type == typeof(Vector4) ||
                type == typeof(Quaternion) ||
                type == typeof(Guid);
        }
    }
}