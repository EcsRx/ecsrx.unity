using System;
using EcsRx.Components;
using EcsRx.Persistence.Data;
using Persistity.Mappings.Mappers;
using Persistity.Registries;
using Persistity.Serialization.Json;
using Persistity.Transformers;
using Persistity.Transformers.Json;
using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours.Helpers
{
    public static class EntityTransformer
    {
        private static readonly ITransformer _transformer;

        static EntityTransformer()
        {
            var ignoredTypes = new[] {typeof(GameObject), typeof(MonoBehaviour)};
            var mapperConfiguration = new MappingConfiguration
            {
                IgnoredTypes = ignoredTypes
            };
            var mapper = new EverythingTypeMapper(mapperConfiguration);
            var _mappingRegistry = new MappingRegistry(mapper);
            _transformer = new JsonTransformer(new JsonSerializer(), new JsonDeserializer(), _mappingRegistry);
        }

        public static byte[] SerializeComponent(IComponent component)
        { return _transformer.Transform(component.GetType(), component); }

        public static IComponent DeserializeComponent(string componentReference, byte[] data)
        {
            var componentType = TypeHelper.GetTypeWithAssembly(componentReference);
            if (componentType == null) { throw new Exception("Cannot resolve type for [" + componentReference + "]"); }

            return (IComponent)_transformer.Transform(componentType, data);
        }

        public static IComponent DeserializeComponent(ComponentData componentData)
        {
            var componentType = TypeHelper.GetTypeWithAssembly(componentData.ComponentTypeReference);
            if (componentType == null) { throw new Exception("Cannot resolve type for [" + componentData.ComponentTypeReference + "]"); }

            return (IComponent)_transformer.Transform(componentType, componentData.ComponentState);
        }
    }
}