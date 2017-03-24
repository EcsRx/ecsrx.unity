using UnityEngine;
using encoder = System.Text.Encoding;

namespace Persistity.Processors.Encoding
{
    public class UrlEncodeProcessor : IProcessor
    {
        public byte[] Process(byte[] data)
        {
            var dataAsString = encoder.UTF8.GetString(data);
            var escapedData = WWW.EscapeURL(dataAsString);
            return encoder.UTF8.GetBytes(escapedData);
        }
    }
}