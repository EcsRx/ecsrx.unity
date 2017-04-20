using EcsRx.Events;
using EcsRx.Persistence.Database.Accessor;
using EcsRx.Persistence.Events;
using EcsRx.Unity.Installers;
using UnityEngine;
using Zenject;

namespace EcsRx.Persistence.Editor
{
    public static class EditorContext
    {
        private static readonly DiContainer _container;
        
        static EditorContext()
        {
            _container = new DiContainer();
            _container.Install<EcsRxInstaller>();

            LoadDatabase();
        }

        private static void LoadDatabase()
        {
            var databaseAccessor = _container.Resolve<IApplicationDatabaseAccessor>();
            var eventSystem = _container.Resolve<IEventSystem>();
            databaseAccessor.ReloadDatabase(() =>
            {
                //Debug.Log("Application Database Loaded");
                eventSystem.Publish(new ApplicationDatabaseLoadedEvent());
            });
        }

        public static DiContainer Container
        {
            get { return _container; }
        }
    }
}