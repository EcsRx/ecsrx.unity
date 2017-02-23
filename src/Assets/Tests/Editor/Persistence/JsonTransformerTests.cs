using System;
using System.Linq;
using EcsRx.Json;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Transformers;
using NUnit.Framework;

namespace Assets.Tests.Editor.Persistence
{
    [TestFixture]
    public class JsonTransformerTests
    {
        [Test]
        public void should_correctly_serialize_entity_data()
        {
            var entityData = new EntityData { EntityId = Guid.NewGuid() };
            entityData.Data.Add("npc.name", "Bob");
            entityData.Data.Add("stats.health", 20);
            entityData.Data.Add("position.x", 1.5f);
            entityData.Data.Add("position.y", 2.1f);
            entityData.Data.Add("position.z", 3.3f);

            var transformer = new JsonTransformer(null);
            var jsonEntity = transformer.TransformEntity(entityData);
            Console.WriteLine(jsonEntity.ToString());

            Assert.That(jsonEntity[transformer.EntityIdKey].Value, Is.EqualTo(entityData.EntityId.ToString()));

            var jsonData = jsonEntity[transformer.DataKey];
            Assert.That(jsonData.Count, Is.EqualTo(5));
            Assert.That(jsonData["npc.name"].Value, Is.EqualTo("Bob"));
            Assert.That(jsonData["stats.health"].Value, Is.EqualTo("20"));
            Assert.That(jsonData["position.x"].Value, Is.EqualTo("1.5"));
            Assert.That(jsonData["position.y"].Value, Is.EqualTo("2.1"));
            Assert.That(jsonData["position.z"].Value, Is.EqualTo("3.3"));
        }

        [Test]
        public void should_correctly_deserialize_entity_data()
        {
            var entityString = "{ \"entityId\":\"67c12ef1-5d0b-4eee-9432-838efb0c958b\", \"data\": { \"npc.name\":\"Bob\", \"stats.health\":20,	\"position.x\":\"1.5\",	\"position.y\":\"2.1\",	\"position.z\":\"3.3\" }}";
            var jsonEntity = JSON.Parse(entityString);
            Console.WriteLine(jsonEntity.ToString());

            var transformer = new JsonTransformer(null);
            var entityData = transformer.TransformEntity(jsonEntity);

            Console.WriteLine(entityData.ToString());
        }
    }
}