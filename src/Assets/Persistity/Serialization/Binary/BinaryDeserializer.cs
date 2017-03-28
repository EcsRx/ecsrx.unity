using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Persistity.Mappings;
using Persistity.Registries;
using UnityEngine;

namespace Persistity.Serialization.Binary
{
    public class BinaryDeserializer : IBinaryDeserializer
    {
        public IMappingRegistry MappingRegistry { get; private set; }
        public BinaryConfiguration Configuration { get; private set; }

        public BinaryDeserializer(IMappingRegistry mappingRegistry, BinaryConfiguration configuration = null)
        {
            MappingRegistry = mappingRegistry;
            Configuration = configuration ?? BinaryConfiguration.Default;
        }

        public bool IsDataNull(BinaryReader reader)
        {
            var currentPosition = reader.BaseStream.Position;

            foreach (var nullChar in BinarySerializer.NullDataSig)
            {
                var readChar = reader.ReadChar();
                if (nullChar != readChar)
                {
                    reader.BaseStream.Position = currentPosition;
                    return false;
                }
            }
            return true;
        }

        public bool IsObjectNull(BinaryReader reader)
        {
            var currentPosition = reader.BaseStream.Position;

            foreach (var nullChar in BinarySerializer.NullObjectSig)
            {
                var readChar = reader.ReadChar();
                if (nullChar != readChar)
                {
                    reader.BaseStream.Position = currentPosition;
                    return false;
                }
            }
            return true;
        }

        public object DeserializePrimitive(Type type, BinaryReader reader)
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

            var matchingHandler = Configuration.TypeHandlers.SingleOrDefault(x => x.MatchesType(type));
            if (matchingHandler != null)
            { return matchingHandler.HandleTypeOut(reader); }

            return reader.ReadString();
        }

        public T Deserialize<T>(DataObject data) where T : new()
        { return (T)Deserialize(data); }

        public object Deserialize(DataObject data)
        {
            using (var memoryStream = new MemoryStream(data.AsBytes))
            using (var reader = new BinaryReader(memoryStream))
            {
                var typeName = reader.ReadString();
                var type = Type.GetType(typeName);
                var typeMapping = MappingRegistry.GetMappingFor(type);
                var instance = Activator.CreateInstance(type);
                Deserialize(typeMapping.InternalMappings, instance, reader);
                return instance;
            }
        }

        public void DeserializeProperty<T>(PropertyMapping propertyMapping, T instance, BinaryReader reader)
        {
            if (IsDataNull(reader))
            { propertyMapping.SetValue(instance, null); }
            else
            {
                var underlyingValue = DeserializePrimitive(propertyMapping.Type, reader);
                propertyMapping.SetValue(instance, underlyingValue);
            }
        }

        public void DeserializeNestedObject<T>(NestedMapping nestedMapping, T instance, BinaryReader reader)
        { Deserialize(nestedMapping.InternalMappings, instance, reader); }

        public void DeserializeCollection(CollectionMapping collectionMapping, IList collectionInstance, int arrayCount, BinaryReader reader)
        {
            for (var i = 0; i < arrayCount; i++)
            {
                if (IsObjectNull(reader))
                {
                    if (collectionInstance.IsFixedSize)
                    { collectionInstance[i] = null; }
                    else
                    { collectionInstance.Insert(i, null); }
                }
                else if (collectionMapping.InternalMappings.Count > 0)
                {
                    var elementInstance = Activator.CreateInstance(collectionMapping.CollectionType);
                    Deserialize(collectionMapping.InternalMappings, elementInstance, reader);

                    if (collectionInstance.IsFixedSize)
                    { collectionInstance[i] = elementInstance; }
                    else
                    { collectionInstance.Insert(i, elementInstance); }
                }
                else
                {
                    object value = null;
                    if(!IsDataNull(reader))
                    { value = DeserializePrimitive(collectionMapping.CollectionType, reader); }
                    if (collectionInstance.IsFixedSize)
                    { collectionInstance[i] = value; }
                    else
                    { collectionInstance.Insert(i, value); }
                }
            }
        }

        public void DeserializeDictionary(DictionaryMapping dictionaryMapping, IDictionary dictionaryInstance, int dictionaryCount, BinaryReader reader)
        {
            for (var i = 0; i < dictionaryCount; i++)
            {
                object keyInstance, valueInstance;
                if (dictionaryMapping.KeyMappings.Count > 0)
                {
                    keyInstance = Activator.CreateInstance(dictionaryMapping.KeyType);
                    Deserialize(dictionaryMapping.KeyMappings, keyInstance, reader);
                }
                else
                { keyInstance = DeserializePrimitive(dictionaryMapping.KeyType, reader); }

                if (IsDataNull(reader))
                { valueInstance = null; }
                else if (dictionaryMapping.ValueMappings.Count > 0)
                {
                    valueInstance = Activator.CreateInstance(dictionaryMapping.ValueType);
                    Deserialize(dictionaryMapping.ValueMappings, valueInstance, reader);
                }
                else
                { valueInstance = DeserializePrimitive(dictionaryMapping.ValueType, reader); }

                dictionaryInstance.Add(keyInstance, valueInstance);
            }
        }

        public void Deserialize<T>(IEnumerable<Mapping> mappings, T instance, BinaryReader reader)
        {
            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                { DeserializeProperty((mapping as PropertyMapping), instance, reader); }
                else if (mapping is NestedMapping)
                {
                    var nestedMapping = (mapping as NestedMapping);
                    if (IsObjectNull(reader))
                    {
                        nestedMapping.SetValue(instance, null);
                        continue;
                    }

                    var childInstance = Activator.CreateInstance(nestedMapping.Type);
                    DeserializeNestedObject(nestedMapping, childInstance, reader);
                    nestedMapping.SetValue(instance, childInstance);
                }
                else if (mapping is DictionaryMapping)
                {
                    var dictionaryMapping = (mapping as DictionaryMapping);
                    if (IsObjectNull(reader))
                    {
                        dictionaryMapping.SetValue(instance, null);
                        continue;
                    }

                    var dictionarytype = typeof(Dictionary<,>);
                    var dictionaryCount = reader.ReadInt32();
                    var constructedDictionaryType = dictionarytype.MakeGenericType(dictionaryMapping.KeyType, dictionaryMapping.ValueType);
                    var dictionary = (IDictionary)Activator.CreateInstance(constructedDictionaryType);
                    DeserializeDictionary(dictionaryMapping, dictionary, dictionaryCount, reader);
                    dictionaryMapping.SetValue(instance, dictionary);
                }
                else
                {
                    var collectionMapping = (mapping as CollectionMapping);
                    if (IsObjectNull(reader))
                    {
                        collectionMapping.SetValue(instance, null);
                        continue;
                    }

                    var arrayCount = reader.ReadInt32();

                    if (collectionMapping.IsArray)
                    {
                        var arrayInstance = (IList) Activator.CreateInstance(collectionMapping.Type, arrayCount);
                        DeserializeCollection(collectionMapping, arrayInstance, arrayCount, reader);
                        collectionMapping.SetValue(instance, arrayInstance);
                    }
                    else
                    {
                        var listType = typeof(List<>);
                        var constructedListType = listType.MakeGenericType(collectionMapping.CollectionType);
                        var listInstance = (IList)Activator.CreateInstance(constructedListType);
                        DeserializeCollection(collectionMapping, listInstance, arrayCount, reader);
                        collectionMapping.SetValue(instance, listInstance);
                    }
                }
            }
        }
    }
}