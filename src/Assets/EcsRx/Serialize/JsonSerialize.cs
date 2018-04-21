using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Serialize
{
    public class JsonSerialize : IJsonSerialize
    {
        public byte[] Serialize<T>(T data)
        {
            var str = JsonExtensions.SerializeObject(data).ToString();
            return Encoding.UTF8.GetBytes(str);
        }

        public JSONNode SerializeObject(object data)
        {
            return JsonExtensions.SerializeObject(data);
        }
    }
}
