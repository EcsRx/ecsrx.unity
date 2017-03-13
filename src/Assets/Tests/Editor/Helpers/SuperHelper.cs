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
        public Func<object, int> GetCount { get; set; }
        public Func<object, Array> GetValue { get; set; }
        public Func<object, int, object> GetIndex { get; set; }
        public Action<object, int, object> SetIndex { get; set; }
        public List<Mapping> InternalMappings { get; private set; }

        public CollectionPropertyMapping()
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

        public List<Mapping> GetPropertyMappingsFor(Type type, string scope, Func<object, object> parentGetter = null)
        {
            var propertyMappings = new List<Mapping>();
            var properties = type.GetProperties().Where(x => x.HasAttribute<PersistDataAttribute>());

            var getCorrectScope = new Func<object, object>((x) =>
            {
                if (parentGetter == null) { return x; }
                return parentGetter(x);
            });

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
                        GetValue = x => propertyInfo.GetValue(getCorrectScope(x), null),
                        SetValue = (x, v) => propertyInfo.SetValue(getCorrectScope(x), v, null)
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
                        GetCount = (x) => (propertyInfo.GetValue(getCorrectScope(x), null) as Array).Length,
                        GetValue = (x) => propertyInfo.GetValue(getCorrectScope(x), null) as Array,
                        GetIndex = (x, i) => (propertyInfo.GetValue(getCorrectScope(x), null) as Array).GetValue(i),
                        SetIndex = (x, i, v) => (propertyInfo.GetValue(getCorrectScope(x), null) as Array).SetValue(v, i)
                    };
                    propertyMappings.Add(collectionMapping);

                    var arrayMappingTypes = GetPropertyMappingsFor(arrayType, newScope, (x) => propertyInfo.GetValue(getCorrectScope(x), null));
                    collectionMapping.InternalMappings.AddRange(arrayMappingTypes);
                    continue;
                }

                var mappingTypes = GetPropertyMappingsFor(propertyInfo.PropertyType, newScope, (x) => propertyInfo.GetValue(getCorrectScope(x), null));
                propertyMappings.AddRange(mappingTypes);
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
            foreach (var mapping in typePropertyMappings.Mappings)
            {
                if (mapping is PropertyMapping)
                {
                    var propertyOutput = SerializeProperty((mapping as PropertyMapping), data);
                    output.AppendLine(propertyOutput);
                }
                else
                {
                    var collectionOutput = SerializeCollection((mapping as CollectionPropertyMapping), data);
                    output.AppendLine(collectionOutput);
                }
            }
            return output.ToString();
        }

        private static string SerializeProperty<T>(this PropertyMapping propertyMapping, T data)
        {
            var output = propertyMapping.GetValue(data);
            return string.Format("{0} : {1}, \n", propertyMapping.ScopedName, output);
        }

        private static string SerializeCollection<T>(this CollectionPropertyMapping collectionMapping, T data)
        {
            var output = new StringBuilder();
            var arrayValue = collectionMapping.GetValue(data);
            foreach (var mapping in collectionMapping.InternalMappings)
            {
                for (var i = 0; i < arrayValue.Length; i++)
                {
                    var currentData = arrayValue.GetValue(i);

                    if (mapping is PropertyMapping)
                    {
                        var propertyOutput = SerializeProperty((mapping as PropertyMapping), currentData);
                        output.AppendLine(propertyOutput);
                    }
                    else
                    {
                        var collectionOutput = SerializeCollection((mapping as CollectionPropertyMapping), currentData);
                        output.AppendLine(collectionOutput);
                    }
                }
            }

            return output.ToString();
        }
    }
}