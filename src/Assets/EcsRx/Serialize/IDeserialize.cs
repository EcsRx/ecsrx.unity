using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EcsRx.Serialize
{
    public interface IDeserialize
    {
        T Deserialize<T>(string data);
        object Deserialize(Type type, MemoryStream source);
    }
}
