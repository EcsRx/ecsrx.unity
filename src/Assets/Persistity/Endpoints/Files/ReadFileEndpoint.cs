using System;
using System.IO;

namespace Persistity.Endpoints.Files
{
    public class ReadFileEndpoint : IReceiveDataEndpoint
    {
        public string FilePath { get; set; }

        public ReadFileEndpoint(string filePath)
        {
            FilePath = filePath;
        }

        public void Execute(Action<byte[]> onSuccess, Action<Exception> onError)
        {
            byte[] data;
            try
            {
                data = File.ReadAllBytes(FilePath);
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