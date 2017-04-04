using System;
using System.Reflection;

namespace Persistity.Mappings.Types
{
    public interface ITypeAnalyzer
    {
        bool IsGenericList(Type type);
        bool IsGenericDictionary(Type type);
        bool IsDynamicType(Type type);
        bool IsDynamicType(PropertyInfo propertyInfo);
        bool IsDefaultPrimitiveType(Type type);
        bool IsNullablePrimitiveType(Type type);
        bool HasIgnoredTypes();
        bool IsIgnoredType(Type type);
        bool ShouldTreatAsPrimitiveType(Type type);
        bool IsPrimitiveType(Type type);
    }
}