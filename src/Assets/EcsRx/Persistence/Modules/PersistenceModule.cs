using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Persistence.Primitives.Checkers;
using EcsRx.Persistence.Primitives.Handlers;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Mappings.Types.Primitives;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Registries;
using LazyData.Serialization.Binary;
using LazyData.Serialization.Binary.Handlers;
using LazyData.Serialization.Json;
using LazyData.Serialization.Json.Handlers;
using LazyData.Serialization.Xml;
using LazyData.Serialization.Xml.Handlers;
using UnityEngine;

namespace EcsRx.Persistence.Modules
{
    public class PersistenceModule : IDependencyModule
    {       
        public void Setup(IDependencyContainer container)
        {
            var ignoredTypes = new[] { typeof(GameObject), typeof(MonoBehaviour), typeof(Behaviour) };
            var typeAnalyzerConfiguration = new TypeAnalyzerConfiguration(ignoredTypes);
            container.Bind<TypeAnalyzerConfiguration>(x =>
            {
                x.ToInstance(typeAnalyzerConfiguration)
                    .AsSingleton();
            });

            var primitiveRegistry = new PrimitiveRegistry();
            primitiveRegistry.AddPrimitiveCheck(new BasicPrimitiveChecker());
            primitiveRegistry.AddPrimitiveCheck(new ReactivePrimitiveChecker());
            primitiveRegistry.AddPrimitiveCheck(new UnityPrimitiveChecker());

            container.Bind<IPrimitiveRegistry>(x =>
            {
                x.ToInstance(primitiveRegistry)
                    .AsSingleton();
            });
            
            container.Bind<ITypeCreator, TypeCreator>();
            container.Bind<ITypeAnalyzer, TypeAnalyzer>();
            container.Bind<ITypeMapper, EverythingTypeMapper>();
            container.Bind<IMappingRegistry, MappingRegistry>();

            container.Bind<IJsonPrimitiveHandler, BasicJsonPrimitiveHandler>();
            container.Bind<IJsonPrimitiveHandler, ReactiveJsonPrimitiveHandler>();
            container.Bind<IJsonPrimitiveHandler, UnityJsonPrimitiveHandler>();
            container.Bind<IJsonSerializer, JsonSerializer>();
            container.Bind<IJsonDeserializer, JsonDeserializer>();
            
            container.Bind<IBinaryPrimitiveHandler, BasicBinaryPrimitiveHandler>();
            container.Bind<IBinaryPrimitiveHandler, ReactiveBinaryPrimitiveHandler>();
            container.Bind<IBinaryPrimitiveHandler, UnityBinaryPrimitiveHandler>();
            container.Bind<IBinarySerializer, BinarySerializer>();
            container.Bind<IBinaryDeserializer, BinaryDeserializer>();
            
            container.Bind<IXmlPrimitiveHandler, BasicXmlPrimitiveHandler>();
            container.Bind<IXmlPrimitiveHandler, ReactiveXmlPrimitiveHandler>();
            container.Bind<IXmlPrimitiveHandler, UnityXmlPrimitiveHandler>();
            container.Bind<IXmlSerializer, XmlSerializer>();
            container.Bind<IXmlDeserializer, XmlDeserializer>();
        }
    }
}