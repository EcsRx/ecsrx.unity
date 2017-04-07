using System;
using Newtonsoft.Json.Linq;
using Persistity.Extensions;
using UnityEngine;

namespace Persistity.Serialization.Json
{
    public class JsonPrimitiveSerializer
    {
        private readonly Type[] CatchmentTypes =
        {
            typeof(string), typeof(bool), typeof(byte), typeof(short), typeof(int),
            typeof(long), typeof(Guid), typeof(float), typeof(double), typeof(decimal)
        };
        
        public void SerializeDefaultPrimitive(object value, Type type, JToken element)
        {
            if (type == typeof(Vector2))
            {
                var typedObject = (Vector2)value;
                element["x"] = typedObject.x;
                element["y"] = typedObject.y;
                return;
            }
            if (type == typeof(Vector3))
            {
                var typedObject = (Vector3)value;
                element["x"] = typedObject.x;
                element["y"] = typedObject.y;
                element["z"] = typedObject.z;
                return;
            }
            if (type == typeof(Vector4))
            {
                var typedObject = (Vector4)value;
                element["x"] = typedObject.x;
                element["y"] = typedObject.y;
                element["z"] = typedObject.z;
                element["w"] = typedObject.w;
                return;
            }
            if (type == typeof(Quaternion))
            {
                var typedObject = (Quaternion)value;
                element["x"] = typedObject.x;
                element["y"] = typedObject.y;
                element["z"] = typedObject.z;
                element["w"] = typedObject.w;
                return;
            }
            if (type == typeof(DateTime))
            {
                var typedValue = (DateTime)value;
                var stringValue = typedValue.ToBinary().ToString();
                element.Replace(new JValue(stringValue));
                return;
            }

            if (type.IsTypeOf(CatchmentTypes) || type.IsEnum)
            {
                element.Replace(new JValue(value));
                return;
            }
        }
    }
}