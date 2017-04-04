using System;
using System.Collections.Generic;
using System.IO;
using Assets.EcsRx.Examples.GroupFilters.Blueprints;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Transformers;
using EcsRx.Persistence.TypeHandlers;
using NSubstitute;
using NUnit.Framework;
using Persistity.Json;
using Persistity.Mappings.Mappers;
using Persistity.Registries;
using Persistity.Serialization;
using Persistity.Serialization.Binary;
using Persistity.Serialization.Json;
using UnityEngine;

namespace Tests.Editor.Persistence
{
    [TestFixture]
    public class PerformanceTests
    {
        private IEventSystem _eventSystem;
        private MappingRegistry _mappingRegistry;

        [SetUp]
        public void Setup()
        {
            var ignoredTypes = new[] { typeof(GameObject), typeof(MonoBehaviour), typeof(IEventSystem) };
            var mapperConfiguration = new MappingConfiguration
            {
                IgnoredTypes = ignoredTypes,
                KnownPrimitives = new[] { typeof(StateData) },
            };
            var mapper = new EverythingTypeMapper(mapperConfiguration);

            _mappingRegistry = new MappingRegistry(mapper);
            _eventSystem = Substitute.For<IEventSystem>();
        }

        [Test]
        public void purely_a_json_performance_test()
        {
            var jsonConfig = new JsonConfiguration
            {
                TypeHandlers = new List<ITypeHandler<JSONNode, JSONNode>> { new JsonStateDataHandler() }
            };
            var serializer = new JsonSerializer(_mappingRegistry, jsonConfig);
            var deserializer = new JsonDeserializer(_mappingRegistry, jsonConfig);
            
            var entityTransformer = new EntityDataTransformer(serializer, deserializer, _eventSystem);

            var applicationDatabase = new ApplicationDatabase();
            var entityCount = 1000;

            for (var i = 0; i < entityCount; i++)
            {
                var entity = new Entity(Guid.NewGuid(), _eventSystem);
                entity.ApplyBlueprint(new PlayerBlueprint("Player " + i, i));

                var entityData = (EntityData)entityTransformer.TransformTo(entity);
                applicationDatabase.EntityData.Add(entityData);
            }

            var startTime = DateTime.Now;
            var output = serializer.Serialize(applicationDatabase);
            var endTime = DateTime.Now;
            var totalTime = endTime - startTime;
            var averageMsPerEntity = totalTime.TotalMilliseconds / entityCount;
            Console.WriteLine("Total Data {0} bytes", output.AsBytes.Length);
            Console.WriteLine("Serialized {0} Entities In {1} - Averaging {2:F2}ms Per Entity", entityCount, totalTime, averageMsPerEntity);

            startTime = DateTime.Now;
            deserializer.Deserialize<ApplicationDatabase>(output);
            endTime = DateTime.Now;
            totalTime = endTime - startTime;
            averageMsPerEntity = totalTime.TotalMilliseconds / entityCount;
            Console.WriteLine("Deserialized {0} Entities In {1} - Averaging {2:F2}ms Per Entity", entityCount, totalTime, averageMsPerEntity);
        }

        [Test]
        public void purely_a_binary_performance_test()
        {
            var binaryConfig = new BinaryConfiguration
            {
                TypeHandlers = new List<ITypeHandler<BinaryWriter, BinaryReader>> { new BinaryStateDataHandler() }
            };
            var serializer = new BinarySerializer(_mappingRegistry, binaryConfig);
            var deserializer = new BinaryDeserializer(_mappingRegistry, binaryConfig);
            var entityTransformer = new EntityDataTransformer(serializer, deserializer, _eventSystem);

            var applicationDatabase = new ApplicationDatabase();
            var entityCount = 1000;

            for (var i = 0; i < entityCount; i++)
            {
                var entity = new Entity(Guid.NewGuid(), _eventSystem);
                entity.ApplyBlueprint(new PlayerBlueprint("Player " + i, i));

                var entityData = (EntityData)entityTransformer.TransformTo(entity);
                applicationDatabase.EntityData.Add(entityData);
            }

            var startTime = DateTime.Now;
            var output = serializer.Serialize(applicationDatabase);
            var endTime = DateTime.Now;
            var totalTime = endTime - startTime;
            var averageMsPerEntity = totalTime.TotalMilliseconds / entityCount;
            Console.WriteLine("Total Data {0} bytes", output.AsBytes.Length);
            Console.WriteLine("Serialized {0} Entities In {1} - Averaging {2:F2}ms Per Entity", entityCount, totalTime, averageMsPerEntity);

            startTime = DateTime.Now;
            deserializer.Deserialize<ApplicationDatabase>(output);
            endTime = DateTime.Now;
            totalTime = endTime - startTime;
            averageMsPerEntity = totalTime.TotalMilliseconds / entityCount;
            Console.WriteLine("Deserialized {0} Entities In {1} - Averaging {2:F2}ms Per Entity", entityCount, totalTime, averageMsPerEntity);
        }
    }
}