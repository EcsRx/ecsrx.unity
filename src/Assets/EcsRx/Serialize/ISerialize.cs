using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Serialize
{
    public interface ISerialize
    {
        string Serialize<T>(T data);
    }
}
