#if !(UNITY_WSA && ENABLE_DOTNET)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModestTree;

namespace Zenject
{
    public class ConventionBindInfo
    {
        readonly List<Func<Type, bool>> _typeFilters = new List<Func<Type, bool>>();
        readonly List<Func<Assembly, bool>> _assemblyFilters = new List<Func<Assembly, bool>>();

        public void AddAssemblyFilter(Func<Assembly, bool> predicate)
        {
            _assemblyFilters.Add(predicate);
        }

        public void AddTypeFilter(Func<Type, bool> predicate)
        {
            _typeFilters.Add(predicate);
        }

        IEnumerable<Assembly> GetAllAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        bool ShouldIncludeAssembly(Assembly assembly)
        {
            return _assemblyFilters.All(predicate => predicate(assembly));
        }

        bool ShouldIncludeType(Type type)
        {
            return _typeFilters.All(predicate => predicate(type));
        }

        public List<Type> ResolveTypes()
        {
            return GetAllAssemblies()
                .Where(ShouldIncludeAssembly)
                .SelectMany(assembly => assembly.GetTypes())
                .Where(ShouldIncludeType).ToList();
        }
    }
}

#endif
