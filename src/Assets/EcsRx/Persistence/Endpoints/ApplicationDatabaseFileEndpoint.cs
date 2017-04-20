using System;
using System.IO;
using Persistity;
using UnityEngine.SceneManagement;

namespace EcsRx.Persistence.Endpoints
{
    public class ApplicationDatabaseFileEndpoint : IApplicationDatabaseFileEndpoint
    {
        private readonly string _databaseFile;
        public Scene Scene { get; private set; }

        public ApplicationDatabaseFileEndpoint(Scene scene)
        {
            Scene = scene;

            var scenePath = scene.path.Replace(scene.name + ".unity", "");
            _databaseFile = string.Format("{0}/{1}.database.json", scenePath, scene.name);
        }

        public void Execute(DataObject data, Action<object> onSuccess, Action<Exception> onError)
        {
            try
            { File.WriteAllText(_databaseFile, data.AsString); }
            catch (Exception ex)
            {
                onError(ex);
                return;
            }

            onSuccess(null);
        }

        public void Execute(Action<DataObject> onSuccess, Action<Exception> onError)
        {
            DataObject data;
            try
            {
                var rawData = "";

                if (File.Exists(_databaseFile))
                { rawData = File.ReadAllText(_databaseFile); }

                data = new DataObject(rawData);
            }
            catch (Exception ex)
            {
                onError(ex);
                return;
            }
            onSuccess(data);
        }
    }
}