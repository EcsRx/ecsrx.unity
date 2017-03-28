using System;
using Assets.EcsRx.Examples.GroupFilters.Blueprints;
using Assets.EcsRx.Examples.PooledViews.Blueprints;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Processors;
using EcsRx.Persistence.Transformers;
using NSubstitute;
using NUnit.Framework;
using Persistity.Mappings.Mappers;
using Persistity.Registries;
using Persistity.Serialization.Json;
using UnityEngine;

namespace Tests.Editor.Persistence
{
    [TestFixture]
    public class ApplicationDataSanityTests
    {
        private IJsonSerializer _serializer;
        private IJsonDeserializer _deserializer;
        private IEntityDataTransformer _entityTransformer;
        private IEventSystem _eventSystem;

        [SetUp]
        public void Setup()
        {
            var ignoredTypes = new[] { typeof(GameObject), typeof(MonoBehaviour), typeof(IEventSystem) };
            var mapperConfiguration = new MappingConfiguration
            {
                IgnoredTypes = ignoredTypes,
                KnownPrimitives = new Type[0],
            };
            var mapper = new EverythingTypeMapper(mapperConfiguration);
            var mappingRegistry = new MappingRegistry(mapper);
            _serializer = new JsonSerializer(mappingRegistry);
            _deserializer = new JsonDeserializer(mappingRegistry);
            _eventSystem = Substitute.For<IEventSystem>();
            _entityTransformer = new EntityDataTransformer(_serializer, _deserializer, _eventSystem);
        }

        private void HandleError(Exception error)
        { Assert.Fail(error.Message); }

        [Test]
        public void should_correctly_serialize_and_transform_application_data()
        {
            var applicationDatabase = new ApplicationDatabase();

            var entity1 = new Entity(Guid.NewGuid(), _eventSystem);
            entity1.ApplyBlueprint(new PlayerBlueprint("Tom", 0));

            var entity2 = new Entity(Guid.NewGuid(), _eventSystem);
            entity2.ApplyBlueprint(new PlayerBlueprint("TP", 1000));

            var entity3 = new Entity(Guid.NewGuid(), _eventSystem);
            entity3.ApplyBlueprint(new PlayerBlueprint("Bob", 10));

            var entity1Data = (EntityData)_entityTransformer.TransformTo(entity1);
            var entity2Data = (EntityData)_entityTransformer.TransformTo(entity2);
            var entity3Data = (EntityData)_entityTransformer.TransformTo(entity3);
            applicationDatabase.EntityData.Add(entity1Data);
            applicationDatabase.EntityData.Add(entity2Data);
            applicationDatabase.EntityData.Add(entity3Data);

            var output = _serializer.Serialize(applicationDatabase);
            var processedOutput = new RemoveEscapeCharacterProcessor().Process(output);
            Console.WriteLine(processedOutput.AsString);
        }
    }
}