using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Persistity.Extensions;
using Persistity.Mappings.Types;

namespace Persistity.Mappings.Mappers
{
    public abstract class TypeMapper : ITypeMapper
    {
        public MappingConfiguration Configuration { get; private set; }
        public ITypeAnalyzer TypeAnalyzer { get; private set; }

        protected TypeMapper(ITypeAnalyzer typeAnalyzer, MappingConfiguration configuration = null)
        {
            TypeAnalyzer = typeAnalyzer;
            Configuration = configuration ?? MappingConfiguration.Default;
        }

        public virtual TypeMapping GetTypeMappingsFor(Type type)
        {
            var typeMapping = new TypeMapping
            {
                Name = type.GetPersistableName(),
                Type = type
            };

            var mappings = GetMappingsFromType(type, type.Name);
            typeMapping.InternalMappings.AddRange(mappings);

            return typeMapping;
        }

        public virtual List<Mapping> GetMappingsFromType(Type type, string scope)
        {
            var properties = GetPropertiesFor(type);

            if (TypeAnalyzer.HasIgnoredTypes())
            { properties = properties.Where(x => TypeAnalyzer.IsIgnoredType(x.PropertyType)); }

            return properties.Select(propertyInfo => GetMappingFor(propertyInfo, scope)).ToList();
        }

        public virtual IEnumerable<PropertyInfo> GetPropertiesFor(Type type)
        {
            return type.GetProperties()
                .Where(x => x.CanRead && x.CanWrite);
        }

        public virtual Mapping GetMappingFor(PropertyInfo propertyInfo, string scope)
        {
            var currentScope = scope + "." + propertyInfo.Name;

            if (TypeAnalyzer.IsPrimitiveType(propertyInfo.PropertyType))
            { return CreatePropertyMappingFor(propertyInfo, currentScope); }

            if (propertyInfo.PropertyType.IsArray || TypeAnalyzer.IsGenericList(propertyInfo.PropertyType))
            { return CreateCollectionMappingFor(propertyInfo, currentScope); }

            if (TypeAnalyzer.IsGenericDictionary(propertyInfo.PropertyType))
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
                IsArray = isArray,
                IsElementDynamicType = TypeAnalyzer.IsDynamicType(collectionType)
            };

            var collectionMappingTypes = GetMappingsFromType(collectionType, scope);
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
                IsKeyDynamicType = TypeAnalyzer.IsDynamicType(keyType),
                IsValueDynamicType = TypeAnalyzer.IsDynamicType(valueType)
            };

            var keyMappingTypes = GetMappingsFromType(keyType, scope);
            dictionaryMapping.KeyMappings.AddRange(keyMappingTypes);

            var valueMappingTypes = GetMappingsFromType(valueType, scope);
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
                SetValue = (x, v) => propertyInfo.SetValue(x, v, null),
                IsDynamicType = TypeAnalyzer.IsDynamicType(propertyInfo)
            };

            var mappingTypes = GetMappingsFromType(propertyInfo.PropertyType, scope);
            nestedMapping.InternalMappings.AddRange(mappingTypes);
            return nestedMapping;
        }

        public virtual T GetKey<T, K>(IDictionary<T, K> dictionary, int index)
        { return dictionary.Keys.ElementAt(index); }

        public virtual K GetValue<T, K>(IDictionary<T, K> dictionary, T key)
        { return dictionary[key]; }
    }
}