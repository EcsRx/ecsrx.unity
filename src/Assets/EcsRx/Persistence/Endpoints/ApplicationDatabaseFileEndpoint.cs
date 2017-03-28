using System;
using System.IO;
using Persistity;
using UnityEngine.SceneManagement;

namespace EcsRx.Persistence.Endpoints
{
    public class ApplicationDatabaseFileEndpoint : IApplicationConfigFileEndpoint
    {
        private string _configFile;
        public Scene Scene { get; private set; }

        public ApplicationDatabaseFileEndpoint(Scene scene)
        {
            Scene = scene;
            _configFile = string.Format("{0}/{1}.database.json", scene.path, scene.name);
        }

        public void Execute(DataObject data, Action<object> onSuccess, Action<Exception> onError)
        {
            try
            { File.WriteAllText(_configFile, data.AsString); }
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
                var byteData = File.ReadAllText(_configFile);
                data = new DataObject(byteData);
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