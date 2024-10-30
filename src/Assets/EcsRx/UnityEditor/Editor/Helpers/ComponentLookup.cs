using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;

namespace EcsRx.UnityEditor.Editor.Helpers
{
    public static class ComponentLookup
    {
        public static IEnumerable<Type> AllComponents { get; private set; }

        static ComponentLookup()
        {
            AllComponents = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(IsComponent);
        }

        private static bool IsComponent(Type type)
        {
            return typeof(IComponent).IsAssignableFrom(type) &&
                   type.IsClass;

        }
    }
}