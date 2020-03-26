using System;
using System.Linq;

namespace EcsRx.UnityEditor.Editor.Helpers
{
    public class TypeHelper
    {
        public static Type GetTypeWithAssembly(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }

        public static Type TryGetConvertedType(string typeName)
        {
            var type = Type.GetType(typeName);
            var namePortions = typeName.Split(',')[0].Split('.');
            typeName = namePortions.Last();

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] assemblyTypes = a.GetTypes();
                for (int j = 0; j < assemblyTypes.Length; j++)
                {
                    if (typeName == assemblyTypes[j].Name)
                    {
                        type = assemblyTypes[j];
                        if (type != null)
                        {
                            return type;
                        }
                    }
                }
            }
            return null;
        }
    }
}