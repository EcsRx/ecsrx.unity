using System;
using Assets.EcsRx.Examples.GroupFilters.Blueprints;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Database;
using EcsRx.Persistence.Transformers;
using EcsRx.Persistence.TypeHandlers.Reactive;
using EcsRx.Pools;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Persistity.Registries;
using Persistity.Serialization;
using Persistity.Serialization.Json;
using UniRx;
using UnityEngine;


namespace Tests.Editor.Persistence
{
    [TestFixture]
    public class ApplicationDataSanityTests
    {
        private ISerializer _serializer;
        private IDeserializer _deserializer;
        private IEntityDataTransformer _entityTransformer;
        private IPoolDataTransformer _poolTransformer;
        private IEventSystem _eventSystem;
        private IEntityFactory _entityFactory;

        [SetUp]
        public void Setup()
        {
            var ignoredTypes = new[] { typeof(GameObject), typeof(MonoBehaviour) };
            var mapperConfiguration = new TypeAnalyzerConfiguration
            {
                IgnoredTypes = ignoredTypes,
                TreatAsPrimitives = new[] { typeof(ReactiveProperty<>) }
            };
            var typeAnalyzer = new TypeAnalyzer(mapperConfiguration);
            var mapper = new EverythingTypeMapper(typeAnalyzer);
            var mappingRegistry = new MappingRegistry(mapper);

            _eventSystem = Substitute.For<IEventSystem>();
            _entityFactory = Substitute.For<IEntityFactory>();

            _entityTransformer = new EntityDataTransformer(_eventSystem);
            _poolTransformer = new PoolDataTransformer(_entityTransformer, _eventSystem, _entityFactory);

            var jsonConfiguration = new JsonConfiguration
            {
                TypeHandlers = new ITypeHandler<JToken, JToken>[]
                {
                    new ReactiveJsonTypeHandler()
                }
            };
            _serializer = new JsonSerializer(mappingRegistry, jsonConfiguration);
            _deserializer = new JsonDeserializer(mappingRegistry, new TypeCreator(), jsonConfiguration);
                /*
            var configuration = new BinaryConfiguration
            {
                TypeHandlers = new ITypeHandler<BinaryWriter, BinaryReader>[]
                {
                    new ReactiveBinaryTypeHandler()
                }
            };
            _serializer = new BinarySerializer(mappingRegistry, configuration);
            _deserializer = new BinaryDeserializer(mappingRegistry, new TypeCreator(), configuration);*/

        }

        private void HandleError(Exception error)
        { Assert.Fail(error.Message); }

        [Test]
        public void should_correctly_serialize_and_transform_application_data()
        {
            var applicationDatabase = new ApplicationDatabase();

            var pool = new Pool("dummy", _entityFactory, _eventSystem);
            
            var entity1 = new Entity(Guid.NewGuid(), _eventSystem);
            entity1.ApplyBlueprint(new PlayerBlueprint("Tom", 0));

            var entity2 = new Entity(Guid.NewGuid(), _eventSystem);
            entity2.ApplyBlueprint(new PlayerBlueprint("TP", 1000));

            var entity3 = new Entity(Guid.NewGuid(), _eventSystem);
            entity3.ApplyBlueprint(new PlayerBlueprint("Bob", 10));

            pool.AddEntity(entity1);
            pool.AddEntity(entity2);
            pool.AddEntity(entity3);

            var poolData = (PoolData)_poolTransformer.TransformTo(pool);
            applicationDatabase.Pools.Add(poolData);
            
            var output = _serializer.Serialize(applicationDatabase);

            var reconstructedDatabase = _deserializer.Deserialize<ApplicationDatabase>(output);

            Console.WriteLine(output.AsString);
            Console.WriteLine(reconstructedDatabase);
        }
    }
}