using System;
using System.Xml.Linq;
using Persistity.Extensions;
using UnityEngine;

namespace Persistity.Serialization.Xml
{
    public class XmlPrimitiveSerializer
    {
        private readonly Type[] CatchmentTypes =
        {
            typeof(string), typeof(bool), typeof(byte), typeof(short), typeof(int),
            typeof(long), typeof(Guid), typeof(float), typeof(double), typeof(decimal)
        };

        public void SerializeDefaultPrimitive(object value, Type type, XElement element)
        {
            if (type == typeof(Vector2))
            {
                var typedObject = (Vector2)value;
                element.Add(new XElement("x", typedObject.x));
                element.Add(new XElement("y", typedObject.y));
                return;
            }
            if (type == typeof(Vector3))
            {
                var typedObject = (Vector3)value;
                element.Add(new XElement("x", typedObject.x));
                element.Add(new XElement("y", typedObject.y));
                element.Add(new XElement("z", typedObject.z));
                return;
            }
            if (type == typeof(Vector4))
            {
                var typedObject = (Vector4)value;
                element.Add(new XElement("x", typedObject.x));
                element.Add(new XElement("y", typedObject.y));
                element.Add(new XElement("z", typedObject.z));
                element.Add(new XElement("w", typedObject.w));
                return;
            }
            if (type == typeof(Quaternion))
            {
                var typedObject = (Quaternion)value;
                element.Add(new XElement("x", typedObject.x));
                element.Add(new XElement("y", typedObject.y));
                element.Add(new XElement("z", typedObject.z));
                element.Add(new XElement("w", typedObject.w));
                return;
            }
            if (type == typeof(DateTime))
            {
                var typedValue = (DateTime)value;
                var stringValue = typedValue.ToBinary().ToString();
                element.Value = stringValue;
                return;
            }

            if (type.IsTypeOf(CatchmentTypes) || type.IsEnum)
            {
                element.Value = value.ToString();
                return;
            }
        }
    }
}