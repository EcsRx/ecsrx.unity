using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EcsRx.Components;

namespace EcsRx.Persistence.Types
{
    public class ComponentTypeRegistry : IComponentTypeRegistry
    {
        public Type IComponentType { get; private set; }
        public IList<Type> AllComponentTypes { get; private set; }
        
        public ComponentTypeRegistry()
        {
            IComponentType = typeof(IComponent);
            RefreshComponentList();
        }

        public void RefreshComponentList()
        {
            var assemblyList = AppDomain.CurrentDomain.GetAssemblies().ToList();

            AllComponentTypes = assemblyList
                .SelectMany(x => x.GetTypes())
                .Where(x => IComponentType.IsAssignableFrom(x))
                .Where(x => x != IComponentType)
                .ToList();
        }
        
        public Type GetComponentFromName(string name)
        {
            var sanitisedName = name.ToLower().Replace("component", "");

            // TODO: could potentially have components of same name in other namespaces
            return AllComponentTypes.SingleOrDefault(x => x.Name.ToLower().Replace("component", "") == sanitisedName);
        }
    }
}