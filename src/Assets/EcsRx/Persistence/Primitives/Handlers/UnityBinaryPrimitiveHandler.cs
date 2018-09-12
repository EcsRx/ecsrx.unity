using System;
using System.IO;
using EcsRx.Persistence.Primitives.Checkers;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Serialization.Binary.Handlers;
using UnityEngine;

namespace EcsRx.Persistence.Primitives.Handlers
{   
    public class UnityBinaryPrimitiveHandler : IBinaryPrimitiveHandler
    {
        public IPrimitiveChecker PrimitiveChecker { get; } = new UnityPrimitiveChecker();

        public void Serialize(BinaryWriter state, object data, Type type)
        {
            if (type == typeof(Vector2))
            {
                var vector = (Vector2)data;
                state.Write(vector.x);
                state.Write(vector.y);
            }
            else if (type == typeof(Vector3))
            {
                var vector = (Vector3)data;
                state.Write(vector.x);
                state.Write(vector.y);
                state.Write(vector.z);
            }
            else if (type == typeof(Vector4))
            {
                var vector = (Vector4)data;
                state.Write(vector.x);
                state.Write(vector.y);
                state.Write(vector.z);
                state.Write(vector.w);
            }
            else if (type == typeof(Quaternion))
            {
                var quaternion = (Quaternion)data;
                state.Write(quaternion.x);
                state.Write(quaternion.y);
                state.Write(quaternion.z);
                state.Write(quaternion.w);
            }
        }

        public object Deserialize(BinaryReader state, Type type)
        {
            if (type == typeof(Vector2))
            {
                var x = state.ReadSingle();
                var y = state.ReadSingle();
                return new Vector2(x, y);
            }
            if (type == typeof(Vector3))
            {
                var x = state.ReadSingle();
                var y = state.ReadSingle();
                var z = state.ReadSingle();
                return new Vector3(x, y, z);
            }
            if (type == typeof(Vector4))
            {
                var x = state.ReadSingle();
                var y = state.ReadSingle();
                var z = state.ReadSingle();
                var w = state.ReadSingle();
                return new Vector4(x, y, z, w);
            }
            if (type == typeof(Quaternion))
            {
                var x = state.ReadSingle();
                var y = state.ReadSingle();
                var z = state.ReadSingle();
                var w = state.ReadSingle();
                return new Quaternion(x, y, z, w);
            }

            return null;
        }
    }
}