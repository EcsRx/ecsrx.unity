using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Serialize
{
    public class JsonSerialize : IJsonSerialize
    {
        public string Serialize<T>(T data)
        {
            return JsonExtensions.SerializeObject(data).ToString();
        }

        public JSONNode SerializeObject(object data)
        {
            return JsonExtensions.SerializeObject(data);
        }
    }
}
