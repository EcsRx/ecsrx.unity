using System;
using System.Xml.Linq;
using UnityEngine;

namespace Persistity.Serialization.Xml
{
    public class XmlPrimitiveDeserializer
    {
        public object DeserializeDefaultPrimitive(Type type, XElement element)
        {
            if (type == typeof(byte)) { return byte.Parse(element.Value); }
            if (type == typeof(short)) { return short.Parse(element.Value); }
            if (type == typeof(int)) { return int.Parse(element.Value); }
            if (type == typeof(long)) { return long.Parse(element.Value); }
            if (type == typeof(bool)) { return bool.Parse(element.Value); }
            if (type == typeof(float)) { return float.Parse(element.Value); }
            if (type == typeof(double)) { return double.Parse(element.Value); }
            if (type == typeof(decimal)) { return decimal.Parse(element.Value); }
            if (type.IsEnum) { return Enum.Parse(type, element.Value); }
            if (type == typeof(Vector2))
            {
                var x = float.Parse(element.Element("x").Value);
                var y = float.Parse(element.Element("y").Value);
                return new Vector2(x, y);
            }
            if (type == typeof(Vector3))
            {
                var x = float.Parse(element.Element("x").Value);
                var y = float.Parse(element.Element("y").Value);
                var z = float.Parse(element.Element("z").Value);
                return new Vector3(x, y, z);
            }
            if (type == typeof(Vector4))
            {
                var x = float.Parse(element.Element("x").Value);
                var y = float.Parse(element.Element("y").Value);
                var z = float.Parse(element.Element("z").Value);
                var w = float.Parse(element.Element("w").Value);
                return new Vector4(x, y, z, w);
            }
            if (type == typeof(Quaternion))
            {
                var x = float.Parse(element.Element("x").Value);
                var y = float.Parse(element.Element("y").Value);
                var z = float.Parse(element.Element("z").Value);
                var w = float.Parse(element.Element("w").Value);
                return new Quaternion(x, y, z, w);
            }
            if (type == typeof(Guid))
            {
                return new Guid(element.Value);
            }
            if (type == typeof(DateTime))
            {
                var binaryTime = long.Parse(element.Value);
                return DateTime.FromBinary(binaryTime);
            }

            return element.Value;
        }
    }
}