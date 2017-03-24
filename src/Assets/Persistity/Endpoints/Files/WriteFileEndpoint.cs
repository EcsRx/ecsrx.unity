using System;
using System.IO;

namespace Persistity.Endpoints.Files
{
    public class WriteFile : ISendDataEndpoint
    {
        public string FilePath { get; set; }

        public WriteFile(string filePath)
        {
            FilePath = filePath;
        }

        public void Execute(byte[] data, Action<object> onSuccess, Action<Exception> onError)
        {
            try
            { File.WriteAllBytes(FilePath, data); }
            catch (Exception ex)
            {
                onError(ex);
                return;
            }

            onSuccess(null);
        }
    }
}