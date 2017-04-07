using System;
using System.IO;
using UnityEngine;

namespace Persistity.Serialization.Binary
{
    public class BinaryPrimitiveSerializer
    {
        public void SerializeDefaultPrimitive(object value, Type type, BinaryWriter state)
        {
            if (type == typeof(byte)) { state.Write((byte)value); }
            else if (type == typeof(short)) { state.Write((short)value); }
            else if (type == typeof(int)) { state.Write((int)value); }
            else if (type == typeof(long)) { state.Write((long)value); }
            else if (type == typeof(bool)) { state.Write((bool)value); }
            else if (type == typeof(float)) { state.Write((float)value); }
            else if (type == typeof(double)) { state.Write((double)value); }
            else if (type == typeof(decimal)) { state.Write((decimal)value); }
            else if (type.IsEnum) { state.Write((int)value); }
            else if (type == typeof(Vector2))
            {
                var vector = (Vector2)value;
                state.Write(vector.x);
                state.Write(vector.y);
            }
            else if (type == typeof(Vector3))
            {
                var vector = (Vector3)value;
                state.Write(vector.x);
                state.Write(vector.y);
                state.Write(vector.z);
            }
            else if (type == typeof(Vector4))
            {
                var vector = (Vector4)value;
                state.Write(vector.x);
                state.Write(vector.y);
                state.Write(vector.z);
                state.Write(vector.w);
            }
            else if (type == typeof(Quaternion))
            {
                var quaternion = (Quaternion)value;
                state.Write(quaternion.x);
                state.Write(quaternion.y);
                state.Write(quaternion.z);
                state.Write(quaternion.w);
            }
            else if (type == typeof(DateTime)) { state.Write(((DateTime)value).ToBinary()); }
            else if (type == typeof(Guid)) { state.Write(((Guid)value).ToString()); }
            else if (type == typeof(string)) { state.Write(value.ToString()); }
        }
    }
}