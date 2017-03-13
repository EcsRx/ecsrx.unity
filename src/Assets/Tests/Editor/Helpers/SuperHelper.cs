using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EcsRx.Components;
using EcsRx.Persistence.Attributes;
using EcsRx.Persistence.Extensions;
using EcsRx.Persistence.Types;
using UnityEditor;

namespace Assets.Tests.Editor.Helpers
{
    public class Mapping
    {
        public string LocalName { get; set; }
        public string ScopedName { get; set; }
        public Type Type { get; set; }
    }

    public class PropertyMapping : Mapping
    {
        public Func<object, object> GetValue { get; set; }
        public Action<object, object> SetValue { get; set; }
    }

    public class CollectionPropertyMapping : Mapping
    {
        public Func<object, Array> GetValue { get; set; }
        public List<Mapping> InternalMappings { get; private set; }

        public CollectionPropertyMapping()
        { InternalMappings = new List<Mapping>(); }
    }

    public class NestedMapping : Mapping
    {
        public Func<object, object> GetValue { get; set; }
        public List<Mapping> InternalMappings { get; private set; }

        public NestedMapping()
        { InternalMappings = new List<Mapping>(); }
    }

    public class TypePropertyMappings
    {
        public string Name { get; set; }
        public List<Mapping> Mappings { get; private set; }

        public TypePropertyMappings()
        {
            Mappings = new List<Mapping>();
        }
    }

    public class SuperHelper
    {
        public TypePropertyMappings GetTypeMappingsFor(Type type)
        {
            var typeMappings = new TypePropertyMappings();
            typeMappings.Name = type.FullName;

            var mappings = GetPropertyMappingsFor(type, type.Name);
            typeMappings.Mappings.AddRange(mappings);

            return typeMappings;
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
                        ScopedName = newScope + "[]",
                        Type = arrayType.GetElementType(),
                        GetValue = (x) => propertyInfo.GetValue(x, null) as Array
                    };
                    propertyMappings.Add(collectionMapping);

                    var arrayMappingTypes = GetPropertyMappingsFor(arrayType, newScope);
                    collectionMapping.InternalMappings.AddRange(arrayMappingTypes);
                    continue;
                }

                var nestedMapping = new NestedMapping
                {
                    LocalName = propertyInfo.Name,
                    ScopedName = newScope,
                    Type = propertyInfo.PropertyType,
                    GetValue = x => propertyInfo.GetValue(x, null)
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

    public static class Ext
    {
        public static string SerializeData<T>(this TypePropertyMappings typePropertyMappings, T data)
        {
            var output = new StringBuilder();

            var result = Blah(typePropertyMappings.Mappings, data);
            output.AppendLine(result);
            return output.ToString();
        }

        private static string SerializeProperty<T>(this PropertyMapping propertyMapping, T data)
        {
            var output = propertyMapping.GetValue(data);
            return string.Format("{0} : {1}, \n", propertyMapping.ScopedName, output);
        }

        private static string SerializeNestedObject<T>(this NestedMapping nestedMapping, T data)
        {
            var output = new StringBuilder();
            var currentData = nestedMapping.GetValue(data);
            var result = Blah(nestedMapping.InternalMappings, currentData);
            output.AppendLine(result);
            return output.ToString();
        }

        private static string Blah<T>(IEnumerable<Mapping> mappings, T data)
        {
            var output = new StringBuilder();

            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                {
                    var result = SerializeProperty((mapping as PropertyMapping), data);
                    output.AppendLine(result);
                }
                else if (mapping is NestedMapping)
                {
                    var result = SerializeNestedObject((mapping as NestedMapping), data);
                    output.AppendLine(result);
                }
                else
                {
                    var result = SerializeCollection((mapping as CollectionPropertyMapping), data);
                    output.AppendLine(result);
                }
            }

            return output.ToString();
        }

        private static string SerializeCollection<T>(this CollectionPropertyMapping collectionMapping, T data)
        {
            var output = new StringBuilder();
            var arrayValue = collectionMapping.GetValue(data);
            output.AppendFormat("{0} : {1}, \n", collectionMapping.ScopedName, arrayValue.Length);

            for (var i = 0; i < arrayValue.Length; i++)
            {
                var currentData = arrayValue.GetValue(i);
                var result = Blah(collectionMapping.InternalMappings, currentData);
                output.AppendLine(result);
            }

            return output.ToString();
        }
    }
}