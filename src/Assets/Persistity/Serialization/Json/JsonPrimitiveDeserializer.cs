using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Persistity.Serialization.Json
{
    public class JsonPrimitiveDeserializer
    {
        public object DeserializeDefaultPrimitive(Type type, JToken state)
        {
            if (type == typeof(DateTime))
            {
                var binaryDate = state.ToObject<long>();
                return DateTime.FromBinary(binaryDate);
            }
            if (type.IsEnum) { return Enum.Parse(type, state.ToString()); }
            if (type == typeof(Vector2))
            { return new Vector2(state["x"].ToObject<float>(), state["y"].ToObject<float>()); }
            if (type == typeof(Vector3))
            { return new Vector3(state["x"].ToObject<float>(), state["y"].ToObject<float>(), state["z"].ToObject<float>()); }
            if (type == typeof(Vector4))
            { return new Vector4(state["x"].ToObject<float>(), state["y"].ToObject<float>(), state["z"].ToObject<float>(), state["w"].ToObject<float>()); }
            if (type == typeof(Quaternion))
            { return new Quaternion(state["x"].ToObject<float>(), state["y"].ToObject<float>(), state["z"].ToObject<float>(), state["w"].ToObject<float>()); }

            return state.ToObject(type);
        }
    }
}