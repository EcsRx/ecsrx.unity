using EcsRx.Components;
using EcsRx.Persistence.Data;
using Persistity;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Persistity.Registries;
using Persistity.Serialization;
using Persistity.Serialization.Json;
using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours.Helpers
{
    public static class EntitySerializer
    {
        private static readonly ISerializer _serializer;
        private static readonly IDeserializer _deserializer;

        static EntitySerializer()
        {
            var ignoredTypes = new[] {typeof(GameObject), typeof(MonoBehaviour)};
            var typeAnalyzerConfiguration = new TypeAnalyzerConfiguration
            {
                IgnoredTypes = ignoredTypes
            };
            var typeAnalyzer = new TypeAnalyzer(typeAnalyzerConfiguration);
            var typeCreator = new TypeCreator();
            var mapper = new EverythingTypeMapper(typeAnalyzer);
            var _mappingRegistry = new MappingRegistry(mapper);
            _serializer = new JsonSerializer(_mappingRegistry);
            _deserializer = new JsonDeserializer(_mappingRegistry, typeCreator);
        }

        public static DataObject SerializeComponent(IComponent component)
        { return _serializer.Serialize(component); }

        public static IComponent DeserializeComponent(DataObject data)
        { return (IComponent)_deserializer.Deserialize(data); }

        public static IComponent DeserializeComponent(ComponentData componentData)
        { return (IComponent)_deserializer.Deserialize(new DataObject(componentData.ComponentState.State)); }
    }
}