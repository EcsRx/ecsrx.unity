using System;
using System.Collections.Generic;
using System.IO;
using EcsRx.Json;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tests.Editor.Helpers.Mapping
{
    public class BinarySerializer
    {
        private void SerializePrimitive(object value, Type type, BinaryWriter writer)
        {
            if (type == typeof(byte)) { writer.Write((byte)value); }
            else if (type == typeof(short)) { writer.Write((short)value); }
            else if(type == typeof(int)) { writer.Write((int)value); }
            else if(type == typeof(long)) { writer.Write((long)value); }
            else if(type == typeof(bool)) { writer.Write((bool)value); }
            else if(type == typeof(float)) { writer.Write((float)value); }
            else if(type == typeof(double)) { writer.Write((double)value); }
            else if(type == typeof(decimal)) { writer.Write((decimal)value); }
            else if (type == typeof(Vector2))
            {
                var vector = (Vector2) value;
                writer.Write(vector.x);
                writer.Write(vector.y);
            }
            else if (type == typeof(Vector3))
            {
                var vector = (Vector3)value;
                writer.Write(vector.x);
                writer.Write(vector.y);
                writer.Write(vector.z);
            }
            else if (type == typeof(Vector4))
            {
                var vector = (Vector4)value;
                writer.Write(vector.x);
                writer.Write(vector.y);
                writer.Write(vector.z);
                writer.Write(vector.w);
            }
            else if (type == typeof(Quaternion))
            {
                var quaternion = (Quaternion) value;
                writer.Write(quaternion.x);
                writer.Write(quaternion.y);
                writer.Write(quaternion.z);
                writer.Write(quaternion.w);
            }
            else
            {
                writer.Write(value.ToString());
            }
        }

        public byte[] SerializeData<T>(TypeMapping typeMapping, T data)
        {
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                Serialize(typeMapping.InternalMappings, data, binaryWriter);
                binaryWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                return memoryStream.ToArray();
            }
        }

        private void SerializeProperty<T>(PropertyMapping propertyMapping, T data, BinaryWriter writer)
        {
            var underlyingValue = propertyMapping.GetValue(data);
            SerializePrimitive(underlyingValue, propertyMapping.Type, writer);
        }

        private void SerializeNestedObject<T>(NestedMapping nestedMapping, T data, BinaryWriter writer)
        {
            var currentData = nestedMapping.GetValue(data);
            Serialize(nestedMapping.InternalMappings, currentData, writer);
        }
        
        private void SerializeCollection<T>(CollectionPropertyMapping collectionMapping, T data, BinaryWriter writer)
        {
            var collectionValue = collectionMapping.GetValue(data);
            writer.Write(collectionValue.Count);
            for (var i = 0; i < collectionValue.Count; i++)
            {
                var currentData = collectionValue[i];

                if (collectionMapping.InternalMappings.Count > 0)
                { Serialize(collectionMapping.InternalMappings, currentData, writer); }
                else
                { SerializePrimitive(currentData, collectionMapping.CollectionType, writer); }
            }
        }

        private void Serialize<T>(IEnumerable<Mapping> mappings, T data, BinaryWriter writer)
        {
            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                { SerializeProperty((mapping as PropertyMapping), data, writer); }
                else if (mapping is NestedMapping)
                { SerializeNestedObject((mapping as NestedMapping), data, writer); }
                else
                { SerializeCollection((mapping as CollectionPropertyMapping), data, writer); }
            }
        }
    }
}