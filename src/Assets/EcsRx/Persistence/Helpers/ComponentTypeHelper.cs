using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;

namespace Assets.EcsRx.Persistence.Helpers
{
    public class ComponentTypeHelper
    {
        public Type IComponentType { get; private set; }
        public IList<Type> AllComponentTypes { get; private set; }

        public ComponentTypeHelper()
        {
            IComponentType = typeof(IComponent);
            RefreshComponentList();
        }

        public void RefreshComponentList()
        {
            AllComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => IComponentType.IsAssignableFrom(x))
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