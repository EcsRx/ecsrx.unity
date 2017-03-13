using System;
using System.Collections.Generic;
using System.IO;
using EcsRx.Json;
using UnityEngine;

namespace Tests.Editor.Helpers.Mapping
{
    public class BinaryDeserializer
    {
        private object DeserializePrimitive(Type type, BinaryReader reader)
        {
            if (type == typeof(byte)) { return reader.ReadByte(); }
            if (type == typeof(short)) { return reader.ReadInt16(); }
            if (type == typeof(int)) { return reader.ReadInt32(); }
            if (type == typeof(bool)) { return reader.ReadBoolean(); }
            if (type == typeof(float)) { return reader.ReadSingle(); }
            if (type == typeof(double)) { return reader.ReadDouble(); }
            if (type == typeof(decimal)) { return reader.ReadDecimal(); }
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
            
            return reader.ReadString();
        }

        public T DeserializeData<T>(TypePropertyMappings typePropertyMappings, byte[] data) where T : new()
        {
            using(var memoryStream = new MemoryStream(data))
            using (var reader = new BinaryReader(memoryStream))
            {
                var instance = new T();
                Deserialize(typePropertyMappings.Mappings, instance, reader);
                return instance;
            }
        }

        private void DeserializeProperty<T>(PropertyMapping propertyMapping, T instance, BinaryReader reader)
        {
            var underlyingValue = DeserializePrimitive(propertyMapping.Type, reader);
            propertyMapping.SetValue(instance, underlyingValue);
        }

        private void DeserializeNestedObject<T>(NestedMapping nestedMapping, T instance, BinaryReader reader)
        { Deserialize(nestedMapping.InternalMappings, instance, reader); }

        private void DeserializeCollection<T>(CollectionPropertyMapping collectionMapping, T[] instance, int arrayCount, BinaryReader reader)
        {
            for (var i = 0; i < arrayCount; i++)
            {
                var elementInstance = (T)Activator.CreateInstance(collectionMapping.ArrayType);
                Deserialize(collectionMapping.InternalMappings, elementInstance, reader);
                instance[i] = elementInstance;
            }
        }

        private void Deserialize<T>(IEnumerable<Mapping> mappings, T instance, BinaryReader reader)
        {
            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                { DeserializeProperty((mapping as PropertyMapping), instance, reader); }
                else if (mapping is NestedMapping)
                {
                    var nestedMapping = (mapping as NestedMapping);
                    var childInstance = Activator.CreateInstance(nestedMapping.Type);
                    DeserializeNestedObject(nestedMapping, childInstance, reader);
                    nestedMapping.SetValue(instance, childInstance);
                }
                else
                {
                    var collectionMapping = (mapping as CollectionPropertyMapping);
                    var arrayCount = reader.ReadInt32();
                    var arrayInstance = (object[])Activator.CreateInstance(collectionMapping.Type, arrayCount);
                    DeserializeCollection(collectionMapping, arrayInstance, arrayCount, reader);
                    collectionMapping.SetValue(instance, arrayInstance);
                }
            }
        }
    }
}