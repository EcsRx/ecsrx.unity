using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Persistence.Database;
using EcsRx.Persistence.Database.Accessor;
using EcsRx.Persistence.Endpoints;
using EcsRx.Persistence.Pipelines;
using EcsRx.Persistence.Transformers;
using EcsRx.Persistence.TypeHandlers.Reactive;
using EcsRx.Pools;
using EcsRx.Systems.Executor;
using EcsRx.Systems.Executor.Handlers;
using EcsRx.Unity.Systems;
using Newtonsoft.Json.Linq;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Persistity.Registries;
using Persistity.Serialization;
using Persistity.Serialization.Binary;
using Persistity.Serialization.Json;
using Persistity.Serialization.Xml;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using TypeAnalyzer = Persistity.Mappings.Types.TypeAnalyzer;

namespace EcsRx.Unity.Installers
{
    public class EcsRxInstaller : Installer
    {
        public override void InstallBindings()
        {
            SetupEcsRx();
            SetupPersistance();
            SetupApplicationDatabase();
        }

        private void SetupEcsRx()
        {
            Container.Bind<IMessageBroker>().To<MessageBroker>().AsSingle();
            Container.Bind<IEventSystem>().To<EventSystem>().AsSingle();

            Container.Bind<IEntityFactory>().To<DefaultEntityFactory>().AsSingle();
            Container.Bind<IPoolFactory>().To<DefaultPoolFactory>().AsSingle();
            Container.Bind<IGroupAccessorFactory>().To<DefaultGroupAccessorFactory>().AsSingle();

            Container.Bind<IPoolManager>().To<PoolManager>().AsSingle();
            Container.Bind<IViewHandler>().To<ViewHandler>().AsSingle();

            Container.Bind<IReactToDataSystemHandler>().To<ReactToDataSystemHandler>();
            Container.Bind<IReactToEntitySystemHandler>().To<ReactToEntitySystemHandler>();
            Container.Bind<IReactToGroupSystemHandler>().To<ReactToGroupSystemHandler>();
            Container.Bind<ISetupSystemHandler>().To<SetupSystemHandler>();
            Container.Bind<IManualSystemHandler>().To<ManualSystemHandler>();

            Container.Bind<ISystemExecutor>().To<SystemExecutor>().AsSingle();
        }

        private void SetupPersistance()
        {
            var ignoredTypes = new[] { typeof(GameObject), typeof(MonoBehaviour) };
            var analyzerConfiguration = new TypeAnalyzerConfiguration
            {
                IgnoredTypes = ignoredTypes,
                TreatAsPrimitives = new Type[] { typeof(ReactiveProperty<>) }
            };
            Container.Bind<ITypeAnalyzer>().FromInstance(new TypeAnalyzer(analyzerConfiguration)).AsSingle();
            Container.Bind<ITypeCreator>().To<TypeCreator>().AsSingle();
            Container.Bind<ITypeMapper>().To<EverythingTypeMapper>().AsSingle();
            Container.Bind<IMappingRegistry>().To<MappingRegistry>().AsSingle();

            var jsonConfiguration = new JsonConfiguration
            {
                TypeHandlers = new List<ITypeHandler<JToken, JToken>>
                {
                    new ReactiveJsonTypeHandler()
                }
            };
            Container.Bind<JsonConfiguration>().FromInstance(jsonConfiguration).AsSingle();
            Container.Bind<IJsonSerializer>().To<JsonSerializer>().AsSingle();
            Container.Bind<IJsonDeserializer>().To<JsonDeserializer>().AsSingle();

            var binaryConfiguration = new BinaryConfiguration
            {
                TypeHandlers = new List<ITypeHandler<BinaryWriter, BinaryReader>>
                {
                    new ReactiveBinaryTypeHandler()
                }
            };
            Container.Bind<BinaryConfiguration>().FromInstance(binaryConfiguration).AsSingle();
            Container.Bind<IBinarySerializer>().To<BinarySerializer>().AsSingle();
            Container.Bind<IBinaryDeserializer>().To<BinaryDeserializer>().AsSingle();

            var xmlConfiguration = new XmlConfiguration
            {
                TypeHandlers = new List<ITypeHandler<XElement, XElement>>
                {
                    new ReactiveXmlTypeHandler()
                }
            };
            Container.Bind<XmlConfiguration>().FromInstance(xmlConfiguration).AsSingle();
            Container.Bind<IXmlSerializer>().To<XmlSerializer>().AsSingle();
            Container.Bind<IXmlDeserializer>().To<XmlDeserializer>().AsSingle();

            Container.Bind<IEntityDataTransformer>().To<EntityDataTransformer>().AsSingle();
            Container.Bind<IPoolDataTransformer>().To<PoolDataTransformer>().AsSingle();
        }

        private void SetupApplicationDatabase()
        {
            var currentScene = SceneManager.GetActiveScene();

            Container.Bind<IApplicationDatabaseFileEndpoint>().FromInstance(new ApplicationDatabaseFileEndpoint(currentScene)).AsSingle();
            Container.Bind<ApplicationDatabase>().ToSelf().AsSingle();
            Container.Bind<IApplicationDatabaseAccessor>().To<ApplicationDatabaseAccessor>().AsSingle();
        }
    }
}