using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Serialize
{
    public class ProtobufSerialize : IProtobufSerialize
    {
        public byte[] Serialize<T>(T data)
        {
            using (var stream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, data);
                return stream.ToArray();
            }
        }

        public byte[] Serialize(object target)
        {
            using (var stream = new MemoryStream())
            {
                ProtoBuf.Serializer.NonGeneric.Serialize(stream, target);
                return stream.ToArray();
            }
        }
    }
}
