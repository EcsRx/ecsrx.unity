using System;
using System.Linq;

namespace Persistity.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsTypeOf(this Type type, params Type[] types)
        { return types.Any(x => x == type); }

        public static string GetPersistableName(this Type type)
        { return type.FullName; }
    }
}