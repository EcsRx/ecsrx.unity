using System;
using EcsRx.Persistence.Transformers;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Persistity.Registries;
using Persistity.Serialization.Json;
using UnityEngine;
using Zenject;
using TypeAnalyzer = Persistity.Mappings.Types.TypeAnalyzer;

namespace EcsRx.Persistence.Installers
{
    public class DefaultPersistanceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var ignoredTypes = new[] { typeof(GameObject), typeof(MonoBehaviour) };
            var analyzerConfiguration = new TypeAnalyzerConfiguration
            {
                IgnoredTypes = ignoredTypes,
                TreatAsPrimitives = new Type[0] //TODO Add Reactive Handlers
            };
            Container.Bind<ITypeAnalyzer>().FromInstance(new TypeAnalyzer(analyzerConfiguration)).AsSingle();
            Container.Bind<ITypeCreator>().To<TypeCreator>().AsSingle();
            Container.Bind<ITypeMapper>().To<EverythingTypeMapper>().AsSingle();
            Container.Bind<IMappingRegistry>().To<MappingRegistry>().AsSingle();

            Container.Bind<IJsonSerializer>().To<JsonSerializer>().AsSingle();
            Container.Bind<IJsonDeserializer>().To<JsonDeserializer>().AsSingle();

            Container.Bind<IEntityDataTransformer>().To<EntityDataTransformer>().AsSingle();
            Container.Bind<IPoolDataTransformer>().To<PoolDataTransformer>().AsSingle();
        }
    }
}