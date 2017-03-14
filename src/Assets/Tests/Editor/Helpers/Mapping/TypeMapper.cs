using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EcsRx.Persistence.Attributes;
using EcsRx.Persistence.Extensions;

namespace Tests.Editor.Helpers.Mapping
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

        public CollectionPropertyMapping CreateCollectionMappingFor(PropertyInfo propertyInfo, string scope)
        {
            var propertyType = propertyInfo.PropertyType;
            var isArray = propertyType.IsArray;
            var collectionType = isArray ? propertyType.GetElementType() : propertyType.GetGenericArguments()[0];

            var collectionMapping = new CollectionPropertyMapping
            {
                LocalName = propertyInfo.Name,
                ScopedName = scope,
                CollectionType = collectionType,
                Type = propertyInfo.PropertyType,
                GetValue = (x) => propertyInfo.GetValue(x, null) as IList,
                SetValue = (x, v) => propertyInfo.SetValue(x, v, null),
                IsArray = isArray
            };

            var arrayMappingTypes = GetPropertyMappingsFor(collectionType, scope);
            collectionMapping.InternalMappings.AddRange(arrayMappingTypes);

            return collectionMapping;
        }

        public PropertyMapping CreatePropertyMappingFor(PropertyInfo propertyInfo, string scope)
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

                var nestedMapping = CreateNestedMappingFor(propertyInfo, currentScope);
                propertyMappings.Add(nestedMapping);
            }

            return propertyMappings;
        }

        private bool isPrimitiveType(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(Guid);
        }
    }
}