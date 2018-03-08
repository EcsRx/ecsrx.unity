using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Serialize
{
    public interface IJsonSerialize : ISerialize
    {
        JSONNode SerializeObject(object target);
    }
}
