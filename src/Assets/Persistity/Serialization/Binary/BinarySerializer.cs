using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Persistity.Exceptions;
using Persistity.Mappings;
using UnityEngine;

namespace Persistity.Serialization.Binary
{
    public class BinarySerializer : IBinarySerializer
    {
        public static readonly char[] NullDataSig = {(char) 141, (char) 141};
        public static readonly char[] NullObjectSig = {(char) 141, (char)229, (char)141};

        public BinaryConfiguration Configuration { get; private set; }

        public BinarySerializer(BinaryConfiguration configuration = null)
        { Configuration = configuration ?? BinaryConfiguration.Default; }

        public void WriteNullData(BinaryWriter writer)
        { writer.Write(NullDataSig); }

        public void WriteNullObject(BinaryWriter writer)
        { writer.Write(NullObjectSig); }

        public void SerializePrimitive(object value, Type type, BinaryWriter writer)
        {
            if (type == typeof(byte)) { writer.Write((byte)value); }
            else if (type == typeof(short)) { writer.Write((short)value); }
            else if(type == typeof(int)) { writer.Write((int)value); }
            else if(type == typeof(long)) { writer.Write((long)value); }
            else if(type == typeof(bool)) { writer.Write((bool)value); }
            else if(type == typeof(float)) { writer.Write((float)value); }
            else if(type == typeof(double)) { writer.Write((double)value); }
            else if(type == typeof(decimal)) { writer.Write((decimal)value); }
            else if(type.IsEnum) { writer.Write((int)value); }
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
            else if (type == typeof(DateTime)) { writer.Write(((DateTime)value).ToBinary()); }
            else if (type == typeof(Guid)) { writer.Write(((Guid)value).ToString()); }
            else if (type == typeof(string)) { writer.Write(value.ToString()); }
            else
            {
                var matchingHandler = Configuration.TypeHandlers.SingleOrDefault(x => x.MatchesType(type));
                if(matchingHandler == null) { throw new NoKnownTypeException(type); }
                matchingHandler.HandleTypeIn(writer, value);
            }
        }

        public byte[] SerializeData<T>(TypeMapping typeMapping, T data) where T : new()
        { return SerializeData(typeMapping, (object) data); }

        public byte[] SerializeData(TypeMapping typeMapping, object data)
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

        public void SerializeProperty<T>(PropertyMapping propertyMapping, T data, BinaryWriter writer)
        {
            if (data == null)
            {
                WriteNullData(writer);
                return;
            }

            var underlyingValue = propertyMapping.GetValue(data);

            if (underlyingValue == null)
            {
                WriteNullData(writer);
                return;
            }

            SerializePrimitive(underlyingValue, propertyMapping.Type, writer);
        }

        public void SerializeNestedObject<T>(NestedMapping nestedMapping, T data, BinaryWriter writer)
        {
            if (data == null)
            {
                WriteNullObject(writer);
                return;
            }

            var currentData = nestedMapping.GetValue(data);

            if (currentData == null)
            {
                WriteNullObject(writer);
                return;
            }

            Serialize(nestedMapping.InternalMappings, currentData, writer);
        }
        
        public void SerializeCollection<T>(CollectionMapping collectionMapping, T data, BinaryWriter writer)
        {
            if (data == null)
            {
                WriteNullObject(writer);
                return;
            }

            var collectionValue = collectionMapping.GetValue(data);

            if (collectionValue == null)
            {
                WriteNullObject(writer);
                return;
            }

            writer.Write(collectionValue.Count);
            for (var i = 0; i < collectionValue.Count; i++)
            {
                var currentData = collectionValue[i];
                if (currentData == null)
                { WriteNullObject(writer); }
                else if (collectionMapping.InternalMappings.Count > 0)
                { Serialize(collectionMapping.InternalMappings, currentData, writer); }
                else
                { SerializePrimitive(currentData, collectionMapping.CollectionType, writer); }
            }
        }

        public void SerializeDictionary<T>(DictionaryMapping dictionaryMapping, T data, BinaryWriter writer)
        {
            if (data == null)
            {
                WriteNullObject(writer);
                return;
            }

            var dictionaryValue = dictionaryMapping.GetValue(data);

            if (dictionaryValue == null)
            {
                WriteNullObject(writer);
                return;
            }

            writer.Write(dictionaryValue.Count);

            foreach (var key in dictionaryValue.Keys)
            {
                var currentValue = dictionaryValue[key];

                if (dictionaryMapping.KeyMappings.Count > 0)
                { Serialize(dictionaryMapping.KeyMappings, key, writer); }
                else
                { SerializePrimitive(key, dictionaryMapping.KeyType, writer); }

                if (currentValue == null)
                { WriteNullData(writer); }
                else if (dictionaryMapping.ValueMappings.Count > 0)
                { Serialize(dictionaryMapping.ValueMappings, currentValue, writer); }
                else
                { SerializePrimitive(currentValue, dictionaryMapping.ValueType, writer); }
            }
        }

        public void Serialize<T>(IEnumerable<Mapping> mappings, T data, BinaryWriter writer)
        {
            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                { SerializeProperty((mapping as PropertyMapping), data, writer); }
                else if (mapping is NestedMapping)
                { SerializeNestedObject((mapping as NestedMapping), data, writer); }
                else if(mapping is DictionaryMapping)
                { SerializeDictionary(mapping as DictionaryMapping, data, writer);}
                else
                { SerializeCollection((mapping as CollectionMapping), data, writer); }
            }
        }
    }
}