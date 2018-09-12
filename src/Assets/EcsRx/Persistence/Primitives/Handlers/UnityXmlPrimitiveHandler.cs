using System;
using System.Xml.Linq;
using EcsRx.Persistence.Primitives.Checkers;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Serialization.Xml.Handlers;
using UnityEngine;

namespace EcsRx.Persistence.Primitives.Handlers
{
    public class UnityXmlPrimitiveHandler : IXmlPrimitiveHandler
    {
        public IPrimitiveChecker PrimitiveChecker { get; } = new UnityPrimitiveChecker();

        public void Serialize(XElement state, object data, Type type)
        {
            if (type == typeof(Vector2))
            {
                var typedObject = (Vector2)data;
                state.Add(new XElement("x", typedObject.x));
                state.Add(new XElement("y", typedObject.y));
                return;
            }
            if (type == typeof(Vector3))
            {
                var typedObject = (Vector3)data;
                state.Add(new XElement("x", typedObject.x));
                state.Add(new XElement("y", typedObject.y));
                state.Add(new XElement("z", typedObject.z));
                return;
            }
            if (type == typeof(Vector4))
            {
                var typedObject = (Vector4)data;
                state.Add(new XElement("x", typedObject.x));
                state.Add(new XElement("y", typedObject.y));
                state.Add(new XElement("z", typedObject.z));
                state.Add(new XElement("w", typedObject.w));
                return;
            }
            if (type == typeof(Quaternion))
            {
                var typedObject = (Quaternion)data;
                state.Add(new XElement("x", typedObject.x));
                state.Add(new XElement("y", typedObject.y));
                state.Add(new XElement("z", typedObject.z));
                state.Add(new XElement("w", typedObject.w));
                return;
            }
        }

        public object Deserialize(XElement state, Type type)
        {
            if (type == typeof(Vector2))
            {
                var x = float.Parse(state.Element("x").Value);
                var y = float.Parse(state.Element("y").Value);
                return new Vector2(x, y);
            }
            if (type == typeof(Vector3))
            {
                var x = float.Parse(state.Element("x").Value);
                var y = float.Parse(state.Element("y").Value);
                var z = float.Parse(state.Element("z").Value);
                return new Vector3(x, y, z);
            }
            if (type == typeof(Vector4))
            {
                var x = float.Parse(state.Element("x").Value);
                var y = float.Parse(state.Element("y").Value);
                var z = float.Parse(state.Element("z").Value);
                var w = float.Parse(state.Element("w").Value);
                return new Vector4(x, y, z, w);
            }
            if (type == typeof(Quaternion))
            {
                var x = float.Parse(state.Element("x").Value);
                var y = float.Parse(state.Element("y").Value);
                var z = float.Parse(state.Element("z").Value);
                var w = float.Parse(state.Element("w").Value);
                return new Quaternion(x, y, z, w);
            }

            return null;
        }
    }
}