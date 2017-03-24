using EcsRx.Components;
using Persistity.Mappings.Mappers;
using Persistity.Registries;
using Persistity.Serialization.Json;
using Persistity.Transformers;
using Persistity.Transformers.Json;

namespace EcsRx.Unity.Helpers
{
    public static class EntityTransformer
    {
        private static ITransformer _transformer;

        static EntityTransformer()
        {
            var _mappingRegistry = new MappingRegistry(new EverythingTypeMapper());
            _transformer = new JsonTransformer(new JsonSerializer(), new JsonDeserializer(), _mappingRegistry);
        }

        public static byte[] TransformComponent(IComponent component)
        { return _transformer.Transform(component.GetType(), component); }
    }
}