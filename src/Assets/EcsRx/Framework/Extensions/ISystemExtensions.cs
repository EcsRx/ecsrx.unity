using System;
using System.Linq;
using EcsRx.Groups;
using EcsRx.Systems;

namespace EcsRx.Extensions
{
    public static class ISystemExtensions
    {
        public static IGroup GroupFor(this ISystem system, params Type[] componentTypes)
        { return new Group(componentTypes); }

        public static bool IsSystemReactive(this ISystem system)
        { return system is IReactToEntitySystem || system is IReactToGroupSystem; }
    }
}