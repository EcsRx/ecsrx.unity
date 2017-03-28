using System.Collections.Generic;
using Persistity.Json;

namespace Persistity.Serialization.Json
{
    public class JsonConfiguration : SerializationConfiguration
    {
        public IEnumerable<ITypeHandler<JSONNode, JSONNode>> TypeHandlers { get; set; }

        public static JsonConfiguration Default
        {
            get
            {
                return new JsonConfiguration
                {
                    TypeHandlers = new ITypeHandler<JSONNode, JSONNode>[0]
                };
            }
        }
    }
}