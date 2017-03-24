using UnityEngine;

namespace Persistity.Processors.Encoding
{
    public class UrlDecodeProcessor : IProcessor
    {
        public byte[] Process(byte[] data)
        {
            var dataAsString = System.Text.Encoding.UTF8.GetString(data);
            var escapedData = WWW.UnEscapeURL(dataAsString);
            return System.Text.Encoding.UTF8.GetBytes(escapedData);
        }
    }
}