using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Crypto;
using EcsRx.Serialize;
using UnityEngine;

namespace EcsRx.Network
{
    public interface IProtocol
    {
        ISerialize Serialize { get; set; }
        IDeserialize Deserialize { get; set; }

        ICrypto Crypto { get; set; }
    }
}
