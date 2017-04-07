using System;
using System.IO;
using UnityEngine;

namespace Persistity.Serialization.Binary
{
    public class BinaryPrimitiveDeserializer
    {
        public object DeserializeDefaultPrimitive(Type type, BinaryReader reader)
        {
            if (type == typeof(byte)) { return reader.ReadByte(); }
            if (type == typeof(short)) { return reader.ReadInt16(); }
            if (type == typeof(int)) { return reader.ReadInt32(); }
            if (type == typeof(long)) { return reader.ReadInt64(); }
            if (type == typeof(bool)) { return reader.ReadBoolean(); }
            if (type == typeof(float)) { return reader.ReadSingle(); }
            if (type == typeof(double)) { return reader.ReadDouble(); }
            if (type == typeof(decimal)) { return reader.ReadDecimal(); }
            if (type.IsEnum)
            {
                var value = reader.ReadInt32();
                return Enum.ToObject(type, value);
            }
            if (type == typeof(Vector2))
            {
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                return new Vector2(x, y);
            }
            if (type == typeof(Vector3))
            {
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = reader.ReadSingle();
                return new Vector3(x, y, z);
            }
            if (type == typeof(Vector4))
            {
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = reader.ReadSingle();
                var w = reader.ReadSingle();
                return new Vector4(x, y, z, w);
            }
            if (type == typeof(Quaternion))
            {
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = reader.ReadSingle();
                var w = reader.ReadSingle();
                return new Quaternion(x, y, z, w);
            }
            if (type == typeof(Guid))
            {
                return new Guid(reader.ReadString());
            }
            if (type == typeof(DateTime))
            {
                var binaryTime = reader.ReadInt64();
                return DateTime.FromBinary(binaryTime);
            }

            return reader.ReadString();
        }
    }
}