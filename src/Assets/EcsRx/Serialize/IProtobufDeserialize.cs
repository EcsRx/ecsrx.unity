using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Serialize
{
    public interface IProtobufDeserialize : IDeserialize
    {
        object Deserialize(System.Type type, Stream source);
    }
}
