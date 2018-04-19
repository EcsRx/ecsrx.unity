using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace cocosocket4unity
{
    class ClassUtil
    {
       public static List<Type> GetClasses(string nameSpace)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            List<Type> list = new List<Type>();
            foreach (Type type in asm.GetTypes())
            {
                if (nameSpace.Equals(type.Namespace))
                    list.Add(type);
            }
            return list;
        }
       public static Attribute GetAttribute(Type claz, Type att)
       {
           Attribute classAttribute = Attribute.GetCustomAttribute(claz, att);
           return classAttribute;
       }
    }
}
