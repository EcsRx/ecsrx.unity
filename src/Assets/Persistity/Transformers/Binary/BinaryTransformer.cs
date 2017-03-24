using System;
using Persistity.Registries;
using Persistity.Serialization.Binary;

namespace Persistity.Transformers.Binary
{
    public class BinaryTransformer : IBinaryTransformer
    {
        public IBinarySerializer Serializer { get; set; }
        public IBinaryDeserializer Deserializer { get; set; }
        public IMappingRegistry MappingRegistry { get; set; }

        public BinaryTransformer(IBinarySerializer serializer, IBinaryDeserializer deserializer, IMappingRegistry mappingRegistry)
        {
            Serializer = serializer;
            Deserializer = deserializer;
            MappingRegistry = mappingRegistry;
        }

        public byte[] Transform<T>(T data) where T : new()
        {
            var typeMapping = MappingRegistry.GetMappingFor<T>();
            return Serializer.SerializeData(typeMapping, data);
        }

        public byte[] Transform(Type type, object data)
        {
            var typeMapping = MappingRegistry.GetMappingFor(type);
            return Serializer.SerializeData(typeMapping, data);
        }

        public T Transform<T>(byte[] data) where T : new()
        {
            var typeMapping = MappingRegistry.GetMappingFor<T>();
            return Deserializer.DeserializeData<T>(typeMapping, data);
        }

        public object Transform(Type type, byte[] data)
        {
            var typeMapping = MappingRegistry.GetMappingFor(type);
            return Deserializer.DeserializeData(typeMapping, data);
        }
    }
}