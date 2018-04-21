using UnityEngine;
using System.Collections;
using System;

namespace cocosocket4unity
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ResponseMappingAttribute : Attribute
    {
        public Type type { get; set; }
    }
}
