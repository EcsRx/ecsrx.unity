using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Persistence.Attributes;
using EcsRx.Persistence.Extensions;

namespace Tests.Editor.Helpers.Mapping
{
    public class TypeMapper
    {
        public TypePropertyMappings GetTypeMappingsFor(Type type)
        {
            var typeMappings = new TypePropertyMappings();
            typeMappings.Name = type.FullName;

            var mappings = GetPropertyMappingsFor(type, type.Name);
            typeMappings.Mappings.AddRange(mappings);

            return typeMappings;
        }

        private bool IsGenericList(Type type)
        {
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IList<>)));
        }

        public List<Mapping> GetPropertyMappingsFor(Type type, string scope)
        {
            var propertyMappings = new List<Mapping>();
            var properties = type.GetProperties().Where(x => x.HasAttribute<PersistDataAttribute>());

            foreach (var propertyInfo in properties)
            {
                var newScope = scope + "." + propertyInfo.Name;
                if (isPrimitiveType(propertyInfo.PropertyType))
                {
                    var propertyDescriptor = new PropertyMapping
                    {
                        LocalName = propertyInfo.Name,
                        ScopedName = newScope,
                        Type = propertyInfo.PropertyType,
                        GetValue = x => propertyInfo.GetValue(x, null),
                        SetValue = (x, v) => propertyInfo.SetValue(x, v, null)
                    };

                    propertyMappings.Add(propertyDescriptor);
                    continue;
                }

                if (propertyInfo.PropertyType.IsArray)
                {
                    var arrayType = propertyInfo.PropertyType.GetElementType();

                    var collectionMapping = new CollectionPropertyMapping
                    {
                        LocalName = propertyInfo.Name,
                        ScopedName = newScope,
                        CollectionType = arrayType,
                        Type = propertyInfo.PropertyType,
                        GetValue = (x) => propertyInfo.GetValue(x, null) as Array,
                        SetValue = (x, v) => propertyInfo.SetValue(x, v, null),
                        IsArray = true
                    };
                    propertyMappings.Add(collectionMapping);

                    var arrayMappingTypes = GetPropertyMappingsFor(arrayType, newScope);
                    collectionMapping.InternalMappings.AddRange(arrayMappingTypes);
                    continue;
                }

                if (IsGenericList(propertyInfo.PropertyType))
                {
                    var listType = propertyInfo.PropertyType.GetGenericArguments()[0];

                    var collectionMapping = new CollectionPropertyMapping
                    {
                        LocalName = propertyInfo.Name,
                        ScopedName = newScope,
                        CollectionType = listType,
                        Type = propertyInfo.PropertyType,
                        GetValue = (x) => propertyInfo.GetValue(x, null) as IList,
                        SetValue = (x, v) => propertyInfo.SetValue(x, v, null)
                    };
                    propertyMappings.Add(collectionMapping);

                    var collectionMappingTypes = GetPropertyMappingsFor(listType, newScope);
                    collectionMapping.InternalMappings.AddRange(collectionMappingTypes);
                    continue;
                }

                var nestedMapping = new NestedMapping
                {
                    LocalName = propertyInfo.Name,
                    ScopedName = newScope,
                    Type = propertyInfo.PropertyType,
                    GetValue = x => propertyInfo.GetValue(x, null),
                    SetValue = (x,v) => propertyInfo.SetValue(x,v, null)
                };
                propertyMappings.Add(nestedMapping);

                var mappingTypes = GetPropertyMappingsFor(propertyInfo.PropertyType, newScope);
                nestedMapping.InternalMappings.AddRange(mappingTypes);
            }

            return propertyMappings;
        }

        private bool isPrimitiveType(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime);
        }
    }
}