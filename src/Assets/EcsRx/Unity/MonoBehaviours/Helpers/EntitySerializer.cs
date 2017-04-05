using EcsRx.Components;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Transformers;
using Persistity;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Persistity.Registries;
using Persistity.Serialization;
using Persistity.Serialization.Json;
using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours.Helpers
{
    public class EntitySelfSerializer
    {
        private ISerializer _serializer;
        private IDeserializer _deserializer;
        private IEntityDataTransformer _transformer;

        public EntitySelfSerializer(ISerializer serializer, IDeserializer deserializer, IEntityDataTransformer transformer)
        {
            _serializer = serializer;
            _deserializer = deserializer;
            _transformer = transformer;
        }

        public DataObject Serialize()
        {
            return new DataObject();
        }

        public EntityData Deserialize()
        {
            return null;
        }
    }
}