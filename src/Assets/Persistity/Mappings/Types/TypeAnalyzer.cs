using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Persistity.Attributes;
using Persistity.Extensions;
using UnityEngine;

namespace Persistity.Mappings.Types
{
    public class TypeAnalyzer : ITypeAnalyzer
    {
        public TypeAnalyzerConfiguration Configuration { get; private set; }
        
        public TypeAnalyzer(TypeAnalyzerConfiguration configuration = null)
        {
            Configuration = configuration ?? TypeAnalyzerConfiguration.Default;
        }

        public bool IsGenericList(Type type)
        { return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>); }

        public bool IsGenericDictionary(Type type)
        { return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>); }

        public bool IsDynamicType(Type type)
        { return type.IsAbstract || type.IsInterface || type == typeof(object); }

        public bool IsDynamicType(PropertyInfo propertyInfo)
        {
            var typeIsDynamic = IsDynamicType(propertyInfo.PropertyType);
            return typeIsDynamic || propertyInfo.HasAttribute<DynamicTypeAttribute>();
        }

        public bool HasIgnoredTypes()
        { return Configuration.IgnoredTypes.Any(); }

        public bool IsIgnoredType(Type type)
        { return !Configuration.IgnoredTypes.Any(type.IsAssignableFrom); }

        public bool IsDefaultPrimitiveType(Type type)
        {
            return type.IsPrimitive ||
                   type == typeof(string) ||
                   type == typeof(DateTime) ||
                   type == typeof(Vector2) ||
                   type == typeof(Vector3) ||
                   type == typeof(Vector4) ||
                   type == typeof(Quaternion) ||
                   type == typeof(Guid) ||
                   type.IsEnum;
        }

        public bool IsNullablePrimitiveType(Type type)
        {
            var nullableType = Nullable.GetUnderlyingType(type);
            if(nullableType == null) { return false; }
            return IsDefaultPrimitiveType(nullableType);
        }
        
        public bool ShouldTreatAsPrimitiveType(Type type)
        { return Configuration.TreatAsPrimitives.Any(x => type == x); }

        public bool IsPrimitiveType(Type type)
        {
            var isDefaultPrimitive = IsDefaultPrimitiveType(type);

            if (isDefaultPrimitive)
            { return true; }

            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
            {
                var isNullablePrimitive = IsDefaultPrimitiveType(nullableType);
                if (isNullablePrimitive) { return true; }
            }

            return ShouldTreatAsPrimitiveType(type);
        }
    }
}