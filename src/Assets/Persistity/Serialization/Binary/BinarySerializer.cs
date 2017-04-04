using System;
using System.IO;
using Persistity.Extensions;
using Persistity.Registries;
using UnityEngine;

namespace Persistity.Serialization.Binary
{
    public class BinarySerializer : GenericSerializer<BinaryWriter, BinaryReader>, IBinarySerializer
    {
        public static readonly char[] NullDataSig = {(char) 141, (char) 141};
        public static readonly char[] NullObjectSig = {(char) 141, (char)229, (char)141};
        
        public BinarySerializer(IMappingRegistry mappingRegistry, BinaryConfiguration configuration = null) : base(mappingRegistry)
        {
            Configuration = configuration ?? BinaryConfiguration.Default;
        }

        protected override void HandleNullData(BinaryWriter state)
        { state.Write(NullDataSig); }

        protected override void HandleNullObject(BinaryWriter state)
        { state.Write(NullObjectSig); }

        protected override void AddCountToState(BinaryWriter state, int count)
        { state.Write(count); }

        protected override BinaryWriter GetDynamicTypeState(BinaryWriter state, Type type)
        {
            state.Write(type.GetPersistableName());
            return state;
        }

        protected override void SerializeDefaultPrimitive(object value, Type type, BinaryWriter state)
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
        
        public override DataObject Serialize(object data)
        {
            var typeMapping = MappingRegistry.GetMappingFor(data.GetType());
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write(typeMapping.Type.GetPersistableName());
                Serialize(typeMapping.InternalMappings, data, binaryWriter);
                binaryWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                return new DataObject(memoryStream.ToArray());
            }
        }
    }
}