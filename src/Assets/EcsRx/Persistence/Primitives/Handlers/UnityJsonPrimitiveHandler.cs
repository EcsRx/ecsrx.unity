using System;
using EcsRx.Persistence.Primitives.Checkers;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Serialization.Json.Handlers;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace EcsRx.Persistence.Primitives.Handlers
{   
    public class UnityJsonPrimitiveHandler : IJsonPrimitiveHandler
    {
        public IPrimitiveChecker PrimitiveChecker { get; } = new UnityPrimitiveChecker();

        public void Serialize(JToken state, object data, Type type)
        {
            if (type == typeof(Vector2))
            {
                var typedObject = (Vector2)data;
                state["x"] = typedObject.x;
                state["y"] = typedObject.y;
                return;
            }
            if (type == typeof(Vector3))
            {
                var typedObject = (Vector3)data;
                state["x"] = typedObject.x;
                state["y"] = typedObject.y;
                state["z"] = typedObject.z;
                return;
            }
            if (type == typeof(Vector4))
            {
                var typedObject = (Vector4)data;
                state["x"] = typedObject.x;
                state["y"] = typedObject.y;
                state["z"] = typedObject.z;
                state["w"] = typedObject.w;
                return;
            }
            if (type == typeof(Quaternion))
            {
                var typedObject = (Quaternion)data;
                state["x"] = typedObject.x;
                state["y"] = typedObject.y;
                state["z"] = typedObject.z;
                state["w"] = typedObject.w;
                return;
            }
        }

        public object Deserialize(JToken state, Type type)
        {
            if (type == typeof(Vector2))
            { return new Vector2(state["x"].ToObject<float>(), state["y"].ToObject<float>()); }
            if (type == typeof(Vector3))
            { return new Vector3(state["x"].ToObject<float>(), state["y"].ToObject<float>(), state["z"].ToObject<float>()); }
            if (type == typeof(Vector4))
            { return new Vector4(state["x"].ToObject<float>(), state["y"].ToObject<float>(), state["z"].ToObject<float>(), state["w"].ToObject<float>()); }
            if (type == typeof(Quaternion))
            { return new Quaternion(state["x"].ToObject<float>(), state["y"].ToObject<float>(), state["z"].ToObject<float>(), state["w"].ToObject<float>()); }

            return null;
        }
    }
}