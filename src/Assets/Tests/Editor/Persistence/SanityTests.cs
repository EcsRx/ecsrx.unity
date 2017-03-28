using System;
using System.Linq;
using System.Text;
using Assets.EcsRx.Examples.UsingBlueprints.Blueprints;
using Assets.EcsRx.Examples.UsingBlueprints.Components;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Extractors;
using NSubstitute;
using NUnit.Framework;
using Persistity.Endpoints.Files;
using Persistity.Endpoints.InMemory;
using Persistity.Mappings.Mappers;
using Persistity.Pipelines.Builders;
using Persistity.Registries;
using Persistity.Serialization.Json;
using UnityEngine;

namespace Tests.Editor.Persistence
{
    [TestFixture]
    public class SanityTests
    {
        private JsonSerializer _serializer;
        private JsonDeserializer _deserializer;
        private EntityDataTransformer _entityTransformer;
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

        private void CompareEntities(IEntity actualEntity, IEntity expectedEntity)
        {
            Assert.That(actualEntity.Id, Is.EqualTo(expectedEntity.Id));
            Assert.That(actualEntity.Components.Count(), Is.EqualTo(expectedEntity.Components.Count()));

            var reconstructedHasName = actualEntity.GetComponent<HasName>();
            var originalHasName = expectedEntity.GetComponent<HasName>();
            Assert.That(reconstructedHasName.Name, Is.EqualTo(originalHasName.Name));

            var reconstructedWithHealth = actualEntity.GetComponent<WithHealthComponent>();
            var originalWithHealth = expectedEntity.GetComponent<WithHealthComponent>();
            Assert.That(reconstructedWithHealth.CurrentHealth, Is.EqualTo(originalWithHealth.CurrentHealth));
            Assert.That(reconstructedWithHealth.MaxHealth, Is.EqualTo(originalWithHealth.MaxHealth));
        }

        [Test]
        public void should_correctly_transform_entity()
        {
            var expectedEntity = new Entity(Guid.NewGuid(), _eventSystem);
            expectedEntity.ApplyBlueprint(new PlayerBlueprint("bob"));

            var entityData = _entityTransformer.TransformTo(expectedEntity);

            var outputData = _serializer.Serialize(entityData);
            Console.WriteLine(outputData.AsString);

            var deserializedEntityData = _deserializer.Deserialize<EntityData>(outputData);
            Console.WriteLine(deserializedEntityData.ToString());

            var reconstructedEntity = (Entity)_entityTransformer.TransformFrom(deserializedEntityData);
            CompareEntities(reconstructedEntity, expectedEntity);
        }

        [Test]
        public void should_correctly_consume_entity_through_pipeline()
        {
            var inMemoryEndpoint = new InMemoryEndpoint();

            var saveEntityPipeline = new PipelineBuilder()
                .SerializeWith(_serializer)
                .TransformWith(_entityTransformer)
                .SendTo(inMemoryEndpoint)
                .Build();

            var loadEntityPipeline = new PipelineBuilder()
                .RecieveFrom(inMemoryEndpoint)
                .DeserializeWith(_deserializer)
                .TransformWith(_entityTransformer)
                .Build();
            
            var expectedEntity = new Entity(Guid.NewGuid(), _eventSystem);
            expectedEntity.ApplyBlueprint(new PlayerBlueprint("bob"));

            saveEntityPipeline.Execute(expectedEntity, x =>
            {
                loadEntityPipeline.Execute<IEntity>(actualEntity =>
                {
                    CompareEntities(actualEntity, expectedEntity);
                }, HandleError);
            }, HandleError);
        }
    }
}