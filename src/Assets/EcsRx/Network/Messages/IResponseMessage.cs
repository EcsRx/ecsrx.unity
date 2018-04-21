using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Network
{
    public interface IResponseMessage<out T> : INetworkMessage where T : struct
    {
        T Data { get; }
    }
}
