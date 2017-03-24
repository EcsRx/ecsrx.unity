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
using Persistity.Mappings.Mappers;
using Persistity.Registries;
using Persistity.Serialization.Json;
using Persistity.Transformers.Json;
using UnityEngine;

namespace Tests.Editor.Persistence
{
    [TestFixture]
    public class SanityTests
    {
        [Test]
        public void should_correctly_transform_entity()
        {
            var ignoredTypes = new[] {typeof(GameObject), typeof(MonoBehaviour), typeof(IEventSystem)};
            var mapperConfiguration = new MappingConfiguration
            {
                IgnoredTypes = ignoredTypes,
                KnownPrimitives = new Type[0],
            };
            var mapper = new EverythingTypeMapper(mapperConfiguration);
            var _mappingRegistry = new MappingRegistry(mapper);
            var transformer = new JsonTransformer(new JsonSerializer(), new JsonDeserializer(), _mappingRegistry);

            var mockEventSystem = Substitute.For<IEventSystem>();
            var entityConvertor = new EntityConvertor(transformer, mockEventSystem);
            
            var entity = new Entity(Guid.NewGuid(), mockEventSystem);
            entity.ApplyBlueprint(new PlayerBlueprint("bob"));

            var entityData = entityConvertor.ConvertToData(entity);

            var outputData = transformer.Transform(entityData.GetType(), entityData);
            Console.WriteLine(Encoding.Default.GetString(outputData));

            var deserializedEntityData = (EntityData)transformer.Transform(typeof(EntityData), outputData);
            Console.WriteLine(deserializedEntityData.ToString());

            var reconstructedEntity = entityConvertor.ConvertFromData(deserializedEntityData);
            Assert.That(reconstructedEntity.Id, Is.EqualTo(entity.Id));
            Assert.That(reconstructedEntity.Components.Count(), Is.EqualTo(entity.Components.Count()));

            var reconstructedHasName = reconstructedEntity.GetComponent<HasName>();
            var originalHasName = entity.GetComponent<HasName>();
            Assert.That(reconstructedHasName.Name, Is.EqualTo(originalHasName.Name));

            var reconstructedWithHealth = reconstructedEntity.GetComponent<WithHealthComponent>();
            var originalWithHealth = entity.GetComponent<WithHealthComponent>();
            Assert.That(reconstructedWithHealth.CurrentHealth, Is.EqualTo(originalWithHealth.CurrentHealth));
            Assert.That(reconstructedWithHealth.MaxHealth, Is.EqualTo(originalWithHealth.MaxHealth));
        }
    }
}