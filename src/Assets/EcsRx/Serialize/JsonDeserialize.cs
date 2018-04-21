using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Serialize
{
    public class JsonDeserialize : IJsonDeserialize
    {
        public T Deserialize<T>(string data)
        {
            return JsonExtensions.DeserializeObject<T>(data);
        }

        public object Deserialize(Type type, MemoryStream source)
        {
            string data = Encoding.UTF8.GetString(source.ToArray());
            return JsonExtensions.DeserializeObject(type, JSON.Parse(data));
        }

        public object Deserialize(Type type, JSONNode data)
        {
            //string data = Encoding.UTF8.GetString(source.ToArray());
            return JsonExtensions.DeserializeObject(type, data);
        }
    }
}
