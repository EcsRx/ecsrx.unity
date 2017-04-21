using System;
using EcsRx.Persistence.Endpoints;
using Persistity.Serialization.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcsRx.Persistence.Database.Accessor
{
    public class ApplicationDatabaseAccessor : IApplicationDatabaseAccessor
    {
        public ApplicationDatabase Database { get; private set; }
        public bool HasInitialized { get; private set; }

        private readonly IApplicationDatabaseFileEndpoint _databaseFileEndpoint;
        private readonly IJsonSerializer _serializer;
        private readonly IJsonDeserializer _deserializer;

        public ApplicationDatabaseAccessor(IApplicationDatabaseFileEndpoint databaseFileEndpoint, IJsonSerializer serializer, IJsonDeserializer deserializer, ApplicationDatabase database)
        {
            _databaseFileEndpoint = databaseFileEndpoint;
            _serializer = serializer;
            _deserializer = deserializer;
            Database = database;
        }

        public void ReloadDatabase(Action onReloaded)
        {
            _databaseFileEndpoint.Execute(data =>
            {
                if (data.AsBytes.Length == 0)
                {
                    var message = string.Format("No application database located, creating new one for scene [{0}]", SceneManager.GetActiveScene().name);
                    Debug.Log(message);
                    onReloaded();
                    return;
                }

                _deserializer.DeserializeInto(data, Database);
                HasInitialized = true;
                onReloaded();
            }, exception =>
            {
                var message = string.Format("Unable to load scene database: [{0}]", exception);
                Debug.Log(message);
            });
        }

        public void ResetDatabase()
        { Database.Pools.Clear(); }

        public void PersistDatabase(Action onSaved)
        {
            var data = _serializer.Serialize(Database);
            _databaseFileEndpoint.Execute(data, _ => onSaved(), exception =>
            {
                var message = string.Format("Unable to save scene database: [{0}]", exception);
                Debug.Log(message);
            });
        }
    }
}