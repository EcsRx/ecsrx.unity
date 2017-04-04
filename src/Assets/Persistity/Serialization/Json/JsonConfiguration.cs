using Newtonsoft.Json.Linq;

namespace Persistity.Serialization.Json
{
    public class JsonConfiguration : SerializationConfiguration<JToken, JToken>
    {
        public static JsonConfiguration Default
        {
            get
            {
                return new JsonConfiguration
                {
                    TypeHandlers = new ITypeHandler<JToken, JToken>[0]
                };
            }
        }
    }
}