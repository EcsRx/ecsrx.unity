using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Persistity.Extensions;
using UnityEngine;

namespace Persistity.Mappings.Mappers
{
    public abstract class TypeMapper : ITypeMapper
    {
        private readonly IEnumerable<Type> _knownPrimitives;

        protected TypeMapper(IEnumerable<Type> knownPrimitives = null)
        {
            _knownPrimitives = knownPrimitives ?? new List<Type>();
        }

        public bool IsGenericList(Type type)
        { return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>); }

        public bool IsGenericDictionary(Type type)
        { return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>); }

        public virtual bool IsPrimitiveType(Type type)
        {
            var isDefaultPrimitive = type.IsPrimitive ||
                   type == typeof(string) ||
                   type == typeof(DateTime) ||
                   type == typeof(Vector2) ||
                   type == typeof(Vector3) ||
                   type == typeof(Vector4) ||
                   type == typeof(Quaternion) ||
                   type == typeof(Guid);

            return isDefaultPrimitive || _knownPrimitives.Any(x => type == x);
        }

        public virtual TypeMapping GetTypeMappingsFor(Type type)
        {
            var typeMapping = new TypeMapping
            {
                Name = type.FullName,
                Type = type
            };

            var mappings = GetMappingsFor(type, type.Name);
            typeMapping.InternalMappings.AddRange(mappings);

            return typeMapping;
        }

        public virtual List<Mapping> GetMappingsFor(Type type, string scope)
        {
            var properties = GetPropertiesFor(type);
            return properties.Select(propertyInfo => GetMappingFor(propertyInfo, scope)).ToList();
        }

        public virtual IEnumerable<PropertyInfo> GetPropertiesFor(Type type)
        { return type.GetProperties().Where(x => x.CanRead && x.CanWrite); }

        public virtual Mapping GetMappingFor(PropertyInfo propertyInfo, string scope)
        {
            var currentScope = scope + "." + propertyInfo.Name;

            if (IsPrimitiveType(propertyInfo.PropertyType))
            { return CreatePropertyMappingFor(propertyInfo, currentScope); }

            if (propertyInfo.PropertyType.IsArray || IsGenericList(propertyInfo.PropertyType))
            { return CreateCollectionMappingFor(propertyInfo, currentScope); }

            if (IsGenericDictionary(propertyInfo.PropertyType))
            { return CreateDictionaryMappingFor(propertyInfo, currentScope); }

            return CreateNestedMappingFor(propertyInfo, currentScope);
        }

        public virtual CollectionMapping CreateCollectionMappingFor(PropertyInfo propertyInfo, string scope)
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

            var collectionMappingTypes = GetMappingsFor(collectionType, scope);
            collectionMapping.InternalMappings.AddRange(collectionMappingTypes);

            return collectionMapping;
        }

        public virtual DictionaryMapping CreateDictionaryMappingFor(PropertyInfo propertyInfo, string scope)
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

            var keyMappingTypes = GetMappingsFor(keyType, scope);
            dictionaryMapping.KeyMappings.AddRange(keyMappingTypes);

            var valueMappingTypes = GetMappingsFor(valueType, scope);
            dictionaryMapping.ValueMappings.AddRange(valueMappingTypes);

            return dictionaryMapping;
        }

        public virtual PropertyMapping CreatePropertyMappingFor(PropertyInfo propertyInfo, string scope)
        {
            return new PropertyMapping
            {
                LocalName = propertyInfo.Name,
                ScopedName = scope,
                Type = propertyInfo.PropertyType,
                GetValue = x => propertyInfo.GetValue(x, null),
                SetValue = (x, v) => propertyInfo.SetValue(x, v, null)
            };
        }

        public virtual NestedMapping CreateNestedMappingFor(PropertyInfo propertyInfo, string scope)
        {
            var nestedMapping = new NestedMapping
            {
                LocalName = propertyInfo.Name,
                ScopedName = scope,
                Type = propertyInfo.PropertyType,
                GetValue = x => propertyInfo.GetValue(x, null),
                SetValue = (x, v) => propertyInfo.SetValue(x, v, null)
            };

            var mappingTypes = GetMappingsFor(propertyInfo.PropertyType, scope);
            nestedMapping.InternalMappings.AddRange(mappingTypes);
            return nestedMapping;
        }

        public virtual T GetKey<T, K>(IDictionary<T, K> dictionary, int index)
        { return dictionary.Keys.ElementAt(index); }

        public virtual K GetValue<T, K>(IDictionary<T, K> dictionary, T key)
        { return dictionary[key]; }
    }
}