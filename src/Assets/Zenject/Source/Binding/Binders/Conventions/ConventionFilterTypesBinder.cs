#if !(UNITY_WSA && ENABLE_DOTNET)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModestTree;

namespace Zenject
{
    public class ConventionFilterTypesBinder : ConventionAssemblySelectionBinder
    {
        public ConventionFilterTypesBinder(ConventionBindInfo bindInfo)
            : base(bindInfo)
        {
        }

        public ConventionFilterTypesBinder DerivingFromOrEqual<T>()
        {
            return DerivingFromOrEqual(typeof(T));
        }

        public ConventionFilterTypesBinder DerivingFromOrEqual(Type parentType)
        {
            BindInfo.AddTypeFilter((type) => type.DerivesFromOrEqual(parentType));
            return this;
        }

        public ConventionFilterTypesBinder DerivingFrom<T>()
        {
            return DerivingFrom(typeof(T));
        }

        public ConventionFilterTypesBinder DerivingFrom(Type parentType)
        {
            BindInfo.AddTypeFilter((type) => type.DerivesFrom(parentType));
            return this;
        }

        public ConventionFilterTypesBinder WithAttribute<T>()
            where T : Attribute
        {
            return WithAttribute(typeof(T));
        }

        public ConventionFilterTypesBinder WithAttributeWhere<T>(Func<T, bool> predicate)
            where T : Attribute
        {
            BindInfo.AddTypeFilter(t => t.HasAttribute<T>() && t.AllAttributes<T>().All(predicate));
            return this;
        }

        public ConventionFilterTypesBinder WithAttribute(Type attribute)
        {
            Assert.That(attribute.DerivesFrom<Attribute>());
            BindInfo.AddTypeFilter(t => t.HasAttribute(attribute));
            return this;
        }

        public ConventionFilterTypesBinder Where(Func<Type, bool> predicate)
        {
            BindInfo.AddTypeFilter(predicate);
            return this;
        }

        public ConventionFilterTypesBinder InNamespace(string ns)
        {
            return InNamespaces(ns);
        }

        public ConventionFilterTypesBinder InNamespaces(params string[] namespaces)
        {
            return InNamespaces((IEnumerable<string>)namespaces);
        }

        public ConventionFilterTypesBinder InNamespaces(IEnumerable<string> namespaces)
        {
            BindInfo.AddTypeFilter(t => namespaces.Any(n => IsInNamespace(t, n)));
            return this;
        }

        static bool IsInNamespace(Type type, string requiredNs)
        {
            var actualNs = type.Namespace ?? "";

            if (requiredNs.Length > actualNs.Length)
            {
                return false;
            }

            return actualNs.StartsWith(requiredNs)
                && (actualNs.Length == requiredNs.Length || actualNs[requiredNs.Length] == '.');
        }
    }
}

#endif
