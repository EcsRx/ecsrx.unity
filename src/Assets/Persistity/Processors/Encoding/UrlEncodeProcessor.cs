using Persistity.Serialization;
using UnityEngine;

namespace Persistity.Processors.Encoding
{
    public class UrlEncodeProcessor : IProcessor
    {
        public DataObject Process(DataObject data)
        {
            var escapedData = WWW.EscapeURL(data.AsString);
            return new DataObject(escapedData);
        }
    }
}