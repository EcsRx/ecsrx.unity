using Persistity.Serialization;
using UnityEngine;

namespace Persistity.Processors.Encoding
{
    public class UrlDecodeProcessor : IProcessor
    {
        public DataObject Process(DataObject data)
        {
            var escapedData = WWW.UnEscapeURL(data.AsString);
            return new DataObject(escapedData);
        }
    }
}