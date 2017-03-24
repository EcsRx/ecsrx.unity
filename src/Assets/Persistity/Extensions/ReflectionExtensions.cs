using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Persistity.Extensions
{
    /// <summary>
    /// This is all stolen from Zenject, just this way there is not a hard dependency on it
    /// as Zenject is only used in the Unity layer. So this way you can still use the persistence
    /// layer without Zenject.
    /// </summary>
    public static class ReflectionExtensions
    {
        public static IEnumerable<Attribute> AllAttributes(this MemberInfo provider, params Type[] attributeTypes)
        {
            var allAttributes = provider.GetCustomAttributes(true).Cast<Attribute>();

            if (attributeTypes.Length == 0)
            {
                return allAttributes;
            }

            return allAttributes.Where(a => attributeTypes.Any(x => a.GetType().DerivesFromOrEqual(x)));
        }

        public static bool DerivesFromOrEqual(this Type a, Type b)
        {
#if UNITY_WSA && !UNITY_EDITOR
            return b == a || b.GetTypeInfo().IsAssignableFrom(a.GetTypeInfo());
#else
            return b == a || b.IsAssignableFrom(a);
#endif
        }

        public static bool HasAttribute(
            this ParameterInfo provider, params Type[] attributeTypes)
        {
            return provider.AllAttributes(attributeTypes).Any();
        }

        public static bool HasAttribute<T>(this MemberInfo provider)
            where T : Attribute
        {
            return provider.AllAttributes(typeof(T)).Any();
        }

        public static bool HasAttribute<T>(this ParameterInfo provider)
            where T : Attribute
        {
            return provider.AllAttributes(typeof(T)).Any();
        }

        public static IEnumerable<T> AllAttributes<T>(
            this ParameterInfo provider)
            where T : Attribute
        {
            return provider.AllAttributes(typeof(T)).Cast<T>();
        }

        public static IEnumerable<Attribute> AllAttributes(
            this ParameterInfo provider, params Type[] attributeTypes)
        {
            var allAttributes = provider.GetCustomAttributes(true).Cast<Attribute>();

            if (attributeTypes.Length == 0)
            {
                return allAttributes;
            }

            return allAttributes.Where(a => attributeTypes.Any(x => a.GetType().DerivesFromOrEqual(x)));
        }

    }
}