using System;
using System.IO;
using Persistity.Serialization;

namespace Persistity.Endpoints.Files
{
    public class ReadFileEndpoint : IReceiveDataEndpoint
    {
        public string FilePath { get; set; }

        public ReadFileEndpoint(string filePath)
        {
            FilePath = filePath;
        }

        public void Execute(Action<DataObject> onSuccess, Action<Exception> onError)
        {
            DataObject data;
            try
            {
                var byteData = File.ReadAllBytes(FilePath);
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