using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Serialize
{
    public class ProtobufDeserialize : IProtobufDeserialize
    {
        public T Deserialize<T>(string data)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(Type type, Stream source)
        {
             return ProtoBuf.Serializer.NonGeneric.Deserialize(type, source);
        }
    }
}
