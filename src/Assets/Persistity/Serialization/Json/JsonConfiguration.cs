using System.Collections.Generic;
using System.Text;
using Persistity.Json;

namespace Persistity.Serialization.Json
{
    public class JsonConfiguration : SerializationConfiguration
    {
        public Encoding Encoder { get; set; }
        public IEnumerable<ITypeHandler<JSONNode, JSONNode>> TypeHandlers { get; set; }

        public static JsonConfiguration Default
        {
            get
            {
                return new JsonConfiguration
                {
                    Encoder = Encoding.Default,
                    TypeHandlers = new ITypeHandler<JSONNode, JSONNode>[0]
                };
            }
        }
    }
}